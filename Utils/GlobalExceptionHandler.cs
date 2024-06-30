using Microsoft.AspNetCore.Diagnostics;
using RandomApp1.Models;
using System.Net;

namespace RandomApp1.Utils
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);
            var error = new ErrorModel((int)StatusCodes.Status500InternalServerError,"Internal Server Error", exception.Message);
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;
        }
    }
}