using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;

namespace DAC_CSharp.Application.Services;

public sealed class EscolaQueryService : IEscolaQueryService
{
    private readonly IEscolaQueryRepository _repository;

    public EscolaQueryService(IEscolaQueryRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<EscolaDto>> ListarAsync(int? municipioId)
    {
        return _repository.ListarAsync(municipioId);
    }
}
