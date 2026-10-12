import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TarefaService } from '../../services/tarefa.service';
import { Tarefa } from '../../models/tarefa';

@Component({
  selector: 'app-lista-tarefas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './lista-tarefas.html',
  styleUrl: './lista-tarefas.css'
})
export class ListaTarefas implements OnInit {
  tarefas: Tarefa[] = [];
  usuarioId = 1;

  mensagem = '';
  erro = '';
  carregando = false;
  editandoId: number | null = null;

  formulario: Tarefa = this.novaTarefa();

  constructor(private tarefaService: TarefaService) {}

  ngOnInit(): void {
    this.carregarTarefas();
  }

  novaTarefa(): Tarefa {
    return {
      titulo: '',
      descricao: '',
      dataVencimento: '',
      concluida: false,
      usuarioId: 1
    };
  }

  carregarTarefas(): void {
    this.carregando = true;
    this.erro = '';

    this.tarefaService.listarPorUsuario(this.usuarioId).subscribe({
      next: (resposta) => {
        this.tarefas = resposta.data ?? resposta;
        this.carregando = false;
      },
      error: (erro) => {
        this.erro =
          erro.error?.message ??
          'Não foi possível carregar as tarefas. Verifique se a API está funcionando e se a usuária existe.';
        this.carregando = false;
      }
    });
  }

  salvar(): void {
    this.mensagem = '';
    this.erro = '';

    if (!this.formulario.titulo.trim()) {
      this.erro = 'Informe o título da tarefa.';
      return;
    }

    if (!this.formulario.dataVencimento) {
      this.erro = 'Informe a data de vencimento.';
      return;
    }

    const tarefa: Tarefa = {
      ...this.formulario,
      titulo: this.formulario.titulo.trim(),
      usuarioId: this.usuarioId,
      dataVencimento: this.formulario.dataVencimento
    };

    const requisicao = this.editandoId !== null
      ? this.tarefaService.atualizar(
          this.editandoId,
          this.usuarioId,
          tarefa
        )
      : this.tarefaService.criar(tarefa);

    requisicao.subscribe({
      next: (resposta) => {
        this.mensagem =
          resposta.message ??
          (this.editandoId !== null
            ? 'Tarefa atualizada com sucesso!'
            : 'Tarefa criada com sucesso!');

        this.cancelarEdicao();
        this.carregarTarefas();
      },
      error: (erro) => {
        this.erro =
          erro.error?.message ??
          'Não foi possível salvar a tarefa.';
      }
    });
  }

  editar(tarefa: Tarefa): void {
    this.editandoId = tarefa.id!;

    this.formulario = {
      ...tarefa,
      dataVencimento: tarefa.dataVencimento
        ? tarefa.dataVencimento.substring(0, 16)
        : ''
    };

    this.mensagem = '';
    this.erro = '';
  }

  cancelarEdicao(): void {
    this.editandoId = null;
    this.formulario = this.novaTarefa();
  }

  marcarConcluida(tarefa: Tarefa): void {
    if (!tarefa.id) return;

    this.erro = '';
    this.mensagem = '';

    this.tarefaService.concluir(tarefa.id, this.usuarioId).subscribe({
      next: (resposta) => {
        this.mensagem = resposta.message ?? 'Tarefa concluída!';
        this.carregarTarefas();
      },
      error: (erro) => {
        this.erro =
          erro.error?.message ??
          'Não foi possível concluir a tarefa.';
      }
    });
  }

  excluir(tarefa: Tarefa): void {
    if (!tarefa.id) return;

    if (!confirm(`Deseja excluir a tarefa "${tarefa.titulo}"?`)) {
      return;
    }

    this.erro = '';
    this.mensagem = '';

    this.tarefaService.excluir(tarefa.id, this.usuarioId).subscribe({
      next: (resposta) => {
        this.mensagem = resposta.message ?? 'Tarefa excluída!';
        this.carregarTarefas();
      },
      error: (erro) => {
        this.erro =
          erro.error?.message ??
          'Não foi possível excluir a tarefa.';
      }
    });
  }
}
