# Brazilian Demographic Density API

A REST API built with .NET 8 to query Brazilian demographic density data from CSV files. The project follows Clean Code and SOLID principles with comprehensive testing.

## Technologies

- .NET 8 & ASP.NET Core Web API
- CsvHelper for CSV parsing
- NUnit + FluentAssertions + Moq for testing
- Swagger/OpenAPI for documentation

## Project Structure

```
densidade-demografica/
    src/densidade-demografica.API/
        Controllers/
            Services/
            Repositories/
            Infrastructure/Parsers/
            Models/
            Data/
    tests/
        densidade-demografica.API.UnitTests/
        densidade-demografica.API.IntegrationTests/
```

The architecture separates concerns into Controllers, Services, Repositories, and Infrastructure layers, following Clean Architecture principles.

## Getting Started

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
# Clone and navigate
git clone https://github.com/your-username/densidade-demografica.git
cd densidade-demografica

# Restore, build and run
dotnet restore
dotnet build
cd src/densidade-demografica.API
dotnet run
```

Access the API at `https://localhost:7176` and Swagger UI at `https://localhost:7176/swagger`

## API Usage

**Get demographic data by state:**

```http
GET /DensidadeDemografica/uf/{uf}
```

Example:
```bash
curl https://localhost:7176/DensidadeDemografica/uf/SP
```

Response:
```json
[
  {
    "uf": "SP",
    "municipio": "São Paulo",
    "populacao": 12325232,
    "areaKm2": 1521.11,
    "habitanteKm2": 8096.82
  }
]
```

## Running Tests

```bash
# All tests
dotnet test

# Unit tests only
dotnet test --filter "FullyQualifiedName~UnitTests"

# Integration tests only
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

## CSV Data Format

Place CSV files in `src/densidade-demografica.API/Data/`:

```csv
Municipality (UF),Population,Area (km²),Density (inhab/km²)
São Paulo (SP),12325232,1521.11,8096.82
Rio de Janeiro (RJ),6747815,1200.27,5621.99
```

## Architecture Highlights

- **Clean Code**: Meaningful names, small focused methods, proper error handling
- **SOLID Principles**: Single responsibility, dependency inversion via interfaces
- **Testing**: Unit tests with mocks and integration tests for full scenarios
- **Design Patterns**: Repository, Service Layer, Dependency Injection, DTO

## License

MIT License - see [LICENSE](LICENSE) for details.

## Acknowledgments

Data source: IBGE (Brazilian Institute of Geography and Statistics)
