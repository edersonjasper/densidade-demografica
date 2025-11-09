using densidade_demografica.API.IntegrationTests.Fixtures;
using densidade_demografica.API.IntegrationTests.Models.DTO;
using densidade_demografica.API.Models.DTO;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Net;
using System.Text.Json;


namespace densidade_demografica.API.IntegrationTests.Controllers
{
    [TestFixture]
    internal class DensidadeDemograficaControllerTests 
    {
        private HttpClient _client;


        [SetUp]
        public void SetUp()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [TestCase("AL")]
        [TestCase("al")]
        public async Task GetByUF_ShouldReturnOk_ForValidUF(string uf)
        {
            // Act
            var response = await _client.GetAsync($"/DensidadeDemografica/uf/{uf}");
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            var records = JsonSerializer.Deserialize<List<DensidadeDemograficaDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            records.Should().NotBeNullOrEmpty();
            records.Should().OnlyContain(x => x.UF == uf.ToUpper());
        }

        [Test]
        public async Task GetByUF_ShouldReturnNotFound_ForInvalidUF()
        {
            // Arrange
            string invalidUF = "XX";
            // Act
            var response = await _client.GetAsync($"/DensidadeDemografica/uf/{invalidUF}");
            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            var failResponse = JsonSerializer.Deserialize<FailResponseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            failResponse!.Message.Should().Be($"No records found for UF: {invalidUF}");
        }

        [Test]
        public async Task GetByUF_ShouldReturnBadRequest_ForInvalidUfRequest()
        {
            // Arrange
            string invalidUF = "a";
            // Act
            var response = await _client.GetAsync($"/DensidadeDemografica/uf/{invalidUF}");
            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            var failResponse = JsonSerializer.Deserialize<FailResponseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            failResponse!.Error.Should().Be($"Invalid UF. It must be a two-letter uppercase abbreviation.");
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }
    }
}
