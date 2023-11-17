using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorkFM.Common.Data.ContextData;
using WorkFM.Common.Exceptions;
using WorkFM.Common.Lib;

namespace WorkFM.API.Middleware
{
    public class JwtContextMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtContextMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public async Task Invoke(HttpContext context)
        {

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                // Endpoint allows anonymous access
                // Do something here
                await _next(context);
                return;
            }
            // Endpoint does not allow anonymous access
            // Do something else
            CreateContextDataFromJwt(context);
            await _next(context);
        }

        private void CreateContextDataFromJwt(HttpContext context)
        {
            try
            {
                string authorizationHeader = context.Request.Headers["Authorization"];
                string token = null;

                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    // Extract the token from the "Bearer " prefix
                    token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);

                    var contextData = context.RequestServices.GetService(typeof(IContextData)) as IContextData;

                    // Trích xuất thông tin từ claims để tạo đối tượng contextData
                    var username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Username")?.Value;
                    var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Email")?.Value;
                    var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
                    var fullname = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Name")?.Value;
                    contextData.UserId = Guid.Parse(userId);
                    contextData.Username = username;
                    contextData.Name = fullname;
                    contextData.Email = email;
                }
            }
            catch (Exception ex)
            {
                throw new AuthException
                {
                    ErrorMessage = "Not authorize",
                };
            }
        }
    }
}
