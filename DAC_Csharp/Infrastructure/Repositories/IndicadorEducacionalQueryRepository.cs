using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace DAC_CSharp.Infrastructure.Repositories;

public sealed class IndicadorEducacionalQueryRepository : IIndicadorEducacionalQueryRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public IndicadorEducacionalQueryRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<IndicadorEducacionalDto>> ListarAsync(int? ano, int? municipioId, int? escolaId)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        var sql = @"
            SELECT d.ano, m.id, m.nome, e.id, e.nome, d.matricula_inicial, d.aprovados, d.reprovados, d.abandono
            FROM dados_educacionais d
            INNER JOIN escolas e ON e.id = d.escola_id
            INNER JOIN municipios m ON m.id = e.municipio_id
            WHERE 1 = 1";

        if (ano.HasValue)
        {
            sql += " AND d.ano = @ano";
        }

        if (municipioId.HasValue)
        {
            sql += " AND m.id = @municipioId";
        }

        if (escolaId.HasValue)
        {
            sql += " AND e.id = @escolaId";
        }

        sql += " ORDER BY d.ano DESC, m.nome, e.nome";

        await using var cmd = new SqlCommand(sql, conn);
        if (ano.HasValue)
        {
            cmd.Parameters.AddWithValue("@ano", ano.Value);
        }
        if (municipioId.HasValue)
        {
            cmd.Parameters.AddWithValue("@municipioId", municipioId.Value);
        }
        if (escolaId.HasValue)
        {
            cmd.Parameters.AddWithValue("@escolaId", escolaId.Value);
        }

        await using var reader = await cmd.ExecuteReaderAsync();

        var results = new List<IndicadorEducacionalDto>();
        while (await reader.ReadAsync())
        {
            results.Add(new IndicadorEducacionalDto
            {
                Ano = reader.GetInt32(0),
                MunicipioId = reader.GetInt32(1),
                Municipio = reader.GetString(2),
                EscolaId = reader.GetInt32(3),
                Escola = reader.GetString(4),
                MatriculaInicial = reader.GetInt32(5),
                Aprovados = reader.GetInt32(6),
                Reprovados = reader.GetInt32(7),
                Abandono = reader.GetInt32(8)
            });
        }

        return results;
    }
}
