using densidade_demografica.API.Models.DTO;
using densidade_demografica.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace densidade_demografica.API.Controllers
{
    /// <summary>
    /// Provides endpoints for retrieving demographic density data.
    /// </summary>
    /// <remarks>This controller is responsible for handling requests related to demographic density
    /// information. It interacts with the underlying service to fetch data based on the specified parameters.</remarks>
    /// <param name="_service"></param>

    [ApiController]
    [Route("[controller]")]
    public class DensidadeDemograficaController (IDensidadeDemograficaService _service, ILogger<DensidadeDemograficaController> _logger) : ControllerBase
    {


        /// <summary>
        /// Retrieves a collection of demographic density data for the specified state.
        /// </summary>        
        /// <param name="uf">The two-letter abbreviation of the state for which to retrieve demographic density data. Must not be null or empty. (ex: AL, AC)</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="IEnumerable{T}"/> of <see
        /// cref="DensidadeDemograficaDTO"/> objects  representing the demographic density data for the specified state.</returns>
        [HttpGet("uf/{uf}")]
        [ProducesResponseType(typeof(IEnumerable<DensidadeDemograficaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DensidadeDemograficaDTO>>> Get([FromRoute][Required] string uf)
        {
            try
            {
                if (string.IsNullOrEmpty(uf) || uf.Length != 2)
                {
                    return BadRequest(new { error = "Invalid UF. It must be a two-letter uppercase abbreviation." });
                }
                var records = await _service.GetByUF(uf.ToUpper());
                if (records == null || !records.Any())
                {
                    return NotFound(new { message = $"No records found for UF: {uf}" });
                }
                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GetByUF request for UF: {UF}", uf);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }
    }
}
