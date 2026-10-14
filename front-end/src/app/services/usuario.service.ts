
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Usuario } from '../models/usuario';
import { ApiResponse } from '../models/api-response';

export interface UsuarioCriado {
  id: number;
  nome: string;
  email: string;
}

export interface UsuarioLogado {
  id: number;
  nome: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  private readonly apiUrl = `${environment.apiUrl}/Usuario`;

  constructor(private http: HttpClient) {}

  cadastrar(usuario: Usuario) {
    return this.http.post<ApiResponse<UsuarioCriado>>(
      this.apiUrl,
      usuario
    );
  }

  login(email: string, senha: string) {
    return this.http.post<ApiResponse<UsuarioLogado>>(
      `${this.apiUrl}/login`,
      { email, senha }
    );
  }
}
