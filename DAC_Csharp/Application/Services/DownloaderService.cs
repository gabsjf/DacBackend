using DAC_CSharp.Application.Interfaces;

namespace DAC_CSharp.Application.Services;

public sealed class DownloaderService : IDownloaderService
{
    private readonly HttpClient _httpClient;

    public DownloaderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> BaixarArquivoAsync(string url, string nomeArquivo)
    {
        Console.WriteLine($"Baixando: {url}");

        var response = await _httpClient.GetAsync(url);

        Console.WriteLine($"Status: {(int)response.StatusCode} - {response.StatusCode}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Erro ao baixar {nomeArquivo}. URL: {url}. Status: {(int)response.StatusCode} - {response.StatusCode}");
        }

        var bytes = await response.Content.ReadAsByteArrayAsync();

        await File.WriteAllBytesAsync(nomeArquivo, bytes);

        Console.WriteLine($"Arquivo salvo: {Path.GetFullPath(nomeArquivo)}");

        return nomeArquivo;
    }
}