namespace ChileGeo.Domain.Entities;

/// <summary>Additional statistical data stored as XML in the Comuna.InformacionAdicional column.</summary>
public class InformacionAdicional
{
    public decimal? Superficie { get; set; }
    public long? Poblacion { get; set; }
    public decimal? Densidad { get; set; }
}
