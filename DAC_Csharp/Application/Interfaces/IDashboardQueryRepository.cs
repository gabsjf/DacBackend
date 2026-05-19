using DAC_CSharp.Application.Dtos;

namespace DAC_CSharp.Application.Interfaces;

public interface IDashboardQueryRepository
{
    Task<ResumoDashboardDto> ObterResumoAsync(int? ano, int? municipioId, int? escolaId);
    Task<IReadOnlyList<CardDashboardDto>> ObterCardsAsync(int? ano, int? municipioId, int? escolaId);
    Task<IReadOnlyList<GraficoEvolucaoDto>> ObterGraficoEvolucaoAsync(int? municipioId, int? escolaId, string? indicador);
    Task<IReadOnlyList<GraficoRankingDto>> ObterGraficoRankingAsync(int? ano, string? indicador);
    Task<IReadOnlyList<GraficoDistribuicaoDto>> ObterGraficoDistribuicaoAsync(int? ano, int? municipioId, int? escolaId, string? indicador);
}
