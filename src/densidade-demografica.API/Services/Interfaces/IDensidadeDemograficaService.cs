using densidade_demografica.API.Models.DTO;

namespace densidade_demografica.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for demographic density business logic
    /// </summary>
    public interface IDensidadeDemograficaService
    {
        /// <summary>
        /// Retrieves demographic density data by state
        /// </summary>
        /// <param name="uf">State abbreviation (e.g., SP, RJ)</param>
        /// <returns>Collection of demographic density DTOs for the specified state</returns>
        Task<IEnumerable<DensidadeDemograficaDTO>> GetByUF(string uf);
    }
}
