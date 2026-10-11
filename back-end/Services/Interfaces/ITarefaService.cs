using back_end.Models;

namespace back_end.Services.Interfaces;

public interface ITarefaService
{
    Task<Tarefa> CriarAsync(Tarefa tarefa);
    Task<IEnumerable<Tarefa>> ListarPorUsuarioAsync(int usuarioId);
    Task<Tarefa?> ObterPorIdAsync(int tarefaId, int usuarioId);
    Task<Tarefa> AtualizarAsync(int tarefaId, int usuarioId, Tarefa dados);
    Task<bool> ExcluirAsync(int tarefaId, int usuarioId);
    Task<Tarefa> MarcarComoConcluidaAsync(int tarefaId, int usuarioId);
}
