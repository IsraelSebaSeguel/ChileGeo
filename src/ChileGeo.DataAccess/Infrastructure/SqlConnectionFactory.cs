using System.Data;
using ChileGeo.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ChileGeo.DataAccess.Infrastructure;

/// <summary>Concrete Factory that creates open SQL Server connections from configuration.</summary>
public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ChileGeoDb")
            ?? throw new InvalidOperationException("La cadena de conexión 'ChileGeoDb' no está configurada.");
    }

    public IDbConnection CreateOpenConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
