using System.Diagnostics.CodeAnalysis;

namespace densidade_demografica.API.Models
{
    /// <summary>
    /// Represents demographic density data for a municipality
    /// </summary>
    public sealed class DensidadeDemografica
    {
        public string UF { get; }
        public string Municipio { get; }
        public int Populacao { get; }
        public double AreaKm2 { get; }
        public double HabitanteKm2 { get; }

        public DensidadeDemografica(
            string uf, 
            string municipio, 
            int populacao, 
            double areaKm2, 
            double habitanteKm2)
        {
            if (string.IsNullOrWhiteSpace(uf) || uf.Length != 2)
                throw new ArgumentException("UF must have exactly 2 characters", nameof(uf));

            if (string.IsNullOrWhiteSpace(municipio))
                throw new ArgumentException("Municipality cannot be empty", nameof(municipio));

            if (populacao < 0)
                throw new ArgumentException("Population cannot be negative", nameof(populacao));

            if (areaKm2 <= 0)
                throw new ArgumentException("Area must be greater than zero", nameof(areaKm2));

            if (habitanteKm2 < 0)
                throw new ArgumentException("Density cannot be negative", nameof(habitanteKm2));

            UF = uf.ToUpper();
            Municipio = municipio;
            Populacao = populacao;
            AreaKm2 = areaKm2;
            HabitanteKm2 = habitanteKm2;
        }

        public bool IsUF(string uf) =>
            UF.Equals(uf, StringComparison.OrdinalIgnoreCase);

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is DensidadeDemografica other &&
            UF == other.UF &&
            Municipio == other.Municipio;

        public override int GetHashCode() =>
            HashCode.Combine(UF, Municipio);
    }
}
