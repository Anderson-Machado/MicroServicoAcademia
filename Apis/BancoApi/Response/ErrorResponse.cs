using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoApi.Response
{
    public class ErrorResponse
    {
        public List<ErroModel> Erros { get; set; }
    }
}
