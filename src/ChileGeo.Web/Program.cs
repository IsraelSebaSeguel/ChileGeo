using System.Globalization;
using ChileGeo.Web.Services;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Force a fixed culture so decimals always use '.' consistently between server rendering,
// jQuery client-side validation and form model binding, regardless of the host OS locale.
var defaultCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Typed HttpClient (Adapter) that talks to ChileGeo.Api. Base URL and optional API key come from configuration.
builder.Services.AddHttpClient<IGeoApiClient, GeoApiClient>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["GeoApi:BaseUrl"]
        ?? throw new InvalidOperationException("La configuración 'GeoApi:BaseUrl' no está definida.");
    client.BaseAddress = new Uri(baseUrl);

    var apiKey = configuration["GeoApi:ApiKey"];
    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
    }
});

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new[] { defaultCulture },
    SupportedUICultures = new[] { defaultCulture }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
