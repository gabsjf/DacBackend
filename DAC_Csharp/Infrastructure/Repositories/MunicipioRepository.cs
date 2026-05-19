using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace DAC_CSharp.Infrastructure.Repositories;

public sealed class MunicipioRepository : IMunicipioRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public MunicipioRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> InserirOuBuscarAsync(string nome)
    {
        var nomeNormalizado = nome.Trim();

        using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();

        async Task<int?> BuscarAsync()
        {
            const string selectSql = @"
            SELECT TOP 1 id
            FROM municipios
            WHERE nome = @nome
            ORDER BY id;
        ";

            await using var selectCmd = new SqlCommand(selectSql, conn);
            selectCmd.Parameters.AddWithValue("@nome", nomeNormalizado);

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
            INSERT INTO municipios (nome)
            VALUES (@nome);
        ";

            await using var insertCmd = new SqlCommand(insertSql, conn);
            insertCmd.Parameters.AddWithValue("@nome", nomeNormalizado);

            await insertCmd.ExecuteNonQueryAsync();
        }
        catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
        {
            // Já existe. Vamos buscar novamente abaixo.
        }

        var idDepoisDoInsert = await BuscarAsync();

        if (idDepoisDoInsert.HasValue)
            return idDepoisDoInsert.Value;

        throw new Exception($"Não foi possível obter o ID do município: {nomeNormalizado}");
    }
}
