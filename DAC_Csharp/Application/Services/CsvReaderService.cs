using System.Globalization;
using System.Text;
using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;

namespace DAC_CSharp.Application.Services;

public sealed class CsvReaderService : ICsvReaderService
{
    public IEnumerable<CsvEducacionalDto> LerCsv(string caminhoArquivo, out int linhasIgnoradas)
    {
        var ignoradas = 0;
        var dados = new List<CsvEducacionalDto>();

        using var reader = new StreamReader(
    caminhoArquivo,
    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
    detectEncodingFromByteOrderMarks: true
);

        reader.ReadLine(); // cabeçalho

        string? linha;

        while ((linha = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(linha))
            {
                ignoradas++;
                continue;
            }

            var colunas = linha.Contains(';')
                ? linha.Split(';')
                : linha.Split(',');

            if (colunas.Length < 13)
            {
                ignoradas++;
                continue;
            }

            var municipio = colunas[2].Trim();
            var escola = colunas[3].Trim();

            if (string.IsNullOrWhiteSpace(municipio) || string.IsNullOrWhiteSpace(escola))
            {
                ignoradas++;
                continue;
            }

            if (!TryParseInt(colunas[1], out var ano) ||
                !TryParseInt(colunas[5], out var matriculaInicial) ||
                !TryParseInt(colunas[10], out var abandono) ||
                !TryParseInt(colunas[11], out var aprovados) ||
                !TryParseInt(colunas[12], out var reprovados))
            {
                ignoradas++;
                continue;
            }

            dados.Add(new CsvEducacionalDto
            {
                Ano = ano,
                Municipio = municipio,
                Escola = escola,
                MatriculaInicial = matriculaInicial,
                Abandono = abandono,
                Aprovados = aprovados,
                Reprovados = reprovados
            });
        }

        linhasIgnoradas = ignoradas;
        return dados;
    }

    private static bool TryParseInt(string valor, out int resultado)
    {
        valor = valor
            .Trim()
            .Replace(".", "")
            .Replace(",", "");

        return int.TryParse(
            valor,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out resultado
        );
    }
}