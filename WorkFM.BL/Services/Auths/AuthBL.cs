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
using WorkFM.Common.Configs;
using WorkFM.Common.Data;
using WorkFM.Common.Models;
using WorkFM.Common.Models.Users;
using WorkFM.DL.DatabaseService;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.BL.Services.Auth
{
    public class AuthBL : IAuthBL
    {
        #region Field

        protected readonly IDatabaseService _databaseService;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _uow;
        private readonly JwtConfig _jwtConfig;

        public AuthBL(IDatabaseService databaseService, IMapper mapper, IUnitOfWork uow, IOptionsMonitor<JwtConfig> jwtConfig)
        {
            _databaseService = databaseService;
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
        public async Task<ServiceResponse> Login(string username, string password)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"@Username", username }
            };

            var sql = $"SELECT * FROM user where Username=@Username";
            var res = await _databaseService.QueryMultiByCommandText<User>(sql, parameters);
            var user = res.FirstOrDefault();

            if (user == null || user.Password != password)
            {
                return new ServiceResponse()
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }
            var tokens = await this.GenerateJwtToken(user);
            return new ServiceResponse()
            {
                Success = true,
                Message = "Login success",
                Data = tokens
            };

        }

        public Task<ServiceResponse> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> Register(UserDto userRegister)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> RenewToken(TokenModel tokenModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
            var tokenParams = new TokenValidationParameters()
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ký token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_jwtConfig.SecretKey)),
                ValidateLifetime = false, // không kiểm tra expired
            };
            try
            {
                // check 1: accessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken,tokenParams,out var validatedToken);

                // check 2: check alg
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);
                    if(!result)
                    {
                        return new ServiceResponse
                        {
                            Success = false,
                            Message = "Invalid"
                        };
                    }
                }

                //check 3: check accessToken expired
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = this.ConvertUnixTimeToDateTime(utcExpireDate);
                if(expireDate > DateTime.UtcNow)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Token has not yet expired"
                    };
                }

                //check 4: refresh token exist in DB
                var sql = "select * from refreshToken where Token = @Token";
                var parameters = new Dictionary<string, object>()
                {
                    {"@Token", tokenModel.RefreshToken }
                };
                var res = await _databaseService.QueryMultiByCommandText<RefreshToken>(sql,parameters);
                var storedToken = res.FirstOrDefault();
                if(storedToken == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "refreshToken does not exist"
                    };
                }

                //check 5: refreshToken is used
                if(storedToken.IsUsed)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "refreshToken is used"
                    };
                }

                // check 6: refreshToken is revoked
                if (storedToken.IsRevoked)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "refreshToken is revoked"
                    };
                }




                return new ServiceResponse
                {
                    Success = false,
                    Message = "Invalid"
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                };
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInteval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInteval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInteval;
        }

        private async Task<TokenModel> GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                new[]
                    {
                    new Claim(ClaimTypes.Name, user.Fullname),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Username", user.Username),
                    new Claim("UserId", user.UserId.ToString()),

                }),
                Expires = DateTime.UtcNow.AddSeconds(25),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);


            var refreshToken = Guid.NewGuid().ToString();
            // lưu refresh token xuống database
            var refreshTokenEntity = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                Token = refreshToken,
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                IssueAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(1)
            };


            var refreshTokenType = typeof(RefreshToken);
            var properties = refreshTokenType.GetProperties();
            var sql = "INSERT INTO refreshToken ( ";
            sql += string.Join(",", properties.Select(prop=>prop.Name));
            sql += ") Values (";
            sql += string.Join(", ", properties.Select(prop => $"@{prop.Name}"));
            sql += ");";

            var parameters = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                parameters.Add($"@{property.Name}",property.GetValue(refreshTokenEntity));
            }
            await _databaseService.ExecuteByCommnandTextAsync(sql,parameters);

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }

}
