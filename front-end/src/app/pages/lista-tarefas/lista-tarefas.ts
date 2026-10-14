import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { TarefaService } from '../../services/tarefa.service';
import { Tarefa } from '../../models/tarefa';
import { UsuarioLogado } from '../../models/usuario';
import { ApiResponse } from '../../models/api-response';

@Component({
  selector: 'app-lista-tarefas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './lista-tarefas.html',
  styleUrl: './lista-tarefas.css'
})
export class ListaTarefas implements OnInit {
  tarefas: Tarefa[] = [];
  usuarioId = 0;
  nomeUsuario = '';
  mensagem = '';
  erro = '';
  carregando = false;
  editandoId: number | null = null;
  formulario: Tarefa = this.novaTarefa();

  constructor(
    private tarefaService: TarefaService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const usuarioSalvo = localStorage.getItem('usuarioLogado');
    if (!usuarioSalvo) {
      this.router.navigate(['/login']);
      return;
    }

    try {
      const usuario = JSON.parse(usuarioSalvo) as UsuarioLogado;
      if (!Number.isInteger(usuario.id) || usuario.id <= 0) {
        throw new Error('ID de usuária inválido.');
      }
      this.usuarioId = usuario.id;
      this.nomeUsuario = usuario.nome;
      this.formulario = this.novaTarefa();
      this.carregarTarefas();
    } catch {
      localStorage.removeItem('usuarioLogado');
      this.router.navigate(['/login']);
    }
  }

  novaTarefa(): Tarefa {
    return {
      titulo: '',
      descricao: '',
      dataVencimento: '',
      concluida: false,
      usuarioId: this.usuarioId
    };
  }

  carregarTarefas(): void {
    if (this.usuarioId <= 0) return;
    this.carregando = true;
    this.erro = '';

    this.tarefaService.listarPorUsuario(this.usuarioId).subscribe({
      next: (resposta: ApiResponse<Tarefa[]>) => {
        this.tarefas = Array.isArray(resposta.dados) ? resposta.dados : [];
        this.carregando = false;
      },
      error: (erro: HttpErrorResponse) => {
        this.erro = erro.error?.mensagem ??
          'Não foi possível carregar as tarefas. Verifique se a API está funcionando.';
        this.carregando = false;
      }
    });
  }

  salvar(): void {
    this.mensagem = '';
    this.erro = '';

    if (this.usuarioId <= 0) {
      this.router.navigate(['/login']);
      return;
    }
    if (!this.formulario.titulo.trim()) {
      this.erro = 'Informe o título da tarefa.';
      return;
    }
    if (!this.formulario.dataVencimento) {
      this.erro = 'Informe a data de vencimento.';
      return;
    }

    const data = new Date(this.formulario.dataVencimento);
    if (Number.isNaN(data.getTime())) {
      this.erro = 'Informe uma data de vencimento válida.';
      return;
    }

    const tarefa: Tarefa = {
      titulo: this.formulario.titulo.trim(),
      descricao: this.formulario.descricao?.trim() ?? '',
      dataVencimento: data.toISOString(),
      concluida: this.formulario.concluida ?? false,
      usuarioId: this.usuarioId
    };

    const idEmEdicao = this.editandoId;
    const requisicao = idEmEdicao !== null
      ? this.tarefaService.atualizar(idEmEdicao, this.usuarioId, tarefa)
      : this.tarefaService.criar(tarefa);

    requisicao.subscribe({
      next: (resposta) => {
        this.mensagem = resposta.mensagem ??
          (idEmEdicao !== null ? 'Tarefa atualizada com sucesso!' : 'Tarefa criada com sucesso!');
        this.cancelarEdicao();
        this.carregarTarefas();
      },
      error: (erro: HttpErrorResponse) => {
        this.erro = erro.error?.mensagem ??
          'Não foi possível salvar a tarefa. Confira os dados enviados à API.';
      }
    });
  }

  editar(tarefa: Tarefa): void {
    if (!tarefa.id) return;
    this.editandoId = tarefa.id;
    this.formulario = {
      ...tarefa,
      dataVencimento: tarefa.dataVencimento ? this.paraDataLocal(tarefa.dataVencimento) : ''
    };
    this.mensagem = '';
    this.erro = '';
  }

  private paraDataLocal(valor: string): string {
    const data = new Date(valor);
    if (Number.isNaN(data.getTime())) return '';
    const ajustarFuso = data.getTimezoneOffset() * 60000;
    return new Date(data.getTime() - ajustarFuso).toISOString().slice(0, 16);
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
        this.mensagem = resposta.mensagem ?? 'Tarefa concluída!';
        this.carregarTarefas();
      },
      error: (erro: HttpErrorResponse) => {
        this.erro = erro.error?.mensagem ?? 'Não foi possível concluir a tarefa.';
      }
    });
  }

  excluir(tarefa: Tarefa): void {
    if (!tarefa.id) return;
    if (!confirm(`Deseja excluir a tarefa "${tarefa.titulo}"?`)) return;

    this.erro = '';
    this.mensagem = '';
    this.tarefaService.excluir(tarefa.id, this.usuarioId).subscribe({
      next: (resposta) => {
        this.mensagem = resposta.mensagem ?? 'Tarefa excluída!';
        this.carregarTarefas();
      },
      error: (erro: HttpErrorResponse) => {
        this.erro = erro.error?.mensagem ?? 'Não foi possível excluir a tarefa.';
      }
    });
  }

  sair(): void {
    localStorage.removeItem('usuarioLogado');
    this.router.navigate(['/login']);
  }
}
