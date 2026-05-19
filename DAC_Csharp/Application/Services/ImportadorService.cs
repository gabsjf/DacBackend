using System.Diagnostics;
using DAC_CSharp.Application.Interfaces;

namespace DAC_CSharp.Application.Services;

public sealed class ImportadorService : IImportadorService
{
    private readonly IMunicipioRepository _municipioRepository;
    private readonly IEscolaRepository _escolaRepository;
    private readonly IDadosEducacionaisRepository _dadosEducacionaisRepository;
    private readonly ICsvReaderService _csvReaderService;

    public ImportadorService(
        IMunicipioRepository municipioRepository,
        IEscolaRepository escolaRepository,
        IDadosEducacionaisRepository dadosEducacionaisRepository,
        ICsvReaderService csvReaderService)
    {
        _municipioRepository = municipioRepository;
        _escolaRepository = escolaRepository;
        _dadosEducacionaisRepository = dadosEducacionaisRepository;
        _csvReaderService = csvReaderService;
    }

    public async Task ImportarAsync(string caminhoArquivo)
    {
        var stopwatch = Stopwatch.StartNew();
        var registrosIgnorados = 0;
        var registrosProcessados = 0;

        var linhas = _csvReaderService.LerCsv(caminhoArquivo, out var linhasIgnoradas);

        foreach (var linha in linhas)
        {
            var municipioId = await _municipioRepository.InserirOuBuscarAsync(linha.Municipio);
            var escolaId = await _escolaRepository.InserirOuBuscarAsync(linha.Escola, municipioId);

            await _dadosEducacionaisRepository.InserirAsync(
                linha.Ano,
                escolaId,
                linha.MatriculaInicial,
                linha.Aprovados,
                linha.Reprovados,
                linha.Abandono);

            registrosProcessados++;
        }

        registrosIgnorados += linhasIgnoradas;

        stopwatch.Stop();

        Console.WriteLine("Resumo da importação:");
        Console.WriteLine($"Arquivo importado: {caminhoArquivo}");
        Console.WriteLine($"Total processados: {registrosProcessados}");
        Console.WriteLine($"Total ignorados: {registrosIgnorados}");
        Console.WriteLine($"Tempo de execução: {stopwatch.Elapsed}");
    }
}
