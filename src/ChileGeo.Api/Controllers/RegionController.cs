using ChileGeo.Api.Security;
using ChileGeo.Domain.Dtos;
using ChileGeo.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChileGeo.Api.Controllers;

[ApiController]
[Route("region")]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
[Produces("application/json")]
public class RegionController : ControllerBase
{
    private readonly IRegionService _regionService;

    public RegionController(IRegionService regionService)
    {
        _regionService = regionService;
    }

    /// <summary>GET /region — Listado de regiones.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RegionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RegionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var regiones = await _regionService.GetAllRegionesAsync(cancellationToken);
        return Ok(regiones);
    }

    /// <summary>GET /region/{idRegion} — Información de 1 región.</summary>
    [HttpGet("{idRegion:int}")]
    [ProducesResponseType(typeof(RegionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegionDto>> GetById(int idRegion, CancellationToken cancellationToken)
    {
        var region = await _regionService.GetRegionByIdAsync(idRegion, cancellationToken);
        return Ok(region);
    }
}
