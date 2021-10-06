using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Refit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PostalCodeController: ControllerBase
    {
        private readonly ILogger<PostalCodeController> _logger;
        private readonly IPostalCode _postalCode;
        public PostalCodeController(ILogger<PostalCodeController> logger, IPostalCode postalCode)
        {
            _postalCode = postalCode;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PostalCode>> Get([FromQuery]string cep)
        {
            try
            {
                return Ok(await _postalCode.GetData(cep));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
