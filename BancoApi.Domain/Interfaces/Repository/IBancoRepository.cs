using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Domain.Interfaces.Repository
{
   public interface IBancoRepository: IRepository<Banco>
    {

        Task<Banco> GetOrderByIdAsync(int Id, CancellationToken cancellationToken);

        Task<List<Banco>> GetAllBancoAsync(CancellationToken cancellationToken);
    }
}
