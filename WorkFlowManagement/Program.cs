using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Minio;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using WorkFM.API.Middleware;
using WorkFM.BL.Services.Auth;
using WorkFM.BL.Services.Bases;
using WorkFM.BL.Services.Cards;
using WorkFM.BL.Services.Checklists;
using WorkFM.BL.Services.Files;
using WorkFM.BL.Services.Jobs;
using WorkFM.BL.Services.Jwt;
using WorkFM.BL.Services.Kanbans;
using WorkFM.BL.Services.Projects;
using WorkFM.BL.Services.Users;
using WorkFM.BL.Services.UserWorkspaces;
using WorkFM.BL.Services.Workspaces;
using WorkFM.Common.Configs;
using WorkFM.Common.Data;
using WorkFM.Common.Data.ContextData;
using WorkFM.Common.Lib;
using WorkFM.Common.Utils;
using WorkFM.DL.Repos.Attachments;
using WorkFM.DL.Repos.Cards;
using WorkFM.DL.Repos.Checklists;
using WorkFM.DL.Repos.Jobs;
using WorkFM.DL.Repos.Kanbans;
using WorkFM.DL.Repos.Projects;
using WorkFM.DL.Repos.UserProjects;
using WorkFM.DL.Repos.Users;
using WorkFM.DL.Repos.UserWorkspaces;
using WorkFM.DL.Repos.Workspaces;
using WorkFM.DL.Service.UnitOfWork;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

    var minioStoreConfig = builder.Configuration.GetSection("Minio").Get<MinioStoreConfig>();
    builder.Services.AddSingleton(minioStoreConfig);
    // Add Minio using the custom endpoint and configure additional settings for default MinioClient initialization
    builder.Services.AddMinio(configureClient => configureClient
        .WithEndpoint(minioStoreConfig.Endpoint)
        .WithCredentials(minioStoreConfig.AccessKey, minioStoreConfig.SecretKey)
        .WithSSL(false)
        );




    // add di jwtconfig
    builder.Services.Configure<JwtConfig>(options =>
    {
        builder.Configuration.GetSection("Jwt").Bind(options);
    });

    // add authen
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ký token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])),


                ValidateLifetime = true,
            };

        });

    //builder.Services.AddLogging(loggingBuilder =>
    //{
    //    loggingBuilder.ClearProviders();
    //    loggingBuilder.AddNLog();
    //});
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    // add auto mapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    var connectionString = builder.Configuration["ConnectionString"];
    builder.Services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(connectionString));

    builder.Services.AddScoped<IAuthBL, AuthBL>();

    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<IUserDL, UserDL>();

    builder.Services.AddScoped<IWorkspaceBL, WorkspaceBL>();
    builder.Services.AddScoped<IWorkspaceDL, WorkspaceDL>();

    builder.Services.AddScoped<IUserWorkspaceBL, UserWorkspaceBL>();
    builder.Services.AddScoped<IUserWorkspaceDL, UserWorkspaceDL>();

    builder.Services.AddScoped<IProjectBL, ProjectBL>();
    builder.Services.AddScoped<IProjectDL, ProjectDL>();

    builder.Services.AddScoped<IUserProjectDL, UserProjectDL>();

    builder.Services.AddScoped<IKanbanDL, KanbanDL>();
    builder.Services.AddScoped<IKanbanBL, KanbanBL>();

    builder.Services.AddScoped<ICardDL, CardDL>();
    builder.Services.AddScoped<ICardBL, CardBL>();

    builder.Services.AddScoped<IChecklistBL, ChecklistBL>();
    builder.Services.AddScoped<IChecklistDL, ChecklistDL>();

    builder.Services.AddScoped<IJobBL, JobBL>();
    builder.Services.AddScoped<IJobDL, JobDL>();

    builder.Services.AddScoped<IFileServvice, FileService>();
    builder.Services.AddScoped<IMinioService, MinioService>();

    builder.Services.AddScoped<IFileDL, FileDL>();

    builder.Services.AddScoped<IJwtSerivce, JwtService>();
    builder.Services.AddScoped<IContextData, ContextData>();
    builder.Services.AddScoped(typeof(IDbLogger<>), typeof(DbLog<>));
    builder.Services.AddSingleton<ProjectHub>();
    builder.Services.AddSingleton<ISystenService, SystemService>();
    builder.Services.AddSingleton<IDictionary<string, UserProjectConnection>>(opt =>
    new Dictionary<string, UserProjectConnection>());

    // add cors
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "CorsPolicy",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials()
                                      ;
                          });
    });


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSignalR();

    var app = builder.Build();
    app.UseCors("CorsPolicy");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseHttpsRedirection();


    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<JwtContextMiddleware>();

    
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<ProjectHub>("/project-socket");
    });
    app.MapControllers();
    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}