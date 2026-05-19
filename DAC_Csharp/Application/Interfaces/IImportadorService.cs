namespace DAC_CSharp.Application.Interfaces;

public interface IImportadorService
{
    Task ImportarAsync(string caminhoArquivo);
}
