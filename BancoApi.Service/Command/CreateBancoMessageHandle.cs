using BancoApi.Domain.Validation;
using BancoApi.Service.Notification;
using BancoApi.Service.Send;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Service.Command
{
    public class CreateBancoMessageHandle : IRequestHandler<CreateBancoMessageCommand, Unit>
    {
        private readonly IBancoCreateSender _bancoMessage;

        private readonly IApiNotification _apiNotification;

        public CreateBancoMessageHandle(IBancoCreateSender bancoMessage, IApiNotification apiNotification)
        {
            _bancoMessage = bancoMessage;
            _apiNotification = apiNotification;
        }

        public Task<Unit> Handle(CreateBancoMessageCommand request, CancellationToken cancellationToken)
        {
            var validation = new BancoValidation().Validate(request.Bancos);
            if (!validation.IsValid)
            {
                _apiNotification.AddProblemDetail(validation);
            }
            else
            {
                _bancoMessage.SendBanco(request.Bancos);
            }
           return Unit.Task;
        }

    }
}

//AutoFixture