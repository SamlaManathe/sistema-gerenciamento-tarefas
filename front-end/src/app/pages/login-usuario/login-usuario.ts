import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { UsuarioService } from '../../services/usuario.service';
import { UsuarioLogado } from '../../models/usuario';
import { ApiResponse } from '../../models/api-response';

@Component({
  selector: 'app-login-usuario',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login-usuario.html',
  styleUrl: './login-usuario.css',
})
export class LoginUsuario {
  email = '';
  senha = '';
  erro = '';
  carregando = false;

  constructor(
    private usuarioService: UsuarioService,
    private router: Router
  ) {}

  entrar(): void {
    this.erro = '';

    if (!this.email.trim() || !this.senha) {
      this.erro = 'Informe seu e-mail e sua senha.';
      return;
    }

    this.carregando = true;

    this.usuarioService.login(this.email.trim(), this.senha).subscribe({
      next: (resposta: ApiResponse<UsuarioLogado>) => {
        this.carregando = false;

        const usuario = resposta.dados;
        if (!resposta.sucesso || !usuario || !Number.isInteger(usuario.id) || usuario.id <= 0) {
          this.erro = 'A API não retornou os dados de uma usuária válida.';
          return;
        }

        localStorage.setItem('usuarioLogado', JSON.stringify(usuario));
        this.router.navigate(['/tarefas']);
      },
      error: (erro: HttpErrorResponse) => {
        this.carregando = false;
        this.erro = erro.error?.mensagem ??
          'Não foi possível entrar. Verifique a API e suas credenciais.';
      }
    });
  }
}
