## Back-end

### Controllers

```text
Controllers
├── UsuarioController.cs
└── TarefaController.cs
```

Responsável por receber as requisições HTTP da aplicação.

Exemplos:

* GET /api/tarefa
* POST /api/tarefa
* PUT /api/tarefa/{id}
* DELETE /api/tarefa/{id}

O Controller não deve acessar o banco diretamente. Ele deve chamar os Services.

Fluxo:

```text
Controller → Service
```

---

### Services

```text
Services
│
├── Interfaces
│   ├── IUsuarioService.cs
│   └── ITarefaService.cs
│
├── UsuarioService.cs
└── TarefaService.cs
```

Responsável pelas regras de negócio da aplicação.

Exemplos:

* Validar se uma tarefa possui título.
* Validar datas.
* Marcar tarefa como concluída.
* Verificar se usuário existe.

Os Services recebem chamadas dos Controllers e utilizam os Repositories.

Fluxo:

```text
Controller → Service → Repository
```

---

### Services/Interfaces

```text
Interfaces
├── IUsuarioService.cs
└── ITarefaService.cs
```

Contém os contratos dos Services.

Exemplo:

```csharp
public interface ITarefaService
{
    Task<List<Tarefa>> Listar();
    Task<Tarefa> BuscarPorId(int id);
}
```

As implementações ficam em:

```text
TarefaService.cs
UsuarioService.cs
```

---

### Repositories

```text
Repositories
│
├── Interfaces
│   ├── IUsuarioRepository.cs
│   └── ITarefaRepository.cs
│
├── UsuarioRepository.cs
└── TarefaRepository.cs
```

Responsável pelo acesso ao banco de dados.

Exemplos:

* Buscar registros.
* Inserir registros.
* Atualizar registros.
* Excluir registros.

Nenhuma regra de negócio deve ficar aqui.

Fluxo:

```text
Repository → Entity Framework → SQLite
```

---

### Repositories/Interfaces

```text
Interfaces
├── IUsuarioRepository.cs
└── ITarefaRepository.cs
```

Contém os contratos dos Repositories.

Exemplo:

```csharp
public interface ITarefaRepository
{
    Task<List<Tarefa>> Listar();
    Task<Tarefa?> BuscarPorId(int id);
}
```

As implementações ficam em:

```text
TarefaRepository.cs
UsuarioRepository.cs
```

---

### Models

```text
Models
├── Usuario.cs
└── Tarefa.cs
```

Representam as entidades do banco de dados.

Essas classes geram automaticamente as tabelas através do Entity Framework.

Exemplo:

```text
Usuario
├── Id
├── Nome
├── Email
└── Senha
```

```text
Tarefa
├── Id
├── Titulo
├── Descricao
├── DataVencimento
├── Concluida
└── UsuarioId
```

Sempre que um Model for alterado é necessário criar uma nova Migration.

---

### DTOs

```text
DTOs
├── CriarUsuarioDTO.cs
├── CriarTarefaDTO.cs
└── AtualizarTarefaDTO.cs
```

DTO significa Data Transfer Object.

São objetos usados para entrada e saída de dados da API.

Evita expor diretamente as entidades do banco.

Exemplo:

Ao cadastrar uma usuária:

```json
{
  "nome": "Maria",
  "email": "maria@email.com",
  "senha": "123456"
}
```

Esse objeto pode ser recebido por um DTO ao invés do Model.

---

### Data

```text
Data
└── ApplicationDbContext.cs
```

Contém a configuração do Entity Framework.

Responsável por conectar a aplicação ao banco SQLite.

Exemplo:

```csharp
public DbSet<Usuario> Usuarios { get; set; }

public DbSet<Tarefa> Tarefas { get; set; }
```

Toda comunicação entre Entity Framework e banco passa pelo DbContext.

---

### Responses

```text
Responses
└── ApiResponse.cs
```

Responsável por padronizar as respostas da API.

Exemplo:

```json
{
  "sucesso": true,
  "mensagem": "Tarefa criada com sucesso",
  "dados": { }
}
```

Objetivo:

* Padronizar retornos.
* Facilitar consumo pelo Angular.
* Centralizar mensagens.

---

### Migrations

```text
Migrations
```

Criadas automaticamente pelo Entity Framework.

Responsáveis por registrar alterações na estrutura do banco.

Exemplos:

* Criar tabela.
* Adicionar coluna.
* Alterar relacionamento.

Comandos:

```bash
dotnet ef migrations add NomeDaMigration
dotnet ef database update
```

Não editar manualmente sem necessidade.


Cada camada possui apenas uma responsabilidade, facilitando manutenção, testes e evolução do projeto.
