using BancoApi.Data.Repository;
using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Repository;
using BancoApi.Service.Send;
using Moq;
using Xunit;

namespace BancoApi.Test
{
    [Collection(nameof(BancoCollection))]
    public class UnitTest1
    {
        private readonly BancoTestFixture _bancoTestsFixture;

        public UnitTest1(BancoTestFixture bancoTestsFixture)
        {
            _bancoTestsFixture = bancoTestsFixture;
        }

        [Fact(DisplayName = "Validando se a criação do banco é válida")]
        public void Test1()
        {
            var bancosRepo = new Mock<IBancoRepository>();

            var db = new Mock<IBancoCreateSender>();

            Assert.NotNull(banco);
        }
    }
}
