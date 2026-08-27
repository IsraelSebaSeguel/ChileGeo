using ChileGeo.Api.Security;
using ChileGeo.Domain.Dtos;
using ChileGeo.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChileGeo.Api.Controllers;

[ApiController]
[Route("region/{idRegion:int}/comuna")]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
[Produces("application/json")]
public class ComunaController : ControllerBase
{
    private readonly IComunaService _comunaService;

    public ComunaController(IComunaService comunaService)
    {
        _comunaService = comunaService;
    }

    /// <summary>GET /region/{idRegion}/comuna — Listado de comunas de la región especificada.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ComunaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ComunaDto>>> GetByRegion(int idRegion, CancellationToken cancellationToken)
    {
        var comunas = await _comunaService.GetComunasByRegionAsync(idRegion, cancellationToken);
        return Ok(comunas);
    }

    /// <summary>GET /region/{idRegion}/comuna/{idComuna} — Información de 1 comuna.</summary>
    [HttpGet("{idComuna:int}")]
    [ProducesResponseType(typeof(ComunaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComunaDto>> GetById(int idRegion, int idComuna, CancellationToken cancellationToken)
    {
        var comuna = await _comunaService.GetComunaByIdAsync(idRegion, idComuna, cancellationToken);
        return Ok(comuna);
    }

    /// <summary>POST /region/{idRegion}/comuna — Actualiza (o inserta) la información de la comuna dada, vía MERGE.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ComunaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComunaDto>> Update(int idRegion, [FromBody] ComunaUpdateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var comuna = await _comunaService.UpdateComunaAsync(idRegion, dto, cancellationToken);
        return Ok(comuna);
    }
}
