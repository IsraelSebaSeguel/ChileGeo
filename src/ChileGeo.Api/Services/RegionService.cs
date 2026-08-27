using ChileGeo.Domain.Dtos;
using ChileGeo.Domain.Exceptions;
using ChileGeo.Domain.Interfaces;

namespace ChileGeo.Api.Services;

/// <summary>Orchestrates Region use cases and maps entities to DTOs, keeping controllers thin (SRP).</summary>
public class RegionService : IRegionService
{
    private readonly IRegionRepository _regionRepository;
    private readonly ILogger<RegionService> _logger;

    public RegionService(IRegionRepository regionRepository, ILogger<RegionService> logger)
    {
        _regionRepository = regionRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<RegionDto>> GetAllRegionesAsync(CancellationToken cancellationToken = default)
    {
        var regiones = await _regionRepository.GetAllAsync(cancellationToken);
        _logger.LogInformation("Se obtuvieron {Count} regiones.", regiones.Count());
        return regiones.Select(ToDto);
    }

    public async Task<RegionDto> GetRegionByIdAsync(int idRegion, CancellationToken cancellationToken = default)
    {
        var region = await _regionRepository.GetByIdAsync(idRegion, cancellationToken)
            ?? throw new NotFoundException($"No se encontró la región con IdRegion={idRegion}.");

        return ToDto(region);
    }

    private static RegionDto ToDto(Domain.Entities.Region region) => new()
    {
        IdRegion = region.IdRegion,
        Nombre = region.Nombre
    };
}
