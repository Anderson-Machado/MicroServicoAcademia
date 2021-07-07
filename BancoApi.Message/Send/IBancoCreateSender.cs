using BancoApi.Domain;

namespace BancoApi.Message.Send
{
    public interface IBancoCreateSender
    {
        void SendBanco(Banco customer);
    }
}
