using DAC_CSharp.Application.Dtos;

namespace DAC_CSharp.Application.Interfaces;

public interface IEscolaQueryRepository
{
    Task<IReadOnlyList<EscolaDto>> ListarAsync(int? municipioId);
}
