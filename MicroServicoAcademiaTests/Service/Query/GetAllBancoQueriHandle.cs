using AutoFixture;
using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Repository;
using BancoApi.Service.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MicroServicoAcademiaTests.Service.Query
{
    public class GetAllBancoQueriHandle
    {

        private readonly Mock<IBancoRepository> _mockBancoRepositor;

        private readonly GetBancoAllQueryHandle _getBancoAllQueryHandle;

        private readonly Fixture _fixture;

        private readonly CancellationTokenSource _cancellationTokenSource;


        public GetAllBancoQueriHandle()
        {
            _mockBancoRepositor = new Mock<IBancoRepository>();
            _getBancoAllQueryHandle = new GetBancoAllQueryHandle(_mockBancoRepositor.Object);
            _fixture = new Fixture();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [Fact]
        public async Task Sucesso()
        {
            // Arrange
            var listBancos = _fixture.Create<List<Banco>>();
            var request = _fixture.Create<GetBancoAllQuery>();
            // Act
            _mockBancoRepositor.Setup(x => x.GetAllBancoAsync(_cancellationTokenSource.Token)).Returns(Task.FromResult(listBancos));
            var result = _getBancoAllQueryHandle.Handle(request, _cancellationTokenSource.Token);
            await result;
            // Assert 
            Assert.True(result.IsCompleted);
            Assert.Null(result.Exception);
            Assert.True(result.Result.Count > 0);
            _mockBancoRepositor.Verify(x => x.GetAllBancoAsync(_cancellationTokenSource.Token), Times.Once);
        }

        [Fact]
        public async Task Falha()
        {
            // Arrange
            var listBancos = new List<Banco>();
            var request = _fixture.Create<GetBancoAllQuery>();
            // Act
            _mockBancoRepositor.Setup(x => x.GetAllBancoAsync(_cancellationTokenSource.Token)).Returns(Task.FromResult(listBancos));
            var result = _getBancoAllQueryHandle.Handle(request, _cancellationTokenSource.Token);
            await result;
            // Assert 
            Assert.True(result.IsCompleted);
            Assert.Null(result.Exception);
            Assert.True(result.Result.Count == 0);
            _mockBancoRepositor.Verify(x => x.GetAllBancoAsync(_cancellationTokenSource.Token), Times.Once);
        }

    }
}
