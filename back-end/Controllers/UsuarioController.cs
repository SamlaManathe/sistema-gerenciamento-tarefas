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
            return Created($"/api/Usuario/{usuarioCriado.Id}", 
                new ApiResponse<Usuario>(true, "Usuário cadastrado com sucesso.", usuarioCriado));
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
}