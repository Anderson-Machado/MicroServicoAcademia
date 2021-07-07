using BancoApi.Domain;
using MediatR;

namespace BancoApi.Service.Query
{
    public class GetBancoByIdQuery : IRequest<Banco>
    {
        public int Id { get; set; }
    }
}
