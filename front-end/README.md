## Front-end

### Models

```text
models
├── usuario.ts
└── tarefa.ts
```

Representam os dados recebidos e enviados para a API.

Exemplo:

```ts
export interface Usuario {
  id?: number;
  nome: string;
  email: string;
  senha: string;
}
```

---

### Services

```text
services
├── usuario.service.ts
└── tarefa.service.ts
```

Responsáveis por consumir a API.

Utilizam HttpClient.

Exemplos:

```ts
GET /api/tarefas
POST /api/tarefas
PUT /api/tarefas
DELETE /api/tarefas
```

Os componentes Angular não devem fazer requisições diretamente.

Fluxo:

```text
Componente → Service → API
```

---

### Environments

```text
environments
└── environment.ts
```

Centraliza configurações da aplicação.

Exemplo:

```ts
export const environment = {
  apiUrl: 'http://localhost:5100/api'
};
```

Caso a URL da API mude, basta alterar este arquivo.

---

## Arquitetura Geral

```text
Angular
    ↓
Components
    ↓
Services (Angular)
    ↓
Controllers
    ↓
Services (.NET)
    ↓
Repositories
    ↓
Entity Framework
    ↓
SQLite
```

Cada camada possui apenas uma responsabilidade, facilitando manutenção, testes e evolução do projeto.
