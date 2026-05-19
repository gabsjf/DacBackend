namespace DAC_CSharp.Application.Interfaces;

public interface IMunicipioRepository
{
    Task<int> InserirOuBuscarAsync(string nome);
}
