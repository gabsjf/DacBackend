namespace DAC_CSharp.Application.Interfaces;

public interface IEscolaRepository
{
    Task<int> InserirOuBuscarAsync(string nome, int municipioId);
}
