using BancoApi.Data.Database;
using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Data.Repository
{
    public class BancoRepository : Repository<Banco>, IBancoRepository
    {
        public BancoRepository(BancoDbContext bancoContext) : base(bancoContext)
        {
        }

        public async Task<List<Banco>> GetAllBancoAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Bancos.ToListAsync(cancellationToken);
        }

     

        public async Task<Banco> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Bancos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

       
    }
}
