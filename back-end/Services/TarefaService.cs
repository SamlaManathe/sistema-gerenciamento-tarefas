using back_end.Models;
using back_end.Repositories.Interfaces;
using back_end.Services.Interfaces;

namespace back_end.Services;

public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public TarefaService(ITarefaRepository tarefaRepository, IUsuarioRepository usuarioRepository)
    {
        _tarefaRepository = tarefaRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Tarefa> CriarAsync(Tarefa tarefa)
    {
        ArgumentNullException.ThrowIfNull(tarefa);
        ValidarTarefa(tarefa);
        await GarantirUsuarioExistenteAsync(tarefa.UsuarioId);
        tarefa.Titulo = tarefa.Titulo.Trim();
        tarefa.Descricao = tarefa.Descricao?.Trim() ?? string.Empty;
        return await _tarefaRepository.AdicionarAsync(tarefa);
    }

    public async Task<IEnumerable<Tarefa>> ListarPorUsuarioAsync(int usuarioId)
    {
        await GarantirUsuarioExistenteAsync(usuarioId);
        return await _tarefaRepository.ObterTodosIdUsuario(usuarioId);
    }

    public async Task<Tarefa?> ObterPorIdAsync(int tarefaId, int usuarioId)
    {
        ValidarId(tarefaId, nameof(tarefaId));
        ValidarId(usuarioId, nameof(usuarioId));
        var tarefa = await _tarefaRepository.ObterPorIdAsync(tarefaId);
        return tarefa?.UsuarioId == usuarioId ? tarefa : null;
    }

    public async Task<Tarefa> AtualizarAsync(int tarefaId, int usuarioId, Tarefa dados)
    {
        ArgumentNullException.ThrowIfNull(dados);
        var existente = await ObterPorIdAsync(tarefaId, usuarioId)
            ?? throw new KeyNotFoundException("Tarefa não encontrada para o usuário informado.");

        // O dono da tarefa é preservado: dados.UsuarioId não altera o vínculo.
        if (string.IsNullOrWhiteSpace(dados.Titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.", nameof(dados));
        if (dados.DataVencimento == default)
            throw new ArgumentException("A data de vencimento é obrigatória.", nameof(dados));

        existente.Titulo = dados.Titulo.Trim();
        existente.Descricao = dados.Descricao?.Trim() ?? string.Empty;
        existente.DataVencimento = dados.DataVencimento;
        existente.Concluida = dados.Concluida;
        return await _tarefaRepository.AtualizarAsync(existente);
    }

    public async Task<bool> ExcluirAsync(int tarefaId, int usuarioId)
    {
        var existente = await ObterPorIdAsync(tarefaId, usuarioId);
        return existente is not null && await _tarefaRepository.DeleteAsync(existente.Id);
    }

    public async Task<Tarefa> MarcarComoConcluidaAsync(int tarefaId, int usuarioId)
    {
        var tarefa = await ObterPorIdAsync(tarefaId, usuarioId)
            ?? throw new KeyNotFoundException("Tarefa não encontrada para o usuário informado.");
        if (tarefa.Concluida) return tarefa;
        tarefa.Concluida = true;
        return await _tarefaRepository.AtualizarAsync(tarefa);
    }

    private async Task GarantirUsuarioExistenteAsync(int usuarioId)
    {
        ValidarId(usuarioId, nameof(usuarioId));
        if (await _usuarioRepository.ObterPorIdAsync(usuarioId) is null)
            throw new KeyNotFoundException("Usuário não encontrado.");
    }

    private static void ValidarTarefa(Tarefa tarefa)
    {
        if (string.IsNullOrWhiteSpace(tarefa.Titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.", nameof(tarefa));
        if (tarefa.DataVencimento == default)
            throw new ArgumentException("A data de vencimento é obrigatória.", nameof(tarefa));
        ValidarId(tarefa.UsuarioId, nameof(tarefa.UsuarioId));
    }

    private static void ValidarId(int id, string nome)
    {
        if (id <= 0) throw new ArgumentException("O ID deve ser positivo.", nome);
    }
}
