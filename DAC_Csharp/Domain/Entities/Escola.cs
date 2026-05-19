namespace DAC_CSharp.Domain.Entities;

public sealed class Escola
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int MunicipioId { get; set; }
}
