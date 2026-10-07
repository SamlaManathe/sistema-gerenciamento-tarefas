import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {

  private apiUrl = `${environment.apiUrl}/tarefas`;

  constructor(private http: HttpClient) { }

}