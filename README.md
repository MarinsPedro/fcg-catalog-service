# FIAP Cloud Games - Catalog Service

Microsserviço responsável pelo **catálogo de jogos** do FIAP Cloud Games: cadastro, consulta, alteração, ativação e desativação de jogos.

## Finalidade e papel no sistema

- Guarda os dados dos jogos (título, descrição, categoria, preço base e se está ativo).
- É **consultado pelo Orders**, que lê o jogo e o preço base ao criar um pedido de compra.
- Não usa mensageria: o Catalog não publica nem consome eventos. Quem inicia o fluxo de compra e coordena pagamento e biblioteca é o Orders (saga), em vez do Catalog.
- Tem banco próprio (PostgreSQL, banco `fcg_catalog`).

## Rotas

Base: `/v1/games`. Todas exigem token de serviço (veja "Autenticação").

| Método e rota | Scope | Finalidade |
|---|---|---|
| `GET /v1/games` | `catalog.games.read` | Lista jogos. Filtros: `title`, `maxPrice`, paginação |
| `GET /v1/games/{id}` | `catalog.games.read` | Consulta um jogo |
| `POST /v1/games` | `catalog.games.create` | Cria um jogo (nasce ativo) |
| `PUT /v1/games/{id}` | `catalog.games.write` | Altera título, descrição, categoria e preço base |
| `POST /v1/games/activate/{id}` | `catalog.games.write` | Ativa o jogo |
| `DELETE /v1/games/deactivate/{id}` | `catalog.games.write` | Desativa o jogo (exclusão lógica, nada é apagado) |

Corpo de `POST /v1/games` e de `PUT /v1/games/{id}`:

```json
{
  "title": "Jogo A",
  "description": "texto opcional",
  "category": "FPS",
  "basePrice": 100.00
}
```

Resposta (`GameResponse`): `id`, `title`, `description`, `category`, `basePrice`, `isActive`.

O Swagger fica disponível em `/swagger` no ambiente `Development`.

## Autenticação

As rotas exigem um **token JWT de serviço**, emitido pelo Identity (`POST /oauth2/token`, `grant_type=client_credentials`), com:

- audience `catalog-api`;
- os scopes da tabela acima.

## Variáveis de ambiente

No ambiente do projeto os valores vêm do orquestrador (`config/*-catalog-api.env` e Secrets); o `appsettings.json` do serviço fica vazio. `__` nas variáveis equivale a `:` na configuração do .NET.

### API (`catalog-api`)

| Variável | Obrigatória | Onde fica no cluster | Exemplo | Para que serve |
|---|---|---|---|---|
| `ConnectionStrings__Database` | sim | Secret (`fcg-domain-dev-secrets`, chave `catalog-connection-string`) | `Host=postgres;Database=fcg_catalog;Username=...;Password=...` | Conexão com o PostgreSQL |
| `Authentication__Authority` | sim | ConfigMap | `http://identity-api:8080` | Endereço do Identity que valida os tokens |
| `Authentication__Audience` | sim | ConfigMap | `catalog-api` | Audience aceita nos tokens |
| `Authentication__Issuer` | sim | ConfigMap | `fiap-cloud-games-identity` | Emissor esperado nos tokens |
| `Authentication__AllowedTokenUses__0` | sim | ConfigMap | `service` | Só aceita tokens de serviço |
| `Authentication__RequireHttpsMetadata` | não | ConfigMap | `false` | `false` em ambiente local, sem HTTPS |
| `Authentication__ClockSkew` | não | ConfigMap | `00:00:30` | Tolerância de relógio na validação do token |
| `ASPNETCORE_ENVIRONMENT` | não | ConfigMap | `Development` | Ambiente (habilita o Swagger) |
| `ASPNETCORE_URLS` | não | ConfigMap | `http://+:8080` | Porta HTTP dentro do contêiner |
| `AllowedHosts` | não | ConfigMap | `*` | Hosts aceitos |
| `Logging__LogLevel__Default` | não | ConfigMap | `Information` | Nível de log |

### Migrations (`catalog-migrations`)

| Variável | Obrigatória | Onde fica | Para que serve |
|---|---|---|---|
| `ConnectionStrings__Database` (ou `CATALOG_MIGRATIONS_CONNECTION`) | sim | Secret | Conexão usada para aplicar as migrations |
| `DOTNET_ENVIRONMENT` | não | ConfigMap | Ambiente do executável (`Development`) |

## Estrutura do código

```text
src/
  FiapCloudGames.Catalog.Api             # composition root e pipeline HTTP
  FiapCloudGames.Catalog.Presentation    # controllers, requests, responses, políticas
  FiapCloudGames.Catalog.Application     # casos de uso
  FiapCloudGames.Catalog.Domain          # entidade Game e regras
  FiapCloudGames.Catalog.Infrastructure  # EF Core, repositórios, autenticação
  FiapCloudGames.Catalog.Migrations      # migrations e executável que aplica no banco

tests/
  FiapCloudGames.Catalog.UnitTests
  FiapCloudGames.Catalog.ArchitectureTests
  FiapCloudGames.Catalog.Database.IntegrationTests
```

A API nunca aplica migrations sozinha: elas rodam no executável `Catalog.Migrations`.

## Como executar

### Pela orquestração (recomendado)

O Catalog sobe junto com os outros serviços pelo repositório do orquestrador (`fgc-orchestrator-service`, clonado como pasta `fcg-orchestration`):

```powershell
cd ..\fcg-orchestration
.\FCG-COMPOSE.cmd   # Catalog em http://127.0.0.1:5084/swagger
.\FCG-K8S.cmd       # Catalog em http://127.0.0.1:5184/swagger
```

O orquestrador cria o banco `fcg_catalog`, roda as migrations e sobe a API.

### Testes

```powershell
dotnet test FiapCloudGames.Catalog.slnx
```

## Docker

O `Dockerfile` é multi-stage e tem dois alvos finais:

- `final`: a API;
- `migrations`: o executável das migrations.

## Kubernetes

A pasta `k8s/` tem:

- `api.yaml`: Service e Deployment da API (porta 8080);
- `migrations-job.yaml`: Job que aplica as migrations antes da API.

Os ConfigMaps e os Secrets são criados pelo instalador do orquestrador, a partir de `config/*.env`.
