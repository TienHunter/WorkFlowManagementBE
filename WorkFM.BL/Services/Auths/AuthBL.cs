using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.BL.Services.Jwt;
using WorkFM.BL.Services.Users;
using WorkFM.Common.Configs;
using WorkFM.Common.Data;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Dto;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Users;
using WorkFM.DL.Repos.Users;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.BL.Services.Auth
{
    public class AuthBL : IAuthBL
    {
        #region Field

        protected readonly IUserBL _userBL;
        protected readonly IUserDL _userDL;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _uow;
        private readonly JwtConfig _jwtConfig;

        public AuthBL(IUserBL userBL,IUserDL userDL, IMapper mapper, IUnitOfWork uow, IOptionsMonitor<JwtConfig> jwtConfig)
        {
            _userBL = userBL;
            _userDL = userDL;
            _mapper = mapper;
            _uow = uow;
            _jwtConfig = jwtConfig.CurrentValue;
        }


        #endregion

        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>token</returns>
        public async Task<ServiceResponse> Login(UserLogin userLogin)
        {
            switch (userLogin.LoginType)
            {
                case LoginType.LoginByUsername:
                    if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password))
                    {
                        return new ServiceResponse {
                            Success = false,
                            Code = ServiceResponseCode.Error,
                            Message = "username and password not empty" 
                        };
                    }
                    var userByUsername = await _userDL.GetUserByUsernameOrEmail(userLogin.Username);
                    if(userByUsername == null || userByUsername.Password != userLogin.Password)
                    {
                        return new ServiceResponse
                        {
                            Success = false,
                            Code = ServiceResponseCode.Error,
                            Message = "username and password invalid"
                        };
                    }
                    var tokens = await this.GenerateJwtToken(userByUsername);
                    return new ServiceResponse()
                    {
                        Success = true,
                        Message = "Login success",
                        Data = tokens
                    };
                    break;
                default:
                    return new ServiceResponse
                    {
                        Success = false,
                        Code = ServiceResponseCode.Error,
                        Message = "Invalid user login"
                    };
            };
        }

        public Task<ServiceResponse> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> Register(UserRegister userRegister)
        {
            var resCreateuser = await _userBL.CreateUser(userRegister);
            return resCreateuser;
        }


        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInteval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInteval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInteval;
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                new[]
                    {
                    new Claim("Name", user.Fullname),
                    new Claim("Email", user.Email),
                    //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Username", user.Username),
                    new Claim("UserId", user.Id.ToString()),

                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);


            //var refreshToken = Guid.NewGuid().ToString();
            //// lưu refresh token xuống database
            //var refreshTokenEntity = new RefreshToken
            //{
            //    RefreshTokenId = Guid.NewGuid(),
            //    UserId = user.UserId,
            //    Token = refreshToken,
            //    JwtId = token.Id,
            //    IsUsed = false,
            //    IsRevoked = false,
            //    IssueAt = DateTime.UtcNow,
            //    ExpiredAt = DateTime.UtcNow.AddDays(1)
            //};


            //var refreshTokenType = typeof(RefreshToken);
            //var properties = refreshTokenType.GetProperties();
            //var sql = "INSERT INTO refreshToken ( ";
            //sql += string.Join(",", properties.Select(prop => prop.Name));
            //sql += ") Values (";
            //sql += string.Join(", ", properties.Select(prop => $"@{prop.Name}"));
            //sql += ");";

            //var parameters = new Dictionary<string, object>();

            //foreach (var property in properties)
            //{
            //    parameters.Add($"@{property.Name}", property.GetValue(refreshTokenEntity));
            //}
            //await _databaseService.ExecuteByCommnandTextAsync(sql, parameters);

            return accessToken;
        }
    }

}
