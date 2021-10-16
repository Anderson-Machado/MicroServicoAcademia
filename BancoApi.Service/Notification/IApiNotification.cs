using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace BancoApi.Service.Notification
{
    public interface IApiNotification
    {
        object GetProblemDetail();
        ValidationResult AddProblemDetail(ValidationResult validationFailure);
        bool HasNotifications();
    }
}
