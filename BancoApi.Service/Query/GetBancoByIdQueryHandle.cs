using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Repository;
using FluentValidation;
using MediatR;
using NotificationToAPI.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Service.Query
{
    public class GetBancoByIdQueryHandle : IRequestHandler<GetBancoByIdQuery, Banco>
    {
        private readonly IBancoRepository _bancoRepository;
        private readonly IApiNotification _apiNotification;

        public GetBancoByIdQueryHandle(IBancoRepository bancoRepository, IApiNotification apiNotification)
        {
            _bancoRepository = bancoRepository;
            _apiNotification = apiNotification;
        }

        public async Task<Banco> Handle(GetBancoByIdQuery request, CancellationToken cancellationToken)
        {
            var validator = new InlineValidator<int>();

            validator.RuleFor(r => r)
                     .NotEmpty()
                     .NotNull()
                     .WithMessage("user id has invalid or default value.")
                     .WithName(nameof(request.Id));
            var result = validator.Validate(request.Id);
            if (!result.IsValid)
            {
                _apiNotification.AddProblemDetail(result);
                return null;
            }
            return await _bancoRepository.GetOrderByIdAsync(request.Id, cancellationToken);
        }
    }
}
