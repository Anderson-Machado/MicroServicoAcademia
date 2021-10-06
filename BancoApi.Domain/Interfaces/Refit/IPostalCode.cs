using Refit;
using System.Threading.Tasks;

namespace BancoApi.Domain.Interfaces.Refit
{
    public interface IPostalCode
    {
        [Get("/{cep}/json")]
        Task<PostalCode> GetData([AliasAs("cep")] string Cep);
    }
}
