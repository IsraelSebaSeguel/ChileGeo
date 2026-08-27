namespace ChileGeo.Domain.Entities;

/// <summary>Represents a "comuna" belonging to a Region.</summary>
public class Comuna
{
    public int IdComuna { get; set; }
    public int IdRegion { get; set; }
    public string? Nombre { get; set; }
    public InformacionAdicional? InformacionAdicional { get; set; }
}
