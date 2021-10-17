using BancoApi.Domain;
using BancoApi.Service.Command;
using BancoApi.Service.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationToAPI.Notification;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BancoController : MainController
    {
        private readonly ILogger<BancoController> _logger;
        private readonly IMediator _mediator;


        public BancoController(ILogger<BancoController> logger, IMediator mediator,
            IApiNotification apiNotification,IHttpContextAccessor httpContext) 
            : base(apiNotification,httpContext)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Banco>> Get(int id)
        {

            return CustomResponse(HttpStatusCode.OK, await _mediator.Send(new GetBancoByIdQuery
            {
                Id = id
            }));

        }

        [HttpGet]
        public async Task<ActionResult<List<Banco>>> GetAll(int id)
        {
            return CustomResponse(HttpStatusCode.OK, await _mediator.Send(new GetBancoAllQuery()));
        }


        [HttpPost]
        [ProducesResponseType(typeof(Banco), StatusCodes.Status200OK)]
        public async Task<ActionResult<Banco>> Banco(Banco banco)
        {

            _logger.LogInformation("passando pela controller para enviar para a fila");
            await _mediator.Send(new CreateBancoMessageCommand
            {
                Bancos = banco
            });
            return CustomResponse(HttpStatusCode.Created, banco);

        }

        [HttpGet("teste")]
        public ActionResult Teste()
        {
            return Ok("vai vendo");
        }

    }

}
