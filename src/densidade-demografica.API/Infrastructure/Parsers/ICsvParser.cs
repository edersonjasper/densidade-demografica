using densidade_demografica.API.Models;

namespace densidade_demografica.API.Infrastructure.Parsers
{
    /// <summary>
    /// Interface for CSV file parsing operations
    /// </summary>
    public interface ICsvParser
    {
        /// <summary>
        /// Parses all CSV files in the specified directory
        /// </summary>
        /// <param name="directoryPath">The directory path containing CSV files</param>
        /// <returns>Collection of parsed demographic density records</returns>
        IEnumerable<DensidadeDemografica> ParseCsvFiles(string directoryPath);
    }
}