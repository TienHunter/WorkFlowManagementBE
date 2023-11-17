using System.Net;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.Common.Lib;

namespace WorkFM.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
            private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
          
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            if (ex is BaseException baseException)
            {
                // Set the response status code and content type
                context.Response.StatusCode = (int)baseException.StatusCode;
                var exceptionRes = new ExceptionResponse
                {
                    Code = baseException.Code,
                    ErrorMessage = baseException.ErrorMessage,
                    Data = baseException.Data
                };
               
                // Convert the error response to JSON
                var responseJson = WMSJsonConvert.SerializeObject(exceptionRes);

                // Write the JSON response to the HTTP response
                await context.Response.WriteAsync(responseJson);
                return;
            }

            // Set the response status code and content type
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Write the JSON response to the HTTP response
            await context.Response.WriteAsync(WMSJsonConvert.SerializeObject(new ExceptionResponse
            {
                Code = "999",
                ErrorMessage = ex.Message,
                Data = null
            }));
        }

    }
}
