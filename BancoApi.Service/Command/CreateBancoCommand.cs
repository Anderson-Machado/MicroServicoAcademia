using BancoApi.Domain;
using MediatR;

namespace BancoApi.Service.Command
{
    public class CreateBancoCommand : IRequest<Banco>
    {
        public Banco Bancos { get; set; }
    }
}
