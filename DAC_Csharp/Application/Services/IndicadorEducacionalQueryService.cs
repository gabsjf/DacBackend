using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;

namespace DAC_CSharp.Application.Services;

public sealed class IndicadorEducacionalQueryService : IIndicadorEducacionalQueryService
{
    private readonly IIndicadorEducacionalQueryRepository _repository;

    public IndicadorEducacionalQueryService(IIndicadorEducacionalQueryRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<IndicadorEducacionalDto>> ListarAsync(int? ano, int? municipioId, int? escolaId)
    {
        return _repository.ListarAsync(ano, municipioId, escolaId);
    }
}
