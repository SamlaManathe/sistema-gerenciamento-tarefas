import { Routes } from '@angular/router';
import { ListaTarefas } from './pages/lista-tarefas/lista-tarefas';
import { CadastroUsuario } from './pages/cadastro-usuario/cadastro-usuario';
import { LoginUsuario } from './pages/login-usuario/login-usuario';

export const routes: Routes = [
  { path: 'login', component: LoginUsuario },
  { path: 'cadastro', component: CadastroUsuario },
  { path: 'tarefas', component: ListaTarefas },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
