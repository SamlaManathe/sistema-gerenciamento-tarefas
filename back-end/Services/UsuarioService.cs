using back_end.Models;
using back_end.Repositories.Interfaces;
using back_end.Services.Interfaces;

namespace back_end.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("O ID do usuário deve ser positivo.", nameof(id));
        return await _usuarioRepository.ObterPorIdAsync(id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail é obrigatório.", nameof(email));

        return await _usuarioRepository.ObterPorEmailAsync(email.Trim());
    }

    public async Task<Usuario> CadastrarAsync(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        if (string.IsNullOrWhiteSpace(usuario.Nome))
            throw new ArgumentException("O nome é obrigatório.", nameof(usuario));
        if (string.IsNullOrWhiteSpace(usuario.Email) ||
            !System.Net.Mail.MailAddress.TryCreate(usuario.Email.Trim(), out var endereco) ||
            endereco.Address != usuario.Email.Trim())
            throw new ArgumentException("Informe um e-mail válido.", nameof(usuario));
        if (string.IsNullOrWhiteSpace(usuario.Senha))
            throw new ArgumentException("A senha é obrigatória.", nameof(usuario));

        usuario.Nome = usuario.Nome.Trim();
        usuario.Email = usuario.Email.Trim();

        if (await _usuarioRepository.ObterPorEmailAsync(usuario.Email) is not null)
            throw new InvalidOperationException("Já existe um usuário cadastrado com este e-mail.");

        // Pendente: definir com o squad um mecanismo de hash de senha.
        // Não chamar AdicionarAsync enquanto a senha estiver em texto puro.
        throw new InvalidOperationException(
            "Cadastro indisponível até a definição do armazenamento seguro de senhas pelo squad.");
    }
}
