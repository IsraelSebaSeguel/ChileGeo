using ChileGeo.Domain.Dtos;

namespace ChileGeo.Domain.Interfaces;

/// <summary>Business logic contract for Region operations, consumed by the API controllers.</summary>
public interface IRegionService
{
    Task<IEnumerable<RegionDto>> GetAllRegionesAsync(CancellationToken cancellationToken = default);
    Task<RegionDto> GetRegionByIdAsync(int idRegion, CancellationToken cancellationToken = default);
}
