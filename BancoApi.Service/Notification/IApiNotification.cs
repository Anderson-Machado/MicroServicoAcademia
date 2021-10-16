using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace BancoApi.Service.Notification
{
    public interface IApiNotification
    {
        object GetProblemDetail(IHttpContextAccessor httpContext);
        ValidationResult AddProblemDetail(ValidationResult validationFailure);
        bool HasNotifications();
    }
}
