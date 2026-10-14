import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Tarefa } from '../models/tarefa';
import { ApiResponse } from '../models/api-response';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {

  private apiUrl = `${environment.apiUrl}/Tarefa`;

  constructor(private http: HttpClient) { }

  listarPorUsuario(usuarioId: number) {
  return this.http.get<ApiResponse<Tarefa[]>>(
    `${this.apiUrl}/usuario/${usuarioId}`
  );
}

criar(tarefa: Tarefa) {
  return this.http.post<ApiResponse<Tarefa>>(
    this.apiUrl, tarefa
  );
}

atualizar(id: number, usuarioId: number, tarefa: Tarefa) {
  return this.http.put<ApiResponse<Tarefa>>(
    `${this.apiUrl}/${id}?usuarioId=${usuarioId}`,
    tarefa
  );
}

concluir(id: number, usuarioId: number) {
  return this.http.patch<ApiResponse<Tarefa>>(
    `${this.apiUrl}/${id}/concluir?usuarioId=${usuarioId}`,
    {}
  );
}

excluir(id: number, usuarioId: number) {
  return this.http.delete<ApiResponse<unknown>>(
    `${this.apiUrl}/${id}?usuarioId=${usuarioId}`
  );
}

}