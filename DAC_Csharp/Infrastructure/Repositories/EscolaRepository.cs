using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace DAC_CSharp.Infrastructure.Repositories;

public sealed class EscolaRepository : IEscolaRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public EscolaRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> InserirOuBuscarAsync(string nome, int municipioId)
    {
        var nomeNormalizado = nome.Trim();

        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        async Task<int?> BuscarAsync()
        {
            const string selectSql = @"
            SELECT TOP 1 id
            FROM escolas
            WHERE nome = @nome
              AND municipio_id = @municipioId
            ORDER BY id;
        ";

            await using var selectCmd = new SqlCommand(selectSql, conn);
            selectCmd.Parameters.AddWithValue("@nome", nomeNormalizado);
            selectCmd.Parameters.AddWithValue("@municipioId", municipioId);

            var result = await selectCmd.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
                return null;

            return Convert.ToInt32(result);
        }

        var idExistente = await BuscarAsync();

        if (idExistente.HasValue)
            return idExistente.Value;

        try
        {
            const string insertSql = @"
            INSERT INTO escolas (nome, municipio_id)
            VALUES (@nome, @municipioId);
        ";

            await using var insertCmd = new SqlCommand(insertSql, conn);
            insertCmd.Parameters.AddWithValue("@nome", nomeNormalizado);
            insertCmd.Parameters.AddWithValue("@municipioId", municipioId);

            await insertCmd.ExecuteNonQueryAsync();
        }
        catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
        {
            // Já existe. Vamos buscar novamente abaixo.
        }

        var idDepoisDoInsert = await BuscarAsync();

        if (idDepoisDoInsert.HasValue)
            return idDepoisDoInsert.Value;

        throw new Exception($"Não foi possível obter o ID da escola: {nomeNormalizado}");
    }
}
