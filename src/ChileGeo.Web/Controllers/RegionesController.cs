using ChileGeo.Domain.Dtos;
using ChileGeo.Web.Services;
using ChileGeo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChileGeo.Web.Controllers;

/// <summary>MVC controller that renders regiones/comunas by consuming ChileGeo.Api through <see cref="IGeoApiClient"/>.
/// Contains no data-access or business logic of its own (SRP): it only orchestrates view rendering.</summary>
public class RegionesController : Controller
{
    private readonly IGeoApiClient _apiClient;
    private readonly ILogger<RegionesController> _logger;

    public RegionesController(IGeoApiClient apiClient, ILogger<RegionesController> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    // GET /Regiones — listado de regiones existentes en la BBDD.
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var regiones = await _apiClient.GetRegionesAsync(cancellationToken);
        return View(regiones);
    }

    // GET /Regiones/Detalle/5 — listado de comunas de la región seleccionada.
    public async Task<IActionResult> Detalle(int id, CancellationToken cancellationToken)
    {
        var region = await _apiClient.GetRegionAsync(id, cancellationToken);
        if (region is null)
        {
            return NotFound();
        }

        var comunas = await _apiClient.GetComunasAsync(id, cancellationToken);
        ViewBag.Region = region;
        return View(comunas);
    }

    // GET /Regiones/EditarComuna/{idRegion}/{idComuna} — formulario para modificar una comuna.
    [HttpGet]
    public async Task<IActionResult> EditarComuna(int idRegion, int idComuna, CancellationToken cancellationToken)
    {
        var comuna = await _apiClient.GetComunaAsync(idRegion, idComuna, cancellationToken);
        if (comuna is null)
        {
            return NotFound();
        }

        var model = new ComunaEditViewModel
        {
            IdRegion = idRegion,
            IdComuna = comuna.IdComuna,
            Nombre = comuna.Nombre ?? string.Empty,
            Superficie = comuna.InformacionAdicional?.Superficie,
            Poblacion = comuna.InformacionAdicional?.Poblacion,
            Densidad = comuna.InformacionAdicional?.Densidad
        };

        return View(model);
    }

    // POST /Regiones/EditarComuna — envía la actualización al servicio web (que la persiste vía MERGE).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarComuna(ComunaEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new ComunaUpdateDto
        {
            IdComuna = model.IdComuna,
            Nombre = model.Nombre,
            Superficie = model.Superficie,
            Poblacion = model.Poblacion,
            Densidad = model.Densidad
        };

        try
        {
            await _apiClient.GuardarComunaAsync(model.IdRegion, dto, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al guardar la comuna {IdComuna} de la región {IdRegion}.", model.IdComuna, model.IdRegion);
            ModelState.AddModelError(string.Empty, "No se pudo guardar la comuna. Intente nuevamente.");
            return View(model);
        }

        return RedirectToAction(nameof(Detalle), new { id = model.IdRegion });
    }
}
