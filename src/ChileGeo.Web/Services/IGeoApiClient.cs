using ChileGeo.Domain.Dtos;

namespace ChileGeo.Web.Services;

/// <summary>Adapter that exposes the ChileGeo.Api REST endpoints as a strongly-typed .NET client,
/// so MVC controllers never deal with raw HttpClient/JSON details (SRP + DIP).</summary>
public interface IGeoApiClient
{
    Task<IReadOnlyList<RegionDto>> GetRegionesAsync(CancellationToken cancellationToken = default);
    Task<RegionDto?> GetRegionAsync(int idRegion, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComunaDto>> GetComunasAsync(int idRegion, CancellationToken cancellationToken = default);
    Task<ComunaDto?> GetComunaAsync(int idRegion, int idComuna, CancellationToken cancellationToken = default);
    Task<ComunaDto?> GuardarComunaAsync(int idRegion, ComunaUpdateDto dto, CancellationToken cancellationToken = default);
}
