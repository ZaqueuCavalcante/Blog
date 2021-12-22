# Architecting Modern Web Applications

## 1 - Características de Aplicações Web Modernas

- Otimizadas para a Cloud e escaláveis.
- Cross Platform.
- Injeção de Dependências, Modularidade e Baixo Acoplamento.
- Testes Automatizados.
- Desenvolvimento e Deploy simples. CI/CD.

## 2 - Single Page Applications

SPAs possibilitam a criação de interfaces ricas em funcionalidades.

## 3 - Princípios Arquiteturais

A arquitetura deve permitir que o sistema seja mantido e evoluído.

### Separação de Responsabilidades
- O software deve ser separado de acordo com os tipos de ações que cada parte realiza (comportamento).
- Exemplo: separar regras de negócio, banco de dados, infraestrutura, front-end...

### Encapsulamento
- Esconder as implementações atrás de **interfaces bem definidas**.
- Possibilita a alteração das implementações sem quebrar quem depende das interfaces.
- Leva ao baixo acoplamento e a modularidade do sistema.

### Inversão de Dependência
- Dependa de abstrações, não de implementações.
- Possibilita o baixo acoplamento entre as partes do sistema.
- Facilita a construção de testes.

### Dependências Explícitas
- Deixar explícito quais são as dependências necessárias para performar determinada funcionalidade.
- Definir nos construtores todas essas dependências, sendo honesto com os clientes.

### Responsabilidade Única
- Uma classe deve possuir apenas uma razão para mudar.

### Don't Repeat Yourself (DRY)
- Reaproveitar código, evitar duplicidade.

### Persistence Ignorance
- Evitar que a tecnologia de armazenamento de dados influencie em objetos de negócio.

### Contextos Delimitados (DDD)
- Modelar o domínio utilizando sua linguagem ubíqua.
- Separar os conceitos dentro de contextos, para facilitar o entendimento e a implementação.

## 4 - Arquiteturas

### Monólitos
- Aplicações que possuem uma única unidade de deploy.

### Clean Architecture
- Utiliza o conceito de **inversão de dependências** para isolar as regras de negócio no centro da aplicação.
- Core:
    - Entities, Services, Interfaces, DTOs...
- Infrastructure:
    - EF Core DbContext, Migrations, Configurations...
    - Repository Pattern
    - FileLogger or SmtpNotifier
- User Interface:
    - Controllers, ViewsModels, Startup...

### Docker
- Torna possível deployar a aplicação como um container.
- Aumenta a escalabilidade.

## 5 - Front-End

HTML (Conteúdo)

CSS (Estilo)

JavaScript (Comportamento)

Vue (Junta todos os 3)

## 6 - MVC

Requests and Responses:
    - Middlewares

Security:
    - ASP.NET Core Identity
    - Suporte para cadastro de usuários, login, recuperação de senha...
    
Authentication:
    - JWT bearer tokens
    - Identity Server

Authorization:
    - Restrição de acesso
    - Claims

Deployment:
    - Reverse proxy servers
    - Kestrel
    - Docker Containers

## 7 - Banco de Dados

### EF Core:
    - DbContext:
        - Classe que abstrai tabelas do banco as representando como coleções.
        - Essas coleções são DbSets.
    - Configuration:
        - Configurar como o EF Core deve ser comportar.
        - Usar o método Startup.ConfigureServices()
    - Features:
        - Change Tracking
        - Eager Loading
        - Explicit Loading
        - Lazy Loading
        - Entity Support
        - Connection Resiliency
        - Migrations

### Dapper:
    - Raw SQL
    - Performace

### Caching:
    - Response Caching Middleware

## 8 - Testes

### Unitários:
    - Testam uma única parte do software.
    - Não possuem dependências externas.
    - Rodam extremamente rápido.

### De Integração:
    - Testam a integração entre duas ou mais partes do sistema.
    - Possuem dependências externas e de infraestrutura.

### Funcionais:
    - São escritos a partir da perspectiva do usuário.
    - Verificam a corretude do sistema baseado em seus requerimentos.

### Organização dos Testes:
    - Separar por tipo (Unitários, Integração, Funcionais)
    - Organizar por Projeto e por Namespace

## 9 - Azure

CI/CD Pipelines

## Referências
- [Architect Modern Web Applications with ASP.NET Core and Azure](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)
