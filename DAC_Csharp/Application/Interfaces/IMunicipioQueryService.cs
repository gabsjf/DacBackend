using DAC_CSharp.Application.Dtos;

namespace DAC_CSharp.Application.Interfaces;

public interface IMunicipioQueryService
{
    Task<IReadOnlyList<MunicipioDto>> ListarAsync();
}
