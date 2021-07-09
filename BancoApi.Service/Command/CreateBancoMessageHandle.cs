using BancoApi.Service.Send;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Service.Command
{
    public class CreateBancoMessageHandle : IRequestHandler<CreateBancoMessageCommand, Unit>
    {
        private readonly IBancoCreateSender _bancoMessage;


        public CreateBancoMessageHandle(IBancoCreateSender bancoMessage)
        {
            _bancoMessage = bancoMessage;
        }

        public Task<Unit> Handle(CreateBancoMessageCommand request, CancellationToken cancellationToken)
        {
            _bancoMessage.SendBanco(request.Bancos);
            return Unit.Task;
        }

    }
}
