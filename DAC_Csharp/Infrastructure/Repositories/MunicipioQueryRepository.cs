using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace DAC_CSharp.Infrastructure.Repositories;

public sealed class MunicipioQueryRepository : IMunicipioQueryRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public MunicipioQueryRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<MunicipioDto>> ListarAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        const string sql = "SELECT id, nome FROM municipios ORDER BY nome";

        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        var results = new List<MunicipioDto>();
        while (await reader.ReadAsync())
        {
            results.Add(new MunicipioDto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1)
            });
        }

        return results;
    }
}
