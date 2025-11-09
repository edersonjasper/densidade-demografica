namespace densidade_demografica.API.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for demographic density information
    /// </summary>
    /// <param name="UF">State abbreviation (e.g., SP, RJ)</param>
    /// <param name="Municipio">Municipality name</param>
    /// <param name="Populacao">Total population</param>
    /// <param name="AreaKm2">Area in square kilometers</param>
    /// <param name="HabitanteKm2">Demographic density (inhabitants per square kilometer)</param>
    public sealed record DensidadeDemograficaDTO(
        string UF,
        string Municipio,
        int Populacao,
        double AreaKm2,
        double HabitanteKm2
    );
}
