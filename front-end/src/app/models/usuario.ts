export interface Usuario {
  id?: number;
  nome: string;
  email: string;
  senha: string;
}

export interface UsuarioLogado {
  id: number;
  nome: string;
  email: string;
}
