using BancoApi.Domain;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BancoApi.Test
{
    [Collection(nameof(BancoCollection))]

    public class BancoCollection : ICollectionFixture<BancoTestFixture> { }
    public class BancoTestFixture : IDisposable
    {

        public IEnumerable<Banco> GerarBancos(int quantidade)
        {
            var bancos = new Faker<Banco>("pt_BR").CustomInstantiator(b => new Banco(
                1,
                b.Company.CompanyName()
                )).RuleFor(c => c.Nome, (b, c) => b.Internet.UserName(c.Nome.ToLower()));
            return bancos.Generate(quantidade);
        }

        public Banco GerarBanco()
        {
            return GerarBancos(1).FirstOrDefault();
        }
        public void Dispose()
        {
            
        }
    }
}
