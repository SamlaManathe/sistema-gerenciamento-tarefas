using Microsoft.AspNetCore.Mvc;
using back_end.DTOs;
using back_end.Models;
using back_end.Responses;
using back_end.Services.Interfaces;

namespace back_end.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefaController : ControllerBase
{
    private readonly ITarefaService _tarefaService;

    public TarefaController(ITarefaService tarefaService)
    {
        _tarefaService = tarefaService;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> ListarPorUsuario(int usuarioId)
    {
        try
        {
            var tarefas = await _tarefaService.ListarPorUsuarioAsync(usuarioId);
            return Ok(new ApiResponse<IEnumerable<Tarefa>>(true, "Tarefas recuperadas com sucesso.", tarefas));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>(false, ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTarefaDTO dto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(new ApiResponse<object>(false, "Dados inválidos.", ModelState));

        var tarefa = new Tarefa
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataVencimento = dto.DataVencimento,
            UsuarioId = dto.UsuarioId
        };

        try
        {
            var tarefaCriada = await _tarefaService.CriarAsync(tarefa);
            return Created($"/api/Tarefa/{tarefaCriada.Id}", 
                new ApiResponse<Tarefa>(true, "Tarefa criada com sucesso.", tarefaCriada));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>(false, ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromQuery] int usuarioId, [FromBody] AtualizarTarefaDTO dto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(new ApiResponse<object>(false, "Dados inválidos.", ModelState));

        var dadosParaAtualizar = new Tarefa
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataVencimento = dto.DataVencimento ?? DateTime.MinValue,
            Concluida = dto.Concluida ?? false
        };

        try
        {
            var tarefaAtualizada = await _tarefaService.AtualizarAsync(id, usuarioId, dadosParaAtualizar);
            return Ok(new ApiResponse<Tarefa>(true, "Tarefa atualizada com sucesso.", tarefaAtualizada));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>(false, ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }

    [HttpPatch("{id}/concluir")]
    public async Task<IActionResult> MarcarComoConcluida(int id, [FromQuery] int usuarioId)
    {
        try
        {
            var tarefa = await _tarefaService.MarcarComoConcluidaAsync(id, usuarioId);
            return Ok(new ApiResponse<Tarefa>(true, "Tarefa marcada como concluída.", tarefa));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>(false, ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id, [FromQuery] int usuarioId)
    {
        try
        {
            var sucesso = await _tarefaService.ExcluirAsync(id, usuarioId);
            if (!sucesso) 
                return NotFound(new ApiResponse<object>(false, "Tarefa não encontrada para exclusão."));
            
            return Ok(new ApiResponse<object>(true, "Tarefa excluída com sucesso."));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }
}