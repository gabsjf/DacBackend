namespace DAC_CSharp.Application.Dtos;

public sealed class ResumoDashboardDto
{
    public int TotalMunicipios { get; set; }
    public int TotalEscolas { get; set; }
    public int TotalMatriculas { get; set; }
    public int TotalAprovados { get; set; }
    public int TotalReprovados { get; set; }
    public int TotalAbandono { get; set; }
}
