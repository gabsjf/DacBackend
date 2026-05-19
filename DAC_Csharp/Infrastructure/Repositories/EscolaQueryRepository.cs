using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace DAC_CSharp.Infrastructure.Repositories;

public sealed class EscolaQueryRepository : IEscolaQueryRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public EscolaQueryRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<EscolaDto>> ListarAsync(int? municipioId)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        var sql = "SELECT id, nome, municipio_id FROM escolas";
        if (municipioId.HasValue)
        {
            sql += " WHERE municipio_id = @municipioId";
        }
        sql += " ORDER BY nome";

        await using var cmd = new SqlCommand(sql, conn);
        if (municipioId.HasValue)
        {
            cmd.Parameters.AddWithValue("@municipioId", municipioId.Value);
        }

        await using var reader = await cmd.ExecuteReaderAsync();

        var results = new List<EscolaDto>();
        while (await reader.ReadAsync())
        {
            results.Add(new EscolaDto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                MunicipioId = reader.GetInt32(2)
            });
        }

        return results;
    }
}
