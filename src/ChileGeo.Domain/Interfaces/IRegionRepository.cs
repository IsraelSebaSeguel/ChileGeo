using ChileGeo.Domain.Entities;

namespace ChileGeo.Domain.Interfaces;

/// <summary>Data-access contract for Region. Implemented in ChileGeo.DataAccess via stored procedures.</summary>
public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Region?> GetByIdAsync(int idRegion, CancellationToken cancellationToken = default);
}
