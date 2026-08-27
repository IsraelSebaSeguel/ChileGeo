using ChileGeo.Domain.Dtos;

namespace ChileGeo.Domain.Interfaces;

/// <summary>Business logic contract for Comuna operations, consumed by the API controllers.</summary>
public interface IComunaService
{
    Task<IEnumerable<ComunaDto>> GetComunasByRegionAsync(int idRegion, CancellationToken cancellationToken = default);
    Task<ComunaDto> GetComunaByIdAsync(int idRegion, int idComuna, CancellationToken cancellationToken = default);
    Task<ComunaDto> UpdateComunaAsync(int idRegion, ComunaUpdateDto dto, CancellationToken cancellationToken = default);
}
