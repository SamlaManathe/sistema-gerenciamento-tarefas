using back_end.Models;
using back_end.Repositories.Interfaces;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace back_end.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.");

        return await _usuarioRepository.ObterPorIdAsync(id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail é obrigatório.");

        return await _usuarioRepository.ObterPorEmailAsync(email.Trim());
    }

    public async Task<Usuario?> AutenticarAsync(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            return null;

        var usuario = await _usuarioRepository.ObterPorEmailAsync(email.Trim());

        if (usuario is null)
            return null;

        var resultado = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.Senha,
            senha
        );

        return resultado == PasswordVerificationResult.Failed
            ? null
            : usuario;
    }

    public async Task<Usuario> CadastrarAsync(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        if (string.IsNullOrWhiteSpace(usuario.Nome))
            throw new ArgumentException("O nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(usuario.Email) ||
            !System.Net.Mail.MailAddress.TryCreate(
                usuario.Email.Trim(), out var endereco) ||
            endereco.Address != usuario.Email.Trim())
        {
            throw new ArgumentException("Informe um e-mail válido.");
        }

        if (string.IsNullOrWhiteSpace(usuario.Senha) || usuario.Senha.Length < 6)
            throw new ArgumentException("A senha deve ter pelo menos 6 caracteres.");

        usuario.Nome = usuario.Nome.Trim();
        usuario.Email = usuario.Email.Trim();

        if (await _usuarioRepository.ObterPorEmailAsync(usuario.Email) is not null)
            throw new InvalidOperationException("Já existe um usuário cadastrado com este e-mail.");

        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);

        return await _usuarioRepository.AdicionarAsync(usuario);
    }
}
