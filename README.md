# 🎮 FIAP Cloud Games - API (.NET 8)

## 📌 Visão Geral

A **FIAP Cloud Games (FCG)** é uma plataforma para venda de jogos digitais e gerenciamento de biblioteca de usuários.

Este projeto representa a **Fase 1 do Tech Challenge**, cujo objetivo é construir uma **API REST em .NET 8** para:

* Cadastro de usuários
* Autenticação via JWT
* Gerenciamento de jogos adquiridos

---

## 🧱 Arquitetura

O projeto segue uma abordagem **Monolítica com princípios de DDD (Domain-Driven Design)**.

### 📂 Estrutura de Pastas


```
- FiapCloudGames.Domain
  - Entidades de domínio, interfaces e regras do negócio (modelos puros, sem dependências de infraestrutura).

- FiapCloudGames.Application
  - Serviços de aplicação, casos de uso, orquestração entre domínio e infraestrutura.

- FiapCloudGames.Infrastructure
  - Implementações de persistência, integrações (por exemplo, Entity Framework Core, repositórios, provedores externos).

- FiapCloudGames.API
  - Camada de apresentação: controllers HTTP, endpoints, configuração do Web API (Swagger, autenticação, etc.).

- FiapCloudGames.Testes
  - Projetos de teste automatizados (unitários e/ou de integração).
```

---

## ⚙️ Tecnologias Utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT (Autenticação)
* Swagger (Documentação)
* BCrypt (Hash de senha)

---

## 🚀 Como Executar o Projeto

### 1. Clonar o repositório

```bash
git clone <url-do-repositorio>
cd .\FiapCloudGames.API\
```

---

### 2. Restaurar dependências

```bash
dotnet restore
```

---

### 3. Configurar banco de dados

No arquivo `appsettings.json`:

```json
"ConnectionStrings": {
  "ConnectionString": "Server=<host>;Database=FiapCloudGames;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

### 4. Para adicionar Migrations

```bash
dotnet ef migrations add NomeDaMigration 
```

---

### 5. Rodar o projeto

```bash
dotnet run
```

Acesse o Swagger em:

```
http://localhost:5137/swagger/index.html
```

---

## 👤 Funcionalidades

### 📌 Usuários

* Cadastro de usuarios
* Listagem de todos os usuarios e um por id
* Atualização de usuario

---

### 🔐 Autenticação

* Login com email e senha
* Geração de token JWT
* Controle de acesso por roles

---

### 🎮 Jogos

* Cadastro de jogos (Admin)
* Associação de jogos ao usuário
* Listagem da biblioteca
* Atualização do jogo
* Criação de promoções

---

## 🔒 Regras de Negócio

* Email deve ser válido e único
* Senha deve conter:

  * Mínimo 8 caracteres
  * Letras + números + caracteres especiais
* Usuários possuem dois níveis:

  * `User`
  * `Admin`

---

## 🧠 Modelagem de Domínio

### 👤 User

```csharp
Id
Name
LastName
Email
PasswordHash
Role
```

---

### 🎮 Game

```csharp
Id
Title
Price
UserId
```

### 👤🎮 UserGames

```csharp
UserId
user
GameId
game
UserId
PurchaseDate
ValuePay
```

---

## 🧪 Testes

* Testes unitários para:

  * Xunit
  * Mock
  * Reqnroll
 
### Rodar os testes

```bash
cd .\FiapCloudGames.testes\
dotnet tests
```
---

## 🧰 Boas Práticas Aplicadas

* Separação por camadas (DDD)
* Uso de DTOs
* Hash de senha (BCrypt)
* Injeção de dependência
* Swagger para documentação


---

## 🧾 Padrão de Commits (Conventional Commits)

Este projeto segue o padrão **Conventional Commits**, facilitando leitura, versionamento e automações.

### 📌 Estrutura do commit

```
<tipo>: <descrição curta>
```

---

### 🏷️ Tipos de commits

| Tipo     | Uso                              |
| -------- | -------------------------------- |
| feat     | Nova funcionalidade              |
| fix      | Correção de bug                  |
| docs     | Documentação                     |
| style    | Formatação (sem alterar lógica)  |
| refactor | Refatoração                      |
| test     | Testes                           |
| chore    | Tarefas internas (build, config) |

---

### ✅ Exemplos

```bash
feat: adicionar cadastro de usuário
fix: corrigir validação de senha
docs: atualizar README com instruções
refactor: melhorar estrutura do UserService
test: adicionar testes de autenticação
chore: configurar EF Core
```

---

### 🚨 Boas práticas

* Use mensagens claras e objetivas
* Commits pequenos e frequentes
* Um commit = uma responsabilidade
* Sempre em português ou inglês (padronizar no time)

---

### 🔥 Exemplo real do projeto

```bash
feat: criar entidade User
feat: adicionar DbContext
feat: criar endpoint de cadastro
fix: corrigir validação de email duplicado
```

---

## 📄 Entregáveis

* API funcional
* Código versionado
* Documentação
* Swagger disponível
* Base pronta para evolução

---

## 👥 Autores

* Nome do Grupo: **
* Integrantes:
```bash
  --Anderson Alves Koshimizu - RM371858 (Anderson Koshimizu)
  --André Felipe da Costa - RM373980 
  --Esterffeson José Duarte de Abreu - RM372754 (EsterffesonDuarte
  --Esther Novais Pinheiro Silva - RM373639  (Esther Novais)
  --Jacqueline Nascimento Albuquerque - RM366275  (Jacqueline)
```

---

## 🔗 Links

* Repositório: *[Adicionar]*
* Vídeo: *[Adicionar]*
* Documentação DDD: *[Adicionar]*

---

## 🏁 Conclusão

Este projeto estabelece a base da plataforma FIAP Cloud Games, garantindo:

* Escalabilidade
* Organização
* Segurança

Preparando o sistema para as próximas fases, como matchmaking e servidores online.

## 👥 Logins iniciais
### User Admin
* email: andre@email.com
* senha: Admin@!23

---
### User Common
* email: carlos@email.com
* senha: Pass@!23


---
