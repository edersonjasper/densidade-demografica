using densidade_demografica.API.Models.DTO;
using densidade_demografica.API.Repositories.Interfaces;
using densidade_demografica.API.Services.Interfaces;

namespace densidade_demografica.API.Services.Implementations
{
    /// <summary>
    /// Service implementation for demographic density business logic
    /// </summary>
    public class DensidadeDemograficaService(
        IDensidadeDemograficaRepository repository,
        ILogger<DensidadeDemograficaService> logger) : IDensidadeDemograficaService
    {
        private readonly IDensidadeDemograficaRepository _repository = repository;
        private readonly ILogger<DensidadeDemograficaService> _logger = logger;

        /// <inheritdoc />
        public async Task<IEnumerable<DensidadeDemograficaDTO>> GetByUF(string uf)
        {
            if (string.IsNullOrWhiteSpace(uf))
            {
                _logger.LogWarning("GetByUF called with null or empty UF");
                return Enumerable.Empty<DensidadeDemograficaDTO>();
            }

            var records = await _repository.GetByUF(uf);
            var dtoRecords = records.Select(MapToDto).ToList();
            
            _logger.LogDebug("Retrieved {Count} records for UF: {UF}", dtoRecords.Count, uf);
            
            return dtoRecords;
        }

        private static DensidadeDemograficaDTO MapToDto(Models.DensidadeDemografica entity) =>
            new(entity.UF, entity.Municipio, entity.Populacao, entity.AreaKm2, entity.HabitanteKm2);
    }
}
