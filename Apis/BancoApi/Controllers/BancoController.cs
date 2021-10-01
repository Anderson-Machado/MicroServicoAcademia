using BancoApi.Domain;
using BancoApi.Service.Command;
using BancoApi.Service.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BancoController : ControllerBase
    {
        private readonly ILogger<BancoController> _logger;
        private readonly IMediator _mediator;

        public BancoController(ILogger<BancoController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Banco>> Get(int id)
        {
            try
            {
                return await _mediator.Send(new GetBancoByIdQuery
                {
                    Id = id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Banco>>> GetAll(int id)
        {
            try
            {
                return await _mediator.Send(new GetBancoAllQuery());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
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

            return Created("", banco);

        }

        [HttpGet("teste")]
        public ActionResult Teste()
        {
            return Ok("vai vendo");
        }

    }

}
