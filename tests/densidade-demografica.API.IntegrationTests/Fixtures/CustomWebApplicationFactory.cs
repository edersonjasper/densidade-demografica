using densidade_demografica.API.Infrastructure.Parsers;
using densidade_demografica.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace densidade_demografica.API.IntegrationTests.Fixtures
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Configure to use test data
                var sp = services.BuildServiceProvider();
                
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var repository = scopedServices.GetRequiredService<IDensidadeDemograficaRepository>();
                
                // Load test data
                var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Fixtures", "Data");
                repository.LoadDataFromCSV(testDataPath).Wait();
            });

            builder.UseEnvironment("Testing");
        }
    }
}
