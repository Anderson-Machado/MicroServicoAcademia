using BancoApi.Service.ProblemDetail;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace BancoApi.Service.Notification
{
    public class ApiNotification : IApiNotification
    {
        private ValidationResult _failuresBusiness;
        
        public ApiNotification()
        {
            
            _failuresBusiness = new ValidationResult();
        }

        public ValidationResult AddProblemDetail(ValidationResult validationFailure)
        {
            _failuresBusiness = validationFailure;
            return _failuresBusiness;
        }

       

        public object GetProblemDetail(IHttpContextAccessor httpContext)
        {
            httpContext.HttpContext.Request.Headers.TryGetValue("TraceId", out var traceId);
            var errorsModelState = _failuresBusiness.Errors.GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => new ValidationProblemDetailsError(y.ErrorCode, y.ErrorMessage)).ToList()
                    );

            var validaTion = new CustomValidationProblemDetails()
            {
                Type = "Error",
                Title = "One or more errors occurred.",
                Errors = new Dictionary<string, List<ValidationProblemDetailsError>>(errorsModelState),
                Status = (int)HttpStatusCode.BadRequest,
                TraceId = traceId.ToString()
             };

            var result = new BadRequestObjectResult(validaTion);
            return result.Value;
        }

        public bool HasNotifications() => _failuresBusiness.Errors.Count > 0;

    }
}
