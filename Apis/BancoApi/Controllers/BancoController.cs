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

        /// <summary>
        /// Action to create a new banco in the database.
        /// </summary>
        /// <param name="orderModel">Model to create a new order</param>
        /// <returns>Returns the created order</returns>
        /// <response code="201">Returned if the order was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost]
        public async Task<ActionResult<Banco>> Banco(Banco banco)
        {

            try
            {
                await _mediator.Send(new CreateBancoMessageCommand
                {
                    Bancos = banco
                });

                return Created("",banco);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}
