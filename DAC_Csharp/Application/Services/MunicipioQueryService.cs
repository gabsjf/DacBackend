using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;

namespace DAC_CSharp.Application.Services;

public sealed class MunicipioQueryService : IMunicipioQueryService
{
    private readonly IMunicipioQueryRepository _repository;

    public MunicipioQueryService(IMunicipioQueryRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<MunicipioDto>> ListarAsync()
    {
        return _repository.ListarAsync();
    }
}
