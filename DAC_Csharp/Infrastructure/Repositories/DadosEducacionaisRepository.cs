using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace DAC_CSharp.Infrastructure.Repositories;

public sealed class DadosEducacionaisRepository : IDadosEducacionaisRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public DadosEducacionaisRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InserirAsync(int ano, int escolaId, int matriculaInicial, int aprovados, int reprovados, int abandono)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        const string insertSql = @"
            INSERT INTO dados_educacionais (ano, escola_id, matricula_inicial, aprovados, reprovados, abandono)
            VALUES (@ano, @escolaId, @matriculaInicial, @aprovados, @reprovados, @abandono)
        ";

        await using var cmd = new SqlCommand(insertSql, conn);
        cmd.Parameters.AddWithValue("@ano", ano);
        cmd.Parameters.AddWithValue("@escolaId", escolaId);
        cmd.Parameters.AddWithValue("@matriculaInicial", matriculaInicial);
        cmd.Parameters.AddWithValue("@aprovados", aprovados);
        cmd.Parameters.AddWithValue("@reprovados", reprovados);
        cmd.Parameters.AddWithValue("@abandono", abandono);

        await cmd.ExecuteNonQueryAsync();
    }
}
