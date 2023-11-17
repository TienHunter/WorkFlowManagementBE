using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.BL.Services.Workspaces;
using WorkFM.Common.Data.Projects;
using WorkFM.Common.Data.UserProjects;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.Common.Lib;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.Projects;
using WorkFM.DL.Repos.UserProjects;
using WorkFM.DL.Repos.UserWorkspaces;

namespace WorkFM.BL.Services.Projects
{
    public class ProjectBL:BaseBL<ProjectDto, Project>, IProjectBL
    {
        private readonly IProjectDL _projectDL;
        private readonly IDbLogger<ProjectBL> _logger;
        private readonly IUserWorkspaceDL _userWorkspaceDL;
        private readonly IUserProjectDL _userProjectDL;

        public ProjectBL(IServiceProvider serviceProvider, IProjectDL projectDL) : base(serviceProvider, projectDL)
        {
            _projectDL = projectDL;
            _logger = serviceProvider.GetService(typeof(IDbLogger<ProjectBL>)) as IDbLogger<ProjectBL>;
            _userWorkspaceDL = serviceProvider.GetService(typeof(IUserWorkspaceDL)) as IUserWorkspaceDL;
            _userProjectDL= serviceProvider.GetService(typeof(IUserProjectDL)) as IUserProjectDL;
        }

        public async Task<ServiceResponse> CreateAsync(ProjectCreateDto projectCreateDto)
        {
            // validate người dùng có ở trong không gian làm việc không
            var parameters = new Dictionary<string, object>()
            {
                 {"@UserId",_contextData.UserId },
                {"@WorkspaceId", projectCreateDto.WorkspaceId}
            };
            _ = await _userWorkspaceDL.GetByUserIdAndWorkspaceId(parameters) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                ErrorMessage= "Unauthorized"
            };
            // mapping
            var project = _mapper.Map<Project>(projectCreateDto);
            base.BeforeCreate<Project>(ref project);
            project.UserId = _contextData.UserId;

            // create entity user_project
            var userProject = new UserProject
            {
                UserId = _contextData.UserId,
                ProjectId = project.Id,
                UserRole = Common.Enums.UserRole.Admin
            };
            base.BeforeCreate<UserProject>(ref userProject);

            try
            {
                await _uow.BeginTransactionAsync();

                await _projectDL.CreateAsync(project);
                await _userProjectDL.CreateAsync(userProject);

                await _uow.CommitAsync();

            }catch(Exception ex)
            {
                await _uow.RollbackAsync();
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            // map entity -> dto
            var projectDto = _mapper.Map<ProjectDto>(project);

            return new ServiceResponse
            {
                Data = projectDto
            };
        }

        public async Task<ServiceResponse> GetProjectsInWorkspaceAsync(Guid id)
        {
            // check user joined workspace
            var parameters = new Dictionary<string, object>()
            {
                {"@UserId", _contextData.UserId },
                {"@WorkspaceId", id }
            };
            _ = await _userWorkspaceDL.GetByUserIdAndWorkspaceId(parameters) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                ErrorMessage = "Unauthorized"
            };

            var res = await _projectDL.GetProjectsInWorkspaceAsync(parameters);

            return new ServiceResponse { Data = res };
        }

        /// <summary>
        /// update project info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="BaseException"></exception>
        public async Task<ServiceResponse> UpdateAsync(Guid id, ProjectUpdateDto projectUpdateDto)
        {
            projectUpdateDto.Id = id;
            // check exist
            var existProject = await _projectDL.GetByIdAsync(id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found"
            };

            // check authen
            var parameters = new Dictionary<string, object>
            {
                {"@ProjectId",id },
                {"@UserId", _contextData.UserId}
            };
            var userProject = await _userProjectDL.GetByProjectIdAndUserId(parameters);
            if (userProject == null || userProject.UserRole != Common.Enums.UserRole.Admin) throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                ErrorMessage = "không có quyền cập nhật project này"
            };

            _mapper.Map<ProjectUpdateDto, Project>(projectUpdateDto, existProject);
            // update
            if (await _projectDL.UpdateAsync(existProject) == 0) throw new BaseException
            {
                ErrorMessage = "Cập nhật thất bại"
            };
            // map enitity -> dto ??

            return new ServiceResponse
            {
                Data = existProject
            };

        }
    }
}
