using BancoApi.Domain;
using MediatR;

namespace BancoApi.Message.Command
{
    public class CreateBancoCommand : IRequest<Banco>
    {
        public Banco Bancos { get; set; }
    }
}
