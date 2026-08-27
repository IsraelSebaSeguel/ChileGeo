namespace ChileGeo.Domain.Exceptions;

/// <summary>Thrown when business validation fails. Mapped to HTTP 400 by the API middleware.</summary>
public class BusinessValidationException : Exception
{
    public BusinessValidationException(string message) : base(message)
    {
    }
}
