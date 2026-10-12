using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs;

public class AtualizarTarefaDTO
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataVencimento { get; set; }
    public bool? Concluida { get; set; } 
}