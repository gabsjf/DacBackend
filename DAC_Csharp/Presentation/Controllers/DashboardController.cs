using DAC_CSharp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAC_Csharp.Presentation.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly IDashboardQueryService _service;

    public DashboardController(IDashboardQueryService service)
    {
        _service = service;
    }

    [HttpGet("resumo")]
    public async Task<IActionResult> ObterResumoAsync([FromQuery] int? ano, [FromQuery] int? municipioId, [FromQuery] int? escolaId)
    {
        var resultado = await _service.ObterResumoAsync(ano, municipioId, escolaId);
        return Ok(resultado);
    }

    [HttpGet("cards")]
    public async Task<IActionResult> ObterCardsAsync([FromQuery] int? ano, [FromQuery] int? municipioId, [FromQuery] int? escolaId)
    {
        var resultado = await _service.ObterCardsAsync(ano, municipioId, escolaId);
        return Ok(resultado);
    }

    [HttpGet("grafico-evolucao")]
    public async Task<IActionResult> ObterGraficoEvolucaoAsync([FromQuery] int? municipioId, [FromQuery] int? escolaId, [FromQuery] string? indicador)
    {
        var resultado = await _service.ObterGraficoEvolucaoAsync(municipioId, escolaId, indicador);
        return Ok(resultado);
    }

    [HttpGet("grafico-ranking")]
    public async Task<IActionResult> ObterGraficoRankingAsync([FromQuery] int? ano, [FromQuery] string? indicador)
    {
        var resultado = await _service.ObterGraficoRankingAsync(ano, indicador);
        return Ok(resultado);
    }

    [HttpGet("grafico-distribuicao")]
    public async Task<IActionResult> ObterGraficoDistribuicaoAsync([FromQuery] int? ano, [FromQuery] int? municipioId, [FromQuery] int? escolaId, [FromQuery] string? indicador)
    {
        var resultado = await _service.ObterGraficoDistribuicaoAsync(ano, municipioId, escolaId, indicador);
        return Ok(resultado);
    }
}
