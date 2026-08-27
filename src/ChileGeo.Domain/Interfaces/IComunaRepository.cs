using ChileGeo.Domain.Entities;

namespace ChileGeo.Domain.Interfaces;

/// <summary>Data-access contract for Comuna. Implemented in ChileGeo.DataAccess via stored procedures.</summary>
public interface IComunaRepository
{
    Task<IEnumerable<Comuna>> GetByRegionAsync(int idRegion, CancellationToken cancellationToken = default);
    Task<Comuna?> GetByIdAsync(int idComuna, CancellationToken cancellationToken = default);

    /// <summary>Inserts or updates a comuna using a MERGE statement and returns the persisted entity.</summary>
    Task<Comuna> MergeAsync(Comuna comuna, CancellationToken cancellationToken = default);
}
