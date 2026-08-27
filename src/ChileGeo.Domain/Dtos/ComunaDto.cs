namespace ChileGeo.Domain.Dtos;

public class InformacionAdicionalDto
{
    public decimal? Superficie { get; set; }
    public long? Poblacion { get; set; }
    public decimal? Densidad { get; set; }
}

public class ComunaDto
{
    public int IdComuna { get; set; }
    public int IdRegion { get; set; }
    public string? Nombre { get; set; }
    public InformacionAdicionalDto? InformacionAdicional { get; set; }
}
