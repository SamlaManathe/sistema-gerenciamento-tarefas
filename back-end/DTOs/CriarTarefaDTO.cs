using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs;

public class CriarTarefaDTO
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
    public DateTime DataVencimento { get; set; }

    [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
    public int UsuarioId { get; set; } // Adicionado para vincular ao usuário
}