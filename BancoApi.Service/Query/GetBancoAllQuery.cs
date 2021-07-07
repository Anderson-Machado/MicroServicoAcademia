using BancoApi.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Service.Query
{
    public class GetBancoAllQuery: IRequest<List<Banco>>
    {
    }
}
