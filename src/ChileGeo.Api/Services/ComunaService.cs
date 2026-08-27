using ChileGeo.Domain.Dtos;
using ChileGeo.Domain.Entities;
using ChileGeo.Domain.Exceptions;
using ChileGeo.Domain.Interfaces;

namespace ChileGeo.Api.Services;

/// <summary>Orchestrates Comuna use cases: listing, retrieval and MERGE-based updates (SRP + DIP).</summary>
public class ComunaService : IComunaService
{
    private readonly IComunaRepository _comunaRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ILogger<ComunaService> _logger;

    public ComunaService(IComunaRepository comunaRepository, IRegionRepository regionRepository, ILogger<ComunaService> logger)
    {
        _comunaRepository = comunaRepository;
        _regionRepository = regionRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ComunaDto>> GetComunasByRegionAsync(int idRegion, CancellationToken cancellationToken = default)
    {
        await EnsureRegionExistsAsync(idRegion, cancellationToken);

        var comunas = await _comunaRepository.GetByRegionAsync(idRegion, cancellationToken);
        return comunas.Select(ToDto);
    }

    public async Task<ComunaDto> GetComunaByIdAsync(int idRegion, int idComuna, CancellationToken cancellationToken = default)
    {
        await EnsureRegionExistsAsync(idRegion, cancellationToken);

        var comuna = await _comunaRepository.GetByIdAsync(idComuna, cancellationToken);
        if (comuna is null || comuna.IdRegion != idRegion)
        {
            throw new NotFoundException($"No se encontró la comuna IdComuna={idComuna} en la región IdRegion={idRegion}.");
        }

        return ToDto(comuna);
    }

    public async Task<ComunaDto> UpdateComunaAsync(int idRegion, ComunaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureRegionExistsAsync(idRegion, cancellationToken);

        if (dto.IdComuna > 0)
        {
            var existente = await _comunaRepository.GetByIdAsync(dto.IdComuna, cancellationToken);
            if (existente is null || existente.IdRegion != idRegion)
            {
                throw new NotFoundException($"No se encontró la comuna IdComuna={dto.IdComuna} en la región IdRegion={idRegion}.");
            }
        }

        var comuna = new Comuna
        {
            IdComuna = dto.IdComuna,
            IdRegion = idRegion,
            Nombre = dto.Nombre,
            InformacionAdicional = new InformacionAdicional
            {
                Superficie = dto.Superficie,
                Poblacion = dto.Poblacion,
                Densidad = dto.Densidad
            }
        };

        var merged = await _comunaRepository.MergeAsync(comuna, cancellationToken);
        _logger.LogInformation("Comuna {IdComuna} de la región {IdRegion} actualizada mediante MERGE.", merged.IdComuna, idRegion);

        return ToDto(merged);
    }

    private async Task EnsureRegionExistsAsync(int idRegion, CancellationToken cancellationToken)
    {
        var region = await _regionRepository.GetByIdAsync(idRegion, cancellationToken);
        if (region is null)
        {
            throw new NotFoundException($"No se encontró la región con IdRegion={idRegion}.");
        }
    }

    private static ComunaDto ToDto(Comuna comuna) => new()
    {
        IdComuna = comuna.IdComuna,
        IdRegion = comuna.IdRegion,
        Nombre = comuna.Nombre,
        InformacionAdicional = comuna.InformacionAdicional is null
            ? null
            : new InformacionAdicionalDto
            {
                Superficie = comuna.InformacionAdicional.Superficie,
                Poblacion = comuna.InformacionAdicional.Poblacion,
                Densidad = comuna.InformacionAdicional.Densidad
            }
    };
}
