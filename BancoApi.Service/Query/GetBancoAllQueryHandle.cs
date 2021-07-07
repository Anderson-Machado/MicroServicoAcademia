using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Service.Query
{
   public class GetBancoAllQueryHandle : IRequestHandler<GetBancoAllQuery, List<Banco>>
    {
        private readonly IBancoRepository _bancoRepository;

        public GetBancoAllQueryHandle(IBancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository;
        }

        public async Task<List<Banco>> Handle(GetBancoAllQuery request, CancellationToken cancellationToken)
        {
            return await _bancoRepository.GetAllBancoAsync(cancellationToken);
        }
    }
}
