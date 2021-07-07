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
    public class GetBancoByIdQueryHandle : IRequestHandler<GetBancoByIdQuery, Banco>
    {
        private readonly IBancoRepository _bancoRepository;

        public GetBancoByIdQueryHandle(IBancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository;
        }

        public async Task<Banco> Handle(GetBancoByIdQuery request, CancellationToken cancellationToken)
        {
            return await _bancoRepository.GetOrderByIdAsync(request.Id, cancellationToken);
        }
    }
}
