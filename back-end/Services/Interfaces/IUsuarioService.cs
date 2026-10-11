using back_end.Models;

namespace back_end.Services.Interfaces;

public interface IUsuarioService
{
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario> CadastrarAsync(Usuario usuario);
}
