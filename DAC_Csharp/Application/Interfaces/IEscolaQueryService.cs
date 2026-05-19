using DAC_CSharp.Application.Dtos;

namespace DAC_CSharp.Application.Interfaces;

public interface IEscolaQueryService
{
    Task<IReadOnlyList<EscolaDto>> ListarAsync(int? municipioId);
}
