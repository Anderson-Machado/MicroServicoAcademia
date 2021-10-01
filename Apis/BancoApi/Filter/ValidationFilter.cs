
using BancoApi.ProblemDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BancoApi.Filter
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsModelState = context.ModelState.Where(x => x.Value.Errors.Count() > 0)
                    .Select(x => new { Fild = x.Key, Messages = x.Value })
                    .Select(s => new KeyValuePair<string, ValidationProblemDetailsError>
                    (s.Fild, new ValidationProblemDetailsError("xx", s.Messages.Errors.
                     Select(x => x.ErrorMessage).ToList())));

                var validaTion = new CustomValidationProblemDetails()
                {
                    Title = "one or more errors occurred.",
                    Errors = new Dictionary<string, ValidationProblemDetailsError>(errorsModelState),
                    Status = (int)HttpStatusCode.BadRequest,
                    TraceId = context.HttpContext.TraceIdentifier
                };

               

                context.Result = new BadRequestObjectResult(validaTion);
                return;
            }
            await next();
        }
    }
}
