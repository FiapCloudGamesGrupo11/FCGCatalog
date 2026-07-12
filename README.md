# FCGCatalog - Microsserviço de Catálogo de Jogos

## Descrição

O **FCGCatalog** é o microsserviço central da plataforma **FIAP Cloud Games (FCG)** responsável pelo gerenciamento do catálogo de jogos, pedidos de compra e histórico de compras dos usuários. Ele fornece funcionalidades completas de catálogo de games, permitindo que usuários naveguem, comprem jogos e acompanhem seus pedidos.

Este microsserviço implementa arquitetura em camadas com separação clara entre API, Aplicação, Domínio e Infraestrutura. Utiliza **JWT** para autenticação, **RabbitMQ** para comunicação assíncrona com outros microsserviços e **SQL Server** para persistência de dados.

---

## Funcionalidades Principais

- **Catálogo de Jogos**: Listagem, busca e visualização de detalhes dos jogos
- **Gerenciamento de Jogos**: CRUD completo de jogos (Admin only)
- **Gerenciamento de Promoções**: Criar e gerenciar ofertas e descontos
- **Processamento de Pedidos**: Receber pedidos de compra de usuários
- **Histórico de Compras**: Consultar jogos adquiridos pelo usuário
- **Integração com Pagamentos**: Consome eventos de pagamento processado
- **Autenticação JWT**: Segurança baseada em tokens JWT
- **Logging Estruturado**: Serilog com persistência em banco de dados
- **Documentação Swagger**: API completamente documentada
- **RabbitMQ**: Comunicação assíncrona com Payment e Notification

---

## Arquitetura em Camadas

```
┌─────────────────────────────────────────────────────────┐
│       FiapCloudGames.API (Presentation)                 │
│  Controllers, Middlewares, Extensions                   │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│    FiapCloudGames.Application (Business Logic)          │
│  Services, DTOs, Use Cases, Behaviors                   │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│     FiapCloudGames.Domain (Business Rules)              │
│  Entities, Interfaces, Enums, Value Objects             │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│    FiapCloudGames.Infrastructure (Data Access)          │
│  Database, Messaging, Repository, Serilog              │
└─────────────────────────────────────────────────────────┘
```

---

## Dependências

- **Microsoft.AspNetCore.Authentication.JwtBearer**: Autenticação JWT
- **Serilog.AspNetCore**: Logging estruturado
- **Serilog.Sinks.MSSqlServer**: Persistência de logs em banco
- **Swashbuckle.AspNetCore**: Swagger/OpenAPI
- **RabbitMQ.Client**: Cliente para RabbitMQ
- **Entity Framework Core**: ORM

---

## Como Executar

### Pré-requisitos

- .NET 8.0 SDK ou superior instalado
- Docker e Docker Compose (para execução containerizada)
- SQL Server em execução (ou use o docker-compose fornecido)
- RabbitMQ em execução (ou use o docker-compose fornecido)

### Opção 1: Executar com Docker Compose

```bash
# Na raiz do projeto FCGCatalog (mesmo diretório deste README)
docker-compose up
```

Este comando inicia:
- **SQL Server** na porta 1433
- **RabbitMQ** na porta 5672 (AMQP) e 15672 (Management UI)
- **FCGCatalog API** na porta 8080

Verifique se os containers estão saudáveis:

```bash
docker-compose ps
```

---

### Opção 2: Executar Localmente

1. **Certifique-se de que SQL Server está em execução**:

```bash
# Use Docker apenas para SQL Server
docker run -d --name sqlserver \
  -e 'ACCEPT_EULA=Y' \
  -e 'MSSQL_SA_PASSWORD=Your_strong!Passw0rd' \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

2. **Certifique-se de que RabbitMQ está em execução**:

```bash
# Use Docker para RabbitMQ
docker run -d --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management
```

3. **Configure as variáveis de ambiente** em `FiapCloudGames.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "ConnectionString": "Server=localhost,1433;Database=FiapCloudGames;User Id=sa;Password=Your_strong!Passw0rd;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "_07P\\Wbk|}j:Bp-zC/%`701{Owu?B_dJLSDJaljdlajdlajlda",
    "Issuer": "FiapCloudGames",
    "Audience": "FiapCloudGames.API"
  },
  "RabbitMq": {
    "Host": "localhost",
    "User": "guest",
    "Password": "guest",
    "OrderQueueName": "order-placed",
    "PaymentProcessQueueName": "catalog-payment-processed"
  }
}
```

4. **Restaure as dependências, aplique migrações e execute a API**:

```bash
cd FiapCloudGames.API

# Restaurar dependências
dotnet restore

# Aplicar migrações de banco de dados
dotnet ef database update --project ../FiapCloudGames.Infrastructure

# Executar a API
dotnet run
```

A API estará disponível em:
- **HTTP**: http://localhost:8080
- **Swagger**: http://localhost:8080/swagger

---

## Acessar a API

### Documentação Swagger com Autenticação JWT

1. Acesse http://localhost:8080/swagger
2. Clique no botão **"Authorize"** (cadeado no canto superior direito)
3. Insira um token JWT válido no formato: `Bearer <seu-token-aqui>`
4. Clique em "Authorize"
5. Agora todos os endpoints protegidos estarão acessíveis

### OpenAPI JSON

- **URL**: http://localhost:8080/swagger/v1/swagger.json

### Health Check

- **URL**: http://localhost:8080/health

### RabbitMQ Management UI

- **URL**: http://localhost:15672
- **Usuário**: guest
- **Senha**: guest

---

## Endpoints da API

### Documentação Completa

Para uma documentação interativa completa, acesse: http://localhost:8080/swagger

### Endpoints de Games

#### 1. Listar Todos os Games

```
GET /api/games
```

**Parâmetros**:
- `page` (opcional): Número da página (padrão: 1)
- `pageSize` (opcional): Itens por página (padrão: 10)

**Resposta (200 OK)**:
```json
[
  {
    "id": "uuid",
    "name": "Elden Ring",
    "description": "A masterpiece of game design",
    "category": "RPG",
    "price": 249.90
  }
]
```

---

#### 2. Obter Game por ID

```
GET /api/games/{id}
```

**Resposta (200 OK)**:
```json
{
  "id": "uuid",
  "name": "Elden Ring",
  "description": "A masterpiece of game design",
  "category": "RPG",
  "price": 249.90
}
```

---

#### 3. Criar Novo Game (Admin Only)

```
POST /api/games
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "name": "Elden Ring",
  "description": "A masterpiece of game design",
  "category": "RPG",
  "price": 249.90
}
```

**Resposta (201 Created)**:
```json
{
  "id": "uuid",
  "name": "Elden Ring",
  "description": "A masterpiece of game design",
  "category": "RPG",
  "price": 249.90
}
```

---

#### 4. Atualizar Game (Admin Only)

```
PUT /api/games/{id}
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "name": "Elden Ring - Updated",
  "description": "A masterpiece of game design - Updated",
  "category": "RPG",
  "price": 259.90
}
```

---

#### 5. Deletar Game (Admin Only)

```
DELETE /api/games/{id}
Authorization: Bearer <admin-token>
```

---

### Endpoints de Pedidos (Orders)

#### 1. Criar Novo Pedido (Order)

```
POST /api/usergames/order
Authorization: Bearer <user-token>
Content-Type: application/json

{
  "gameId": "uuid",
  "paymentDetails": {
    "paymentMethod": "CREDIT_CARD",
    "cardNumber": "1234-5678-9012-7890",
    "cvv": "123",
    "expirationDate": "12/29"
  }
}
```

**Resposta (201 Created)**:
```json
{
  "orderId": "ORD_ABC123XYZ",
  "status": "Pending",
  "totalPrice": 249.90
}
```

---

#### 2. Listar Pedidos do Usuário

```
GET /api/usergames
Authorization: Bearer <user-token>
```

**Resposta (200 OK)**:
```json
[
  {
    "id": "uuid",
    "gameId": "uuid",
    "gameName": "Elden Ring",
    "purchaseDate": "2026-07-12T18:30:00Z",
    "price": 249.90
  }
]
```

---

#### 3. Obter Detalhes do Pedido

```
GET /api/usergames/{orderId}
Authorization: Bearer <user-token>
```

---

### Endpoints de Promoções (On Sale)

#### 1. Listar Promoções Ativas

```
GET /api/onsale
```

**Resposta (200 OK)**:
```json
[
  {
    "id": "uuid",
    "gameId": "uuid",
    "gameName": "Elden Ring",
    "originalPrice": 249.90,
    "discountPercentage": 20,
    "finalPrice": 199.92,
    "startDate": "2026-07-12T00:00:00Z",
    "endDate": "2026-07-31T23:59:59Z"
  }
]
```

---

#### 2. Criar Promoção (Admin Only)

```
POST /api/onsale
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "gameId": "uuid",
  "discountPercentage": 20,
  "startDate": "2026-07-12T00:00:00Z",
  "endDate": "2026-07-31T23:59:59Z"
}
```

---

#### 3. Atualizar Promoção (Admin Only)

```
PUT /api/onsale/{id}
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "discountPercentage": 25,
  "endDate": "2026-08-31T23:59:59Z"
}
```

---

#### 4. Deletar Promoção (Admin Only)

```
DELETE /api/onsale/{id}
Authorization: Bearer <admin-token>
```

---

##  Eventos do RabbitMQ

### 1. OrderPlacedEvent (Publicado)

**Quando**: Um usuário cria um pedido

**Fila**: `order-placed`

**Payload**:
```json
{
  "orderId": "ORD_ABC123XYZ",
  "userId": "uuid",
  "gameId": "uuid",
  "amount": 249.90,
  "paymentDetails": {
    "paymentMethod": "CREDIT_CARD",
    "cardNumber": "1234-5678-9012-7890",
    "cvv": "123",
    "expirationDate": "12/29"
  }
}
```

**Consumidor**: FCGPayment

---

### 2. PaymentProcessedEvent (Consumido)

**Origem**: FCGPayment

**Exchange**: `payment.exchange`

**Payload**:
```json
{
  "orderId": "ORD_ABC123XYZ",
  "paymentId": "PAY_12345ABCDE",
  "userId": "uuid",
  "gameId": "uuid",
  "amount": 249.90,
  "status": "Approved|Rejected",
  "reason": "Motivo se rejeitado",
  "processedAt": "2026-07-12T18:30:01Z"
}
```

**Ação**: Atualiza status do pedido, registra a compra do usuário

---

## Estrutura do Projeto

```
FCGCatalog/
├── FiapCloudGames.API/                 # Camada de Apresentação
│   ├── Controllers/
│   │   ├── GameController.cs           # Endpoints de games
│   │   ├── OnSaleController.cs         # Endpoints de promoções
│   │   └── UsersGamesController.cs     # Endpoints de pedidos
│   ├── Extensions/                     # Configurações de extensão
│   ├── Middlewares/                    # Middleware customizado
│   ├── Program.cs                      # Configuração da aplicação
│   ├── FiapCloudGames.API.csproj       # Arquivo de projeto
│   ├── appsettings.json                # Configurações
│   └── appsettings.Development.json    # Configurações de desenvolvimento
│
├── FiapCloudGames.Application/         # Camada de Aplicação
│   ├── Services/
│   │   ├── GameService.cs              # Serviços de games
│   │   ├── OnSaleService.cs            # Serviços de promoções
│   │   └── UserGameService.cs          # Serviços de pedidos
│   ├── DTOs/
│   │   ├── Game/                       # DTOs de games
│   │   ├── OnSale/                     # DTOs de promoções
│   │   ├── User/                       # DTOs de usuários
│   │   └── UserGame/                   # DTOs de pedidos
│   ├── Behaviors/                      # Comportamentos (pipeline)
│   ├── Interfaces/                     # Interfaces de contrato
│   └── ApplicationConfigModule.cs      # Configuração da camada
│
├── FiapCloudGames.Domain/              # Camada de Domínio
│   ├── Entity/
│   │   ├── BaseEntity.cs               # Entidade base
│   │   ├── Game.cs                     # Entidade Game
│   │   ├── OnSale.cs                   # Entidade OnSale
│   │   ├── User.cs                     # Entidade User
│   │   ├── UsersGames.cs               # Entidade UsersGames (relacionamento)
│   │   ├── MessageBus/                 # Modelos de eventos
│   │   └── PaymentResult.cs            # Resultado de pagamento
│   ├── Enums/                          # Enumerações
│   ├── Interfaces/                     # Interfaces de contrato
│   └── FiapCloudGames.Domain.csproj    # Arquivo de projeto
│
├── FiapCloudGames.Infrastructure/      # Camada de Infraestrutura
│   ├── Persistence/                    # Configurações EF Core
│   ├── MessageBus/
│   │   ├── IRabbitMqConnection.cs      # Interface de conexão
│   │   ├── RabbitMqConnection.cs       # Implementação
│   │   ├── RabbitMqPublisher.cs        # Publisher
│   │   ├── RabbitMqConsumer.cs         # Consumer
│   │   ├── RabbitMqWorker.cs           # Worker
│   │   └── RabbitMqSettings.cs         # Configurações
│   ├── Repository/                     # Repositories
│   ├── Utils/                          # Utilitários
│   ├── InfrastructureConfigModule.cs   # Configuração da camada
│   └── FiapCloudGames.Infrastructure.csproj # Arquivo de projeto
│
├── FiapCloudGames.Testes/              # Testes Unitários
│
├── Dockerfile                          # Build do container
├── docker-compose.yml                  # Orquestração de containers
└── README.md                           # Este arquivo
```

---

## Configurações

### appsettings.json

```json
{
  "ConnectionStrings": {
    "ConnectionString": "Server=localhost,1433;Database=FiapCloudGames;User Id=sa;Password=Your_strong!Passw0rd;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "_07P\\Wbk|}j:Bp-zC/%`701{Owu?B_dJLSDJaljdlajdlajlda",
    "Issuer": "FiapCloudGames",
    "Audience": "FiapCloudGames.API"
  },
  "RabbitMq": {
    "Host": "localhost",
    "User": "guest",
    "Password": "guest",
    "OrderQueueName": "order-placed",
    "PaymentProcessQueueName": "catalog-payment-processed"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Variáveis de Ambiente

```bash
# Banco de Dados
export ConnectionStrings__ConnectionString=Server=localhost,1433;Database=FiapCloudGames;User Id=sa;Password=Your_strong!Passw0rd;TrustServerCertificate=True

# JWT
export JwtSettings__SecretKey=sua-chave-secreta-super-segura
export JwtSettings__Issuer=FiapCloudGames
export JwtSettings__Audience=FiapCloudGames.API

# RabbitMQ
export RabbitMq__Host=localhost
export RabbitMq__User=guest
export RabbitMq__Password=guest
export RabbitMq__OrderQueueName=order-placed
export RabbitMq__PaymentProcessQueueName=catalog-payment-processed
```

---

## Banco de Dados

### Esquema Principal

```sql
-- Tabela de Jogos
CREATE TABLE Games (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Tabela de Usuários
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Tabela de Compras dos Usuários
CREATE TABLE UsersGames (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    GameId UNIQUEIDENTIFIER NOT NULL,
    OrderId NVARCHAR(50) NOT NULL UNIQUE,
    OrderStatus NVARCHAR(50) NOT NULL,
    PurchaseDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Tabela de Promoções
CREATE TABLE OnSales (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    GameId UNIQUEIDENTIFIER NOT NULL,
    DiscountPercentage DECIMAL(5, 2) NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Tabela de Logs (criada automaticamente pelo Serilog)
CREATE TABLE Logs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MessageTemplate NVARCHAR(MAX),
    Level VARCHAR(128),
    TimeStamp DATETIME2,
    Exception NVARCHAR(MAX),
    LogEvent NVARCHAR(MAX)
);
```

### Executar Migrações

```bash
# Adicionar migração
dotnet ef migrations add MigrationName --project FiapCloudGames.Infrastructure

# Aplicar migrações
dotnet ef database update --project FiapCloudGames.Infrastructure

# Listar migrações
dotnet ef migrations list --project FiapCloudGames.Infrastructure

# Reverter para uma migração anterior
dotnet ef database update PreviousMigrationName --project FiapCloudGames.Infrastructure
```
---

## Fluxo de Compra Completo

```
1. Usuário faz login (obtém JWT token)
   ↓
2. Navega no catálogo: GET /api/games
   ↓
3. Visualiza detalhes: GET /api/games/{id}
   ↓
4. Cria pedido: POST /api/usergames/order
   ↓
5. OrderPlacedEvent publicado no RabbitMQ
   ↓
6. FCGPayment consome e processa pagamento
   ↓
7. PaymentProcessedEvent retorna para Catalog
   ↓
8. Catalog atualiza status do pedido
   ↓
9. Se aprovado:
   ├─→ Registra jogo na biblioteca do usuário
   ├─→ FCGNotification envia confirmação de compra
   └─→ Usuário pode consultar pedidos: GET /api/usergames
   ↓
10. Se rejeitado:
    └─→ Pedido mantém status "Rejected"
```

---

## Tipos de Dados de Entrada

### PaymentDetails (ao criar pedido)

```json
{
  "paymentMethod": "CREDIT_CARD|PIX|BOLETO",
  "cardNumber": "1234-5678-9012-7890",
  "cvv": "123",
  "expirationDate": "12/29"
}
```

### Categorias de Jogos Suportadas

- `RPG`: Role-Playing Games
- `FPS`: First-Person Shooter
- `STRATEGY`: Estratégia
- `INDIE`: Indie Games
- `PUZZLE`: Puzzle
- `SPORTS`: Esportes
- `ADVENTURE`: Aventura
- `ACTION`: Ação

---

## Relacionamento com Outros Microsserviços

### FCGUser
- **Consome**: Informações de usuário para contexto
- **Publica**: Não publica eventos diretos

### FCGPayment
- **Publica**: `OrderPlacedEvent` quando pedido é criado
- **Consome**: `PaymentProcessedEvent` para atualizar status

### FCGNotification
- **Consome**: Indiretamente via PaymentProcessedEvent (aprovação)

---

**Última atualização**: 2026-07-12  
**Versão**: 1.0.0
