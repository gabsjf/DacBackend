using DAC_CSharp.Application.Dtos;
using DAC_CSharp.Application.Interfaces;

namespace DAC_CSharp.Application.Services;

public sealed class DashboardQueryService : IDashboardQueryService
{
    private readonly IDashboardQueryRepository _repository;

    public DashboardQueryService(IDashboardQueryRepository repository)
    {
        _repository = repository;
    }

    public Task<ResumoDashboardDto> ObterResumoAsync(int? ano, int? municipioId, int? escolaId)
    {
        return _repository.ObterResumoAsync(ano, municipioId, escolaId);
    }

    public Task<IReadOnlyList<CardDashboardDto>> ObterCardsAsync(int? ano, int? municipioId, int? escolaId)
    {
        return _repository.ObterCardsAsync(ano, municipioId, escolaId);
    }

    public Task<IReadOnlyList<GraficoEvolucaoDto>> ObterGraficoEvolucaoAsync(int? municipioId, int? escolaId, string? indicador)
    {
        return _repository.ObterGraficoEvolucaoAsync(municipioId, escolaId, indicador);
    }

    public Task<IReadOnlyList<GraficoRankingDto>> ObterGraficoRankingAsync(int? ano, string? indicador)
    {
        return _repository.ObterGraficoRankingAsync(ano, indicador);
    }

    public Task<IReadOnlyList<GraficoDistribuicaoDto>> ObterGraficoDistribuicaoAsync(int? ano, int? municipioId, int? escolaId, string? indicador)
    {
        return _repository.ObterGraficoDistribuicaoAsync(ano, municipioId, escolaId, indicador);
    }
}
