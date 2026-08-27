using System.ComponentModel.DataAnnotations;

namespace ChileGeo.Web.ViewModels;

public class ComunaEditViewModel
{
    public int IdRegion { get; set; }
    public int IdComuna { get; set; }

    [Display(Name = "Nombre de la comuna")]
    [Required(ErrorMessage = "El nombre de la comuna es obligatorio.")]
    [StringLength(128)]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Superficie (km²)")]
    [Range(0, double.MaxValue, ErrorMessage = "La superficie no puede ser negativa.")]
    public decimal? Superficie { get; set; }

    [Display(Name = "Población")]
    [Range(0, long.MaxValue, ErrorMessage = "La población no puede ser negativa.")]
    public long? Poblacion { get; set; }

    [Display(Name = "Densidad (hab/km²)")]
    [Range(0, double.MaxValue, ErrorMessage = "La densidad no puede ser negativa.")]
    public decimal? Densidad { get; set; }
}
