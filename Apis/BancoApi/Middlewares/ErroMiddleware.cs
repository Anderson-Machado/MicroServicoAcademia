using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;


namespace BancoApi.Middlewares
{
    public class ErroMiddleware : IMiddleware
    {
        private readonly ILogger<ErroMiddleware> _logger;

        public ErroMiddleware(ILogger<ErroMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Caraiii neguim, deu erro: {ex.Message}");
               await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = (int)HttpStatusCode.InternalServerError;
            var problemDetail = new ProblemDetails()
            {   Type = "****",
                Title = "one or more errors occurred.",
                Detail = exception.Message,
                Status = code,
                Instance = "http://localhost/erros"
            };

            var result = JsonConvert.SerializeObject(problemDetail);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
