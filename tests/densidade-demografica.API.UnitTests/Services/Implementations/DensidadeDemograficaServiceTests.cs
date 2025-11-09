using densidade_demografica.API.Models;
using densidade_demografica.API.Repositories.Interfaces;
using densidade_demografica.API.Services.Implementations;
using densidade_demografica.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace densidade_demografica.API.UnitTests.Services.Implementations
{
    internal class DensidadeDemograficaServiceTests
    {
        private IDensidadeDemograficaService _service;
        private Mock<IDensidadeDemograficaRepository> _repositoryMock;
        private Mock<ILogger<DensidadeDemograficaService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<DensidadeDemograficaService>>();            
            _repositoryMock = new Mock<IDensidadeDemograficaRepository>();
            _service = new DensidadeDemograficaService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetByUF_ShouldReturnData_ForValidUF()
        {
            // Arrange
            string uf = "SP";
            var mockData = new List<DensidadeDemografica>
            {
                new("SP", "São Paulo", 1465454, 1521.11, 8100.5),
                new("SP", "Campinas", 121654, 795.7, 1525.3)
            };
            _repositoryMock.Setup(r => r.GetByUF(uf)).ReturnsAsync(mockData);
            // Act
            var result = await _service.GetByUF(uf);
            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.All(d => d.UF.Equals(uf, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
            _repositoryMock.Verify(r => r.GetByUF(uf), Times.Once());
        }

        [Test]
        public async Task GetByUF_ShouldReturnEmpty_ForInvalidUF()
        {
            // Arrange
            string invalidUF = "XX";
            _repositoryMock.Setup(r => r.GetByUF(invalidUF)).ReturnsAsync(new List<DensidadeDemografica>());
            // Act
            var result = await _service.GetByUF(invalidUF);
            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetByUF_shouldReturnEmpty_ForInvalidInput()
        {
            // Arrange
            string invalidUF = "";
            _repositoryMock.Setup(r => r.GetByUF(invalidUF)).ReturnsAsync(new List<DensidadeDemografica>());
            // Act
            var result = await _service.GetByUF(invalidUF);
            // Assert
            result.Should().BeEmpty();
            _repositoryMock.Verify(r => r.GetByUF(It.IsAny<string>()), Times.Never());
        }
    }
}
