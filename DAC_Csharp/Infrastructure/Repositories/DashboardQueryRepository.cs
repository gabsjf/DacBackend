using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace DAC_CSharp.Infrastructure.Repositories;

public sealed class DashboardQueryRepository : IDashboardQueryRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public DashboardQueryRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ResumoDashboardDto> ObterResumoAsync(int? ano, int? municipioId, int? escolaId)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        var sql = @"
            SELECT
                COUNT(DISTINCT m.id) AS total_municipios,
                COUNT(DISTINCT e.id) AS total_escolas,
                COALESCE(SUM(d.matricula_inicial), 0) AS total_matriculas,
                COALESCE(SUM(d.aprovados), 0) AS total_aprovados,
                COALESCE(SUM(d.reprovados), 0) AS total_reprovados,
                COALESCE(SUM(d.abandono), 0) AS total_abandono
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
        await reader.ReadAsync();

        return new ResumoDashboardDto
        {
            TotalMunicipios = reader.GetInt32(0),
            TotalEscolas = reader.GetInt32(1),
            TotalMatriculas = reader.GetInt32(2),
            TotalAprovados = reader.GetInt32(3),
            TotalReprovados = reader.GetInt32(4),
            TotalAbandono = reader.GetInt32(5)
        };
    }

    public async Task<IReadOnlyList<CardDashboardDto>> ObterCardsAsync(int? ano, int? municipioId, int? escolaId)
    {
        var resumo = await ObterResumoAsync(ano, municipioId, escolaId);

        return new List<CardDashboardDto>
        {
            new() { Titulo = "Matrículas", Valor = resumo.TotalMatriculas, Descricao = "Total de matrículas" },
            new() { Titulo = "Aprovados", Valor = resumo.TotalAprovados, Descricao = "Total de aprovados" },
            new() { Titulo = "Reprovados", Valor = resumo.TotalReprovados, Descricao = "Total de reprovados" },
            new() { Titulo = "Abandono", Valor = resumo.TotalAbandono, Descricao = "Total de abandono" }
        };
    }

    public async Task<IReadOnlyList<GraficoEvolucaoDto>> ObterGraficoEvolucaoAsync(int? municipioId, int? escolaId, string? indicador)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        var coluna = ResolveColunaIndicador(indicador);
        var sql = $@"
            SELECT d.ano, COALESCE(SUM(d.{coluna}), 0) AS valor
            FROM dados_educacionais d
            INNER JOIN escolas e ON e.id = d.escola_id
            INNER JOIN municipios m ON m.id = e.municipio_id
            WHERE 1 = 1";

        if (municipioId.HasValue)
        {
            sql += " AND m.id = @municipioId";
        }

        if (escolaId.HasValue)
        {
            sql += " AND e.id = @escolaId";
        }

        sql += " GROUP BY d.ano ORDER BY d.ano";

        await using var cmd = new SqlCommand(sql, conn);
        if (municipioId.HasValue)
        {
            cmd.Parameters.AddWithValue("@municipioId", municipioId.Value);
        }
        if (escolaId.HasValue)
        {
            cmd.Parameters.AddWithValue("@escolaId", escolaId.Value);
        }

        await using var reader = await cmd.ExecuteReaderAsync();

        var results = new List<GraficoEvolucaoDto>();
        while (await reader.ReadAsync())
        {
            results.Add(new GraficoEvolucaoDto
            {
                Ano = reader.GetInt32(0),
                Valor = reader.GetInt32(1)
            });
        }

        return results;
    }

    public async Task<IReadOnlyList<GraficoRankingDto>> ObterGraficoRankingAsync(int? ano, string? indicador)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        var coluna = ResolveColunaIndicador(indicador);
        var sql = $@"
            SELECT m.nome, COALESCE(SUM(d.{coluna}), 0) AS valor
            FROM dados_educacionais d
            INNER JOIN escolas e ON e.id = d.escola_id
            INNER JOIN municipios m ON m.id = e.municipio_id
            WHERE 1 = 1";

        if (ano.HasValue)
        {
            sql += " AND d.ano = @ano";
        }

        sql += " GROUP BY m.nome ORDER BY valor DESC";

        await using var cmd = new SqlCommand(sql, conn);
        if (ano.HasValue)
        {
            cmd.Parameters.AddWithValue("@ano", ano.Value);
        }

        await using var reader = await cmd.ExecuteReaderAsync();

        var results = new List<GraficoRankingDto>();
        while (await reader.ReadAsync())
        {
            results.Add(new GraficoRankingDto
            {
                Nome = reader.GetString(0),
                Valor = reader.GetInt32(1)
            });
        }

        return results;
    }

    public async Task<IReadOnlyList<GraficoDistribuicaoDto>> ObterGraficoDistribuicaoAsync(int? ano, int? municipioId, int? escolaId, string? indicador)
    {
        var dados = await ObterGraficoRankingAsync(ano, indicador);

        return dados.Select(item => new GraficoDistribuicaoDto
        {
            Categoria = item.Nome,
            Valor = item.Valor
        }).ToList();
    }

    private static string ResolveColunaIndicador(string? indicador)
    {
        return indicador?.ToLowerInvariant() switch
        {
            "aprovados" => "aprovados",
            "reprovados" => "reprovados",
            "abandono" => "abandono",
            _ => "matricula_inicial"
        };
    }
}
