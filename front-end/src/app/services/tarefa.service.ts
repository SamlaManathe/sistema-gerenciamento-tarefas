import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Tarefa } from '../models/tarefa';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {

  private apiUrl = `${environment.apiUrl}/Tarefa`;

  constructor(private http: HttpClient) { }

  listarPorUsuario(usuarioId: number) {
    return this.http.get<any>(
      `${this.apiUrl}/usuario/${usuarioId}`
    );
  }

  criar(tarefa: Tarefa) {
    return this.http.post<any>(
      this.apiUrl,
      tarefa
    );
  }

  atualizar(id: number, usuarioId: number, tarefa: Tarefa) {
    return this.http.put<any>(
      `${this.apiUrl}/${id}?usuarioId=${usuarioId}`,
      tarefa
    );
  }

  concluir(id: number, usuarioId: number) {
    return this.http.patch<any>(
      `${this.apiUrl}/${id}/concluir?usuarioId=${usuarioId}`,
      {}
    );
  }

  excluir(id: number, usuarioId: number) {
    return this.http.delete<any>(
      `${this.apiUrl}/${id}?usuarioId=${usuarioId}`
    );
  }

}