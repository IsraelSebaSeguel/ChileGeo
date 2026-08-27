using System.Data;

namespace ChileGeo.Domain.Interfaces;

/// <summary>Factory abstraction that creates ready-to-use database connections (Factory Method pattern).
/// Keeps repositories decoupled from the concrete ADO.NET provider and connection string source.</summary>
public interface IDbConnectionFactory
{
    IDbConnection CreateOpenConnection();
}
