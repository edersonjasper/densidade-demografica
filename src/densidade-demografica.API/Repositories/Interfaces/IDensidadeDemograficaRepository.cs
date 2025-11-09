using densidade_demografica.API.Models;

namespace densidade_demografica.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for demographic density data access
    /// </summary>
    public interface IDensidadeDemograficaRepository
    {
        /// <summary>
        /// Retrieves demographic density records by state
        /// </summary>
        /// <param name="uf">State abbreviation (e.g., SP, RJ)</param>
        /// <returns>Collection of demographic density records for the specified state</returns>
        Task<IEnumerable<DensidadeDemografica>> GetByUF(string uf);
        
        /// <summary>
        /// Loads demographic density data from CSV files in the specified directory
        /// </summary>
        /// <param name="csvDirPath">Directory path containing CSV files</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task LoadDataFromCSV(string csvDirPath);
    }
}
