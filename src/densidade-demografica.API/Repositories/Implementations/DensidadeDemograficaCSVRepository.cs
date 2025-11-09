using densidade_demografica.API.Infrastructure.Parsers;
using densidade_demografica.API.Models;
using densidade_demografica.API.Repositories.Interfaces;

namespace densidade_demografica.API.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation that reads demographic density data from CSV files
    /// </summary>
    public class DensidadeDemograficaCSVRepository(
        ICsvParser csvParser,
        ILogger<DensidadeDemograficaCSVRepository> logger) : IDensidadeDemograficaRepository
    {
        private readonly ICsvParser _csvParser = csvParser;
        private readonly ILogger<DensidadeDemograficaCSVRepository> _logger = logger;
        private readonly List<DensidadeDemografica> _data = [];

        /// <inheritdoc />
        public Task LoadDataFromCSV(string csvDirPath)
        {
            try
            {
                _data.Clear();
                var records = _csvParser.ParseCsvFiles(csvDirPath);
                _data.AddRange(records);
                
                _logger.LogInformation("Total of {Count} records loaded", _data.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading CSV data from directory: {CsvDirPath}", csvDirPath);
                throw;
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<IEnumerable<DensidadeDemografica>> GetByUF(string uf)
        {
            if (string.IsNullOrWhiteSpace(uf))
            {
                return Task.FromResult(Enumerable.Empty<DensidadeDemografica>());
            }

            var results = _data.Where(x => x.IsUF(uf.ToUpper()));
            return Task.FromResult(results.AsEnumerable());
        }
    }
}
