using System.Collections.Generic;
using System.Threading.Tasks;
using back_end.Models;

namespace back_end.Repositories.Interfaces
{
    public interface ITarefaRepository
    {
        // Create
        Task<Tarefa> AdicionarAsync(Tarefa tarefa);

        //Read
        Task<IEnumerable<Tarefa>> ObterTodosIdUsuario(int idUsuario);
        Task<Tarefa?> ObterPorIdAsync(int id);

        //Update
        Task<Tarefa> AtualizarAsync(Tarefa tarefa);

        //Delete
        Task<bool> DeleteAsync(int id);
    }
}
