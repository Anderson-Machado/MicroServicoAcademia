using BancoApi.Domain;

namespace BancoApi.Service.Send
{
    public interface IBancoCreateSender
    {
        void SendBanco(Banco customer);
    }
}
