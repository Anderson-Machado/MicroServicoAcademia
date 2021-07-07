using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Service.Command
{
    public class CreateBancoCommandHandle : IRequestHandler<CreateBancoCommand, Banco>
    {
        private readonly IBancoRepository _bancoRepository;

        public CreateBancoCommandHandle(IBancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository;
        }

        public async Task<Banco> Handle(CreateBancoCommand request, CancellationToken cancellationToken)
        {
            return await _bancoRepository.AddAsync(request.Bancos);
        }
    }
}
