using DAC_CSharp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAC_Csharp.Presentation.Controllers;

[ApiController]
[Route("api/importacao")]
public sealed class ImportacaoController : ControllerBase
{
    private readonly IImportadorService _importadorService;
    private readonly IDownloaderService _downloaderService;

    public ImportacaoController(
        IImportadorService importadorService,
        IDownloaderService downloaderService)
    {
        _importadorService = importadorService;
        _downloaderService = downloaderService;
    }

    [HttpPost("importar")]
    public async Task<IActionResult> ImportarAsync()
    {
        var erros = new List<object>();

        string[,] arquivos =
        {
            { "2018", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2018" },
            { "2019", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2019" },
            { "2020", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2020" },
            { "2021", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2021" },
            { "2022", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2022" },
            { "2023", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2023" },
            { "2024", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2024" },
            { "2025", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2025" },
            { "2026", "https://www.dados.ms.gov.br/datastore/dump/matriculas-por-unidade-escolar-2026" }
        };

        for (int i = 0; i < arquivos.GetLength(0); i++)
        {
            var ano = arquivos[i, 0];
            var link = arquivos[i, 1];

            try
            {
                var nomeArquivo = $"{ano}.csv";

                Console.WriteLine("========================================");
                Console.WriteLine($"Iniciando download do arquivo {nomeArquivo}");
                Console.WriteLine($"Link: {link}");

                await _downloaderService.BaixarArquivoAsync(link, nomeArquivo);

                var caminhoCompleto = Path.GetFullPath(nomeArquivo);

                Console.WriteLine($"Arquivo baixado: {nomeArquivo}");
                Console.WriteLine($"Arquivo existe? {System.IO.File.Exists(caminhoCompleto)}");
                Console.WriteLine($"Caminho completo: {caminhoCompleto}");

                Console.WriteLine($"Iniciando importação do arquivo {nomeArquivo}");

                await _importadorService.ImportarAsync(caminhoCompleto);

                Console.WriteLine($"Importação finalizada: {nomeArquivo}");
                Console.WriteLine("========================================");
            }
            catch (Exception ex)
            {
                var nomeArquivo = $"{ano}.csv";
                var caminhoCompleto = Path.GetFullPath(nomeArquivo);

                if (System.IO.File.Exists(caminhoCompleto))
                {
                    Console.WriteLine($"Download falhou para {ano}, mas arquivo local foi encontrado. Importando localmente...");

                    await _importadorService.ImportarAsync(caminhoCompleto);

                    continue;
                }

                Console.WriteLine($"Erro ao importar o ano {ano}:");
                Console.WriteLine(ex.Message);

                erros.Add(new
                {
                    ano,
                    erro = ex.Message
                });

                continue;
            }
        }

        return Ok(new
        {
            sucesso = true,
            mensagem = "Importação concluída.",
            erros
        });
    }
}