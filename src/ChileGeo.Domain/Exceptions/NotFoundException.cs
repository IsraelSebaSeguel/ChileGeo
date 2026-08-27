namespace ChileGeo.Domain.Exceptions;

/// <summary>Thrown when a requested Region or Comuna does not exist. Mapped to HTTP 404 by the API middleware.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
