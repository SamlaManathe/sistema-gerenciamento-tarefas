using Microsoft.AspNetCore.Mvc;
using back_end.DTOs;
using back_end.Models;
using back_end.Responses;
using back_end.Services.Interfaces;

namespace back_end.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CriarUsuarioDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Dados inválidos.", ModelState));

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha
        };

        try
        {
            var usuarioCriado = await _usuarioService.CadastrarAsync(usuario);
            return Created(
                $"/api/Usuario/{usuarioCriado.Id}",
                new ApiResponse<object>(
                    true,
                    "Usuário cadastrado com sucesso.",
                    new
                    {
                        usuarioCriado.Id,
                        usuarioCriado.Nome,
                        usuarioCriado.Email
                    }));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, "Dados inválidos.", ModelState));

        var usuario = await _usuarioService.AutenticarAsync(dto.Email, dto.Senha);

        if (usuario is null)
            return Unauthorized(new ApiResponse<object>(false, "E-mail ou senha inválidos."));

        return Ok(new ApiResponse<object>(
            true,
            "Login realizado com sucesso.",
            new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email
            }));
    }
}
