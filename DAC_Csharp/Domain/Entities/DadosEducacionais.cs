namespace DAC_CSharp.Domain.Entities;

public sealed class DadosEducacionais
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int EscolaId { get; set; }
    public int MatriculaInicial { get; set; }
    public int Aprovados { get; set; }
    public int Reprovados { get; set; }
    public int Abandono { get; set; }
}
