namespace DAC_CSharp.Application.Interfaces;

public interface IDadosEducacionaisRepository
{
    Task InserirAsync(int ano, int escolaId, int matriculaInicial, int aprovados, int reprovados, int abandono);
}
