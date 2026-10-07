# Sistema de Gerenciamento de Tarefas

Projeto desenvolvido para o Bootcamp de Desenvolvimento Backend com .NET.

## Tecnologias Utilizadas

### Back-end

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* Swagger

### Front-end

* Angular 21
* TypeScript
* HttpClient

---

# Estrutura do Projeto

```text
sistema-gerenciamento-tarefas
│
├── back-end
│
│   ├── Controllers
│   ├── Services
│   │   └── Interfaces
│   ├── Repositories
│   │   └── Interfaces
│   ├── Models
│   ├── DTOs
│   ├── Data
│   ├── Responses
│   ├── Migrations
│   ├── Program.cs
│   └── appsettings.json
│
└── front-end
    │
    ├── src
    │   ├── app
    │   │   ├── models
    │   │   └── services
    │   └── environments
    │
    └── angular.json
```

---

# Pré-requisitos

Instalar:

* .NET SDK 8 ou superior
* Node.js 24 ou superior
* Angular CLI

Verificar instalações:

```bash
dotnet --version
node -v
npm -v
ng version
```

---

# Clonando o Projeto

```bash
git clone https://github.com/SamlaManathe/sistema-gerenciamento-tarefas.git
```

Entrar na pasta do projeto:

```bash
cd sistema-gerenciamento-tarefas
```

---

# Configuração do Back-end

Entrar na pasta:

```bash
cd back-end
```

Restaurar dependências:

```bash
dotnet restore
```

Gerar o banco SQLite local:

```bash
dotnet ef database update
```

Executar a API:

```bash
dotnet run
```

A API ficará disponível em:

```text
http://localhost:5100
```

Swagger:

```text
http://localhost:5100/swagger
```

---

# Configuração do Front-end

Entrar na pasta:

```bash
cd front-end
```

Instalar dependências:

```bash
npm install
```

Executar:

```bash
ng serve
```

Aplicação Angular:

```text
http://localhost:4200
```

---

# Configuração da API no Angular

Arquivo:

```text
src/environments/environment.ts
```

Conteúdo:

```ts
export const environment = {
  apiUrl: 'http://localhost:5100/api'
};
```

---

# O que já foi implementado

## Back-end

* Estrutura inicial do projeto
* Models
* ApplicationDbContext
* Configuração do Entity Framework Core
* Configuração do SQLite
* Migration inicial
* Configuração do Swagger
* Configuração do CORS

## Front-end

* Estrutura Angular criada
* Models iniciais
* Services iniciais
* Configuração do environment

---

# Divisão de Tarefas

## Pessoa 1 - Infraestrutura (Samla)

Responsável por:

* Estrutura inicial do projeto
* Models
* DbContext
* Entity Framework Core
* SQLite
* Migrations
* Swagger
* CORS

---

## Pessoa 2 - Repositories

Implementar:

### Interfaces

```text
IUsuarioRepository
ITarefaRepository
```

### Classes

```text
UsuarioRepository
TarefaRepository
```

Responsabilidades:

* CRUD de Usuário
* CRUD de Tarefa
* Acesso ao banco de dados via Entity Framework

---

## Pessoa 3 - Services

Implementar:

### Interfaces

```text
IUsuarioService
ITarefaService
```

### Classes

```text
UsuarioService
TarefaService
```

Responsabilidades:

* Regras de negócio
* Validações
* Comunicação entre Controllers e Repositories

---

## Pessoa 4 - Controllers e DTOs

Implementar:

### Controllers

```text
UsuarioController
TarefaController
```

### DTOs

```text
CriarUsuarioDTO
CriarTarefaDTO
AtualizarTarefaDTO
```

Responsabilidades:

* Endpoints da API
* Receber requisições
* Retornar respostas HTTP corretas

---

## Pessoa 5 - Angular

Implementar:

### Telas

* Cadastro de Usuária
* Lista de Tarefas
* Formulário de Tarefa
* Edição de Tarefa

### Integração

* UsuarioService
* TarefaService

### Funcionalidades

* Criar tarefa
* Editar tarefa
* Excluir tarefa
* Marcar tarefa como concluída
* Exibir mensagens de sucesso e erro

---

# Fluxo da Aplicação

```text
Angular
    ↓
Controllers
    ↓
Services
    ↓
Repositories
    ↓
Entity Framework
    ↓
SQLite
```

---

# Boas Práticas

* Sempre criar branch para novas funcionalidades.
* Não desenvolver diretamente na branch main.
* Fazer commits pequenos e descritivos.
* Testar endpoints no Swagger antes de integrar com o Angular.
* Atualizar a branch local antes de abrir Pull Request.

Exemplo:

```bash
git checkout -b feat/nome-da-feature
```
