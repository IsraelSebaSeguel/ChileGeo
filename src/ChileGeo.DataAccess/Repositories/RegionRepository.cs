using ChileGeo.Domain.Entities;
using ChileGeo.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace ChileGeo.DataAccess.Repositories;

/// <summary>Repository Pattern implementation for Region, backed 100% by stored procedures.</summary>
public class RegionRepository : IRegionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RegionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Region>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var regiones = new List<Region>();

        using var connection = (SqlConnection)_connectionFactory.CreateOpenConnection();
        using var command = new SqlCommand("dbo.usp_Region_GetAll", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            regiones.Add(MapRegion(reader));
        }

        return regiones;
    }

    public async Task<Region?> GetByIdAsync(int idRegion, CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)_connectionFactory.CreateOpenConnection();
        using var command = new SqlCommand("dbo.usp_Region_GetById", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@IdRegion", System.Data.SqlDbType.Int) { Value = idRegion });

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? MapRegion(reader) : null;
    }

    private static Region MapRegion(SqlDataReader reader) => new()
    {
        IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
        Nombre = reader.IsDBNull(reader.GetOrdinal("Region")) ? null : reader.GetString(reader.GetOrdinal("Region"))
    };
}
