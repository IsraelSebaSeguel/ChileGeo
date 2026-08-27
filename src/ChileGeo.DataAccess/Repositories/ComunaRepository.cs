using ChileGeo.DataAccess.Mapping;
using ChileGeo.Domain.Entities;
using ChileGeo.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace ChileGeo.DataAccess.Repositories;

/// <summary>Repository Pattern implementation for Comuna, backed 100% by stored procedures.
/// Updates use the MERGE statement (dbo.usp_Comuna_Merge) as required by the exercise.</summary>
public class ComunaRepository : IComunaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ComunaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Comuna>> GetByRegionAsync(int idRegion, CancellationToken cancellationToken = default)
    {
        var comunas = new List<Comuna>();

        using var connection = (SqlConnection)_connectionFactory.CreateOpenConnection();
        using var command = new SqlCommand("dbo.usp_Comuna_GetByRegion", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@IdRegion", System.Data.SqlDbType.Int) { Value = idRegion });

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            comunas.Add(MapComuna(reader));
        }

        return comunas;
    }

    public async Task<Comuna?> GetByIdAsync(int idComuna, CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)_connectionFactory.CreateOpenConnection();
        using var command = new SqlCommand("dbo.usp_Comuna_GetById", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@IdComuna", System.Data.SqlDbType.Int) { Value = idComuna });

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? MapComuna(reader) : null;
    }

    public async Task<Comuna> MergeAsync(Comuna comuna, CancellationToken cancellationToken = default)
    {
        using var connection = (SqlConnection)_connectionFactory.CreateOpenConnection();
        using var command = new SqlCommand("dbo.usp_Comuna_Merge", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@IdComuna", System.Data.SqlDbType.Int) { Value = comuna.IdComuna });
        command.Parameters.Add(new SqlParameter("@IdRegion", System.Data.SqlDbType.Int) { Value = comuna.IdRegion });
        command.Parameters.Add(new SqlParameter("@Comuna", System.Data.SqlDbType.NVarChar, 128) { Value = comuna.Nombre ?? (object)DBNull.Value });
        command.Parameters.Add(new SqlParameter("@InformacionAdicional", System.Data.SqlDbType.Xml)
        {
            Value = (object?)InformacionAdicionalMapper.ToXml(comuna.InformacionAdicional) ?? DBNull.Value
        });

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException("El procedimiento almacenado usp_Comuna_Merge no devolvió resultados.");
        }

        return MapComuna(reader);
    }

    private static Comuna MapComuna(SqlDataReader reader)
    {
        var idRegionOrdinal = reader.GetOrdinal("IdRegion");
        var infoOrdinal = reader.GetOrdinal("InformacionAdicional");

        return new Comuna
        {
            IdComuna = reader.GetInt32(reader.GetOrdinal("IdComuna")),
            IdRegion = reader.IsDBNull(idRegionOrdinal) ? 0 : reader.GetInt32(idRegionOrdinal),
            Nombre = reader.IsDBNull(reader.GetOrdinal("Comuna")) ? null : reader.GetString(reader.GetOrdinal("Comuna")),
            InformacionAdicional = reader.IsDBNull(infoOrdinal)
                ? null
                : InformacionAdicionalMapper.FromXml(reader.GetString(infoOrdinal))
        };
    }
}
