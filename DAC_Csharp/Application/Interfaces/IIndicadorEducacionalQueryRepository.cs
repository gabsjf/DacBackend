using DAC_CSharp.Application.Dtos;

namespace DAC_CSharp.Application.Interfaces;

public interface IIndicadorEducacionalQueryRepository
{
    Task<IReadOnlyList<IndicadorEducacionalDto>> ListarAsync(int? ano, int? municipioId, int? escolaId);
}
