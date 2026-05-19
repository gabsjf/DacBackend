using DAC_CSharp.Application.Dtos;

namespace DAC_CSharp.Application.Interfaces;

public interface IMunicipioQueryRepository
{
    Task<IReadOnlyList<MunicipioDto>> ListarAsync();
}
