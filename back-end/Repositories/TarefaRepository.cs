using back_end.Data;
using back_end.Models;
using back_end.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Repositories;

public class TarefaRepository : ITarefaRepository
{
    // Variável privada que vai guardar a conexão com o banco de dados.
    private readonly ApplicationDbContext _context;

    public TarefaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //Create: Adiciona uma nova tarefa
    public async Task<Tarefa> AdicionarAsync(Tarefa tarefa)
    {
        await _context.Tarefas.AddAsync(tarefa);
        await _context.SaveChangesAsync();
        return tarefa;
    }

    //READ:Busca todas as tarefas por Usuarios
    public async Task<IEnumerable<Tarefa>> ObterTodosIdUsuario(int idUsuario)
    {
        // Where filtra as tarefas e ToListAsync executa a consulta no banco
            return await _context.Tarefas
                .Where(t => t.UsuarioId == idUsuario)
                .ToListAsync();
    }

    //Read: Busca uma tarefa por ID
    public async Task<Tarefa?> ObterPorIdAsync(int id)
    {
        return await _context.Tarefas.FindAsync(id);
    }

    //Update: Atualiza os dados de uma Tarefa existente
    public async Task<Tarefa> AtualizarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
        return tarefa;
    }

    //Delete: Exclui uma tarefa 
    public async Task<bool> DeleteAsync(int id)
    {
        //Aqui ele procura a tarefa usando o id
        var tarefa = await _context.Tarefas.FindAsync(id);

        //verifica primeiro se a tarefa existe
        if(tarefa == null)
        {
            return false;
        }else
        {
             _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return true;
        }
     
    }
 
}