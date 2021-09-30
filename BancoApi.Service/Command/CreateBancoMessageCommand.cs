using BancoApi.Domain;
using MediatR;

namespace BancoApi.Service.Command
{
    public class CreateBancoMessageCommand : IRequest
    {
        public Banco Bancos { get; set; }
    }
}
