using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationToAPI.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IApiNotification _apiNotification;
        private readonly IHttpContextAccessor _httpContext;

        public MainController(IApiNotification apiNotification, IHttpContextAccessor httpContext)
        {
            _apiNotification = apiNotification;
            _httpContext = httpContext;
        }

        protected ActionResult CustomResponse(HttpStatusCode httpStatusCode, object result = null)
        {
            if (_apiNotification.HasNotifications())
            {
                return BadRequest(_apiNotification.GetProblemDetail(_httpContext));
            }
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                     return Ok(result);
                    break;
                case HttpStatusCode.Created:
                    return Created("", result);
                    break;

                default:
                  return Ok(result);
            }
           
        }
    }
}
