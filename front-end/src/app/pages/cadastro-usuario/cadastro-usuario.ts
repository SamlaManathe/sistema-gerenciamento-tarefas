import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario } from '../../models/usuario';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../models/api-response';
import { UsuarioCriado } from '../../services/usuario.service';

@Component({
  selector: 'app-cadastro-usuario',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './cadastro-usuario.html',
  styleUrl: './cadastro-usuario.css'
})
export class CadastroUsuario {
  usuario: Usuario = { nome: '', email: '', senha: '' };
  mensagem = '';
  erro = '';
  carregando = false;

  constructor(
    private usuarioService: UsuarioService,
    private router: Router
  ) {}

  cadastrar(): void {
    this.mensagem = '';
    this.erro = '';

    if (!this.usuario.nome.trim() || !this.usuario.email.trim() || this.usuario.senha.length < 6) {
      this.erro = 'Preencha todos os campos. A senha deve ter pelo menos 6 caracteres.';
      return;
    }

    this.carregando = true;

    this.usuarioService.cadastrar({
      ...this.usuario,
      nome: this.usuario.nome.trim(),
      email: this.usuario.email.trim()
    }).subscribe({
      next: (resposta: ApiResponse<UsuarioCriado>) => {
        this.carregando = false;
        if (!resposta.sucesso) {
          this.erro = resposta.mensagem || 'Não foi possível cadastrar a usuária.';
          return;
        }
        this.mensagem = resposta.mensagem;
        this.router.navigate(['/login']);
      },
      error: (erro: HttpErrorResponse) => {
        this.carregando = false;
        this.erro = erro.error?.mensagem ?? 'Não foi possível cadastrar a usuária.';
      }
    });
  }
}
