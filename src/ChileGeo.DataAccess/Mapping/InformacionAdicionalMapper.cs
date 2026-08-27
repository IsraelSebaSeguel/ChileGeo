using System.Globalization;
using System.Xml.Linq;
using ChileGeo.Domain.Entities;

namespace ChileGeo.DataAccess.Mapping;

/// <summary>Adapter that converts between the domain InformacionAdicional entity and the XML format
/// stored in Comuna.InformacionAdicional: &lt;Info&gt;&lt;Superficie&gt;.../&lt;Poblacion Densidad="..."&gt;.../&lt;/Info&gt;.</summary>
public static class InformacionAdicionalMapper
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    public static string? ToXml(InformacionAdicional? info)
    {
        if (info is null)
        {
            return null;
        }

        var poblacionElement = new XElement("Poblacion", info.Poblacion?.ToString(Invariant) ?? string.Empty);
        if (info.Densidad.HasValue)
        {
            poblacionElement.Add(new XAttribute("Densidad", info.Densidad.Value.ToString(Invariant)));
        }

        var root = new XElement("Info",
            new XElement("Superficie", info.Superficie?.ToString(Invariant) ?? string.Empty),
            poblacionElement);

        return root.ToString(SaveOptions.DisableFormatting);
    }

    public static InformacionAdicional? FromXml(string? xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            return null;
        }

        var root = XElement.Parse(xml);
        var superficieText = root.Element("Superficie")?.Value;
        var poblacionElement = root.Element("Poblacion");
        var poblacionText = poblacionElement?.Value;
        var densidadText = poblacionElement?.Attribute("Densidad")?.Value;

        return new InformacionAdicional
        {
            Superficie = TryParseDecimal(superficieText),
            Poblacion = TryParseLong(poblacionText),
            Densidad = TryParseDecimal(densidadText)
        };
    }

    private static decimal? TryParseDecimal(string? value) =>
        decimal.TryParse(value, NumberStyles.Number, Invariant, out var result) ? result : null;

    private static long? TryParseLong(string? value) =>
        long.TryParse(value, NumberStyles.Number, Invariant, out var result) ? result : null;
}
