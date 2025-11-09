using densidade_demografica.API.Infrastructure.Parsers;
using densidade_demografica.API.Repositories.Implementations;
using densidade_demografica.API.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace densidade_demografica.API.IntegrationTests.Repositories.Implementations
{
    internal class DensidadeDemograficaCSVRepositoryTests
    {
        private IDensidadeDemograficaRepository repository;
        private string _directoryPath = Path.Combine("Fixtures", "Data");

        [SetUp]
        public void Setup()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            repository = new DensidadeDemograficaCSVRepository(new DensidadeDemograficaCsvParser(loggerFactory.CreateLogger<DensidadeDemograficaCsvParser>()), loggerFactory.CreateLogger<DensidadeDemograficaCSVRepository>());
            repository.LoadDataFromCSV(_directoryPath);
        }

        [TestCase("AL", 102)]
        [TestCase("AC", 22)]
        public async Task GetByUF_ShouldReturnData_ForValidUF(string uf, int count)
        {

            // Act
            var result = await repository.GetByUF(uf);
            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(count);
            result.All(d => d.UF.Equals(uf, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
        }

        [Test]
        public async Task GetByUF_ShouldReturnEmpty_ForInvalidUF()
        {
            // Arrange
            string invalidUF = "XX";
            // Act
            var result = await repository.GetByUF(invalidUF);
            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task ReloadDataFromCSV_ShouldThrowException_ForInvalidPath()
        {
            // Arrange
            string invalidPath = "NonExistentPath";
            // Act
            Func<Task> act = async () => await repository.LoadDataFromCSV(invalidPath);
            // Assert
            await act.Should().ThrowAsync<DirectoryNotFoundException>();
        }

        [Test]
        public async Task ReloadDataFromCSV_NotShould_ReturnDuplicateRecords()
        {
            // Arrange
            await repository.LoadDataFromCSV(_directoryPath);
            // Act
            var allData = await repository.GetByUF("AL");
            // Assert
            allData.GroupBy(d => new { d.UF, d.Municipio })
                   .All(g => g.Count() == 1)
                   .Should().BeTrue();
        }
    }
}
