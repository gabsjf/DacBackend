namespace DAC_CSharp.Application.Dtos;

public sealed class EscolaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int MunicipioId { get; set; }
}
