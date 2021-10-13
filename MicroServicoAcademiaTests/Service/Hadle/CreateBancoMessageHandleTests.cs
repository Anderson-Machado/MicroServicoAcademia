using AutoFixture;
using BancoApi.Service.Command;
using BancoApi.Service.Send;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MicroServicoAcademiaTests.Service.Hadle
{
    public class CreateBancoMessageHandleTests
    {
        private readonly Mock<IBancoCreateSender> _bancoCreateSender;
        private readonly CreateBancoMessageHandle _createBancoMessageHandle;
        private readonly Fixture _fixture;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public CreateBancoMessageHandleTests()
        {
            _bancoCreateSender = new Mock<IBancoCreateSender>();
            _createBancoMessageHandle = new CreateBancoMessageHandle(_bancoCreateSender.Object);
            _fixture = new Fixture();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [Fact]
        public async Task Sucesso()
        {
            var createBancoMessage = _fixture.Create<CreateBancoMessageCommand>();
            var sapo = _createBancoMessageHandle.Handle(createBancoMessage, _cancellationTokenSource.Token);
            await sapo;

            Assert.True(sapo.IsCompleted);
            Assert.Null(sapo.Exception);
        }
    }
}
