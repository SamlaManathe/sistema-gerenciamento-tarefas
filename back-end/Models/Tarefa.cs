namespace back_end.Models;

public class Tarefa
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataVencimento { get; set; }

    public bool Concluida { get; set; }

    public int UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;
}