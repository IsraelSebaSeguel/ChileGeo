using System.Net;
using System.Net.Http.Json;
using ChileGeo.Domain.Dtos;

namespace ChileGeo.Web.Services;

/// <summary>HttpClient-based implementation of <see cref="IGeoApiClient"/>. Registered as a typed client
/// pointing to the ChileGeo.Api base address (see appsettings "GeoApi:BaseUrl").</summary>
public class GeoApiClient : IGeoApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeoApiClient> _logger;

    public GeoApiClient(HttpClient httpClient, ILogger<GeoApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<RegionDto>> GetRegionesAsync(CancellationToken cancellationToken = default)
    {
        var regiones = await _httpClient.GetFromJsonAsync<List<RegionDto>>("region", cancellationToken);
        return regiones ?? new List<RegionDto>();
    }

    public async Task<RegionDto?> GetRegionAsync(int idRegion, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"region/{idRegion}", cancellationToken);
        return await ReadOrDefaultAsync<RegionDto>(response, cancellationToken);
    }

    public async Task<IReadOnlyList<ComunaDto>> GetComunasAsync(int idRegion, CancellationToken cancellationToken = default)
    {
        var comunas = await _httpClient.GetFromJsonAsync<List<ComunaDto>>($"region/{idRegion}/comuna", cancellationToken);
        return comunas ?? new List<ComunaDto>();
    }

    public async Task<ComunaDto?> GetComunaAsync(int idRegion, int idComuna, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"region/{idRegion}/comuna/{idComuna}", cancellationToken);
        return await ReadOrDefaultAsync<ComunaDto>(response, cancellationToken);
    }

    public async Task<ComunaDto?> GuardarComunaAsync(int idRegion, ComunaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"region/{idRegion}/comuna", dto, cancellationToken);
        return await ReadOrDefaultAsync<ComunaDto>(response, cancellationToken);
    }

    private async Task<T?> ReadOrDefaultAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("Llamada a la API falló con {StatusCode}: {Body}", response.StatusCode, body);
            throw new HttpRequestException($"La API respondió {(int)response.StatusCode}: {body}");
        }

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
    }
}
