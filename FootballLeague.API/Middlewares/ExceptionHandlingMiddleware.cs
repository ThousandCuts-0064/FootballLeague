using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballLeague.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "[{TraceId}] [{Time}] [{Type}] at [{Method}] [{Path}]:{NewLine}{Message}",
                    context.TraceIdentifier,
                    DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    e.GetType().Name,
                    context.Request.Method,
                    context.Request.Path.Value,
                    Environment.NewLine,
                    e.Message);

                var responseBody = new ProblemDetails
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                    Title = "Unexpected Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Extensions =
                    {
                        { "traceId", context.TraceIdentifier }
                    }
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(responseBody);
            }
        }
    }
}