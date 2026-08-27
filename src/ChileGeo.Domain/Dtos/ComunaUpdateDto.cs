using System.ComponentModel.DataAnnotations;

namespace ChileGeo.Domain.Dtos;

/// <summary>Payload used to create/update a Comuna (POST region/{idRegion}/comuna).</summary>
public class ComunaUpdateDto
{
    /// <summary>Use 0 to insert a new comuna, or an existing IdComuna to update it.</summary>
    public int IdComuna { get; set; }

    [Required(ErrorMessage = "El nombre de la comuna es obligatorio.")]
    [StringLength(128, ErrorMessage = "El nombre de la comuna no puede superar los 128 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "La superficie no puede ser negativa.")]
    public decimal? Superficie { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "La población no puede ser negativa.")]
    public long? Poblacion { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "La densidad no puede ser negativa.")]
    public decimal? Densidad { get; set; }
}
