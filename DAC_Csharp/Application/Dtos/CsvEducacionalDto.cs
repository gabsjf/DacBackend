namespace DAC_CSharp.Application.Dtos;

public sealed class CsvEducacionalDto
{
    public int Ano { get; set; }
    public string Municipio { get; set; } = string.Empty;
    public string Escola { get; set; } = string.Empty;
    public int MatriculaInicial { get; set; }
    public int Aprovados { get; set; }
    public int Reprovados { get; set; }
    public int Abandono { get; set; }
}
