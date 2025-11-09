using CsvHelper.Configuration;
using densidade_demografica.API.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace densidade_demografica.API.Infrastructure.Parsers
{
    /// <summary>
    /// CSV parser implementation for demographic density data
    /// </summary>
    public partial class DensidadeDemograficaCsvParser(
        ILogger<DensidadeDemograficaCsvParser> logger) : ICsvParser
    {
        private readonly ILogger<DensidadeDemograficaCsvParser> _logger = logger;

        [GeneratedRegex(@"^(?<municipio>.+?)\s*\((?<uf>[A-Z]{2})\)$", RegexOptions.Compiled)]
        private static partial Regex MunicipioUfRegex();

        /// <inheritdoc />
        public IEnumerable<DensidadeDemografica> ParseCsvFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                _logger.LogError("Directory not found: {DirectoryPath}", directoryPath);
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
            }

            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");
            var allRecords = new List<DensidadeDemografica>();

            foreach (var file in csvFiles)
            {
                try
                {
                    var records = ParseCsvFile(file);
                    allRecords.AddRange(records);
                    _logger.LogInformation("File processed: {FileName}, records: {Count}", 
                        Path.GetFileName(file), records.Count());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing file: {FileName}", file);
                }
            }

            return allRecords;
        }

        /// <summary>
        /// Parses a single CSV file
        /// </summary>
        /// <param name="filePath">The file path to parse</param>
        /// <returns>Collection of demographic density records</returns>
        private IEnumerable<DensidadeDemografica> ParseCsvFile(string filePath)
        {
            var records = new List<DensidadeDemografica>();

            using var reader = new StreamReader(filePath);
            reader.ReadLine(); // Skip header

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var record = ParseLine(line);
                if (record != null)
                {
                    records.Add(record);
                }
            }

            return records;
        }

        /// <summary>
        /// Parses a single CSV line into a demographic density record
        /// </summary>
        /// <param name="line">The CSV line to parse</param>
        /// <returns>Demographic density record or null if parsing fails</returns>
        private DensidadeDemografica? ParseLine(string line)
        {
            var parts = line.Split(',');
            
            if (parts.Length != 4)
            {
                _logger.LogWarning("Invalid line format: {Line}", line);
                return null;
            }

            var match = MunicipioUfRegex().Match(parts[0]);
            
            if (!match.Success)
            {
                _logger.LogWarning("Failed to extract municipality and UF from line: {Line}", line);
                return null;
            }

            try
            {
                string municipio = match.Groups["municipio"].Value.Trim();
                string uf = match.Groups["uf"].Value;
                int populacao = int.Parse(parts[1], CultureInfo.InvariantCulture);
                double areaKm2 = double.Parse(parts[2], CultureInfo.InvariantCulture);
                double habitanteKm2 = double.Parse(parts[3], CultureInfo.InvariantCulture);

                return new DensidadeDemografica(uf, municipio, populacao, areaKm2, habitanteKm2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing line: {Line}", line);
                return null;
            }
        }
    }
}