namespace DAC_CSharp.Application.Interfaces;

public interface IDownloaderService
{
    Task<string> BaixarArquivoAsync(string url, string nomeArquivo);
}
