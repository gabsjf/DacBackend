using DAC_CSharp.Application.Dtos;

namespace DAC_CSharp.Application.Interfaces;

public interface ICsvReaderService
{
    IEnumerable<CsvEducacionalDto> LerCsv(string caminhoArquivo, out int linhasIgnoradas);
}
