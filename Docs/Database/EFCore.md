# Entity Framework Core in Action

## Part 1 - Getting Started

## 1 - Introduction to EF Core

Tabelas <=> EF Core <=> Objetos

## 2 - Querying the database

- Relacionamentos:
    - One-to-one, one-to-many, and many-to-many
    - Owned Type class
    - Table splitting
    - Table per hierarchy
    - Table per type

- Migrations:
    - Microsoft.EntityFrameworkCore.Tools
    - Quando são feitas mudanças no código, o banco pode ser facilmente atualizado via migrations

- Buscando dados relacionados:
    - Eager loading
    - Explicit loading
    - Select loading
    - Lazy loading

## 3 - Changing the database content

- State:
    - Propriedade de entidades
    - Como acessar: context.Entry(someEntityInstance).State
    - Diz ao EF Core o que fazer com a entidade quando o SaveChanges() é chamado
    - Possíveis valores:
        - Added: a entidade deve ser criada no banco
        - Unchanged: a entidade não mudou, ignorar
        - Modified: a entidade existe no banco e foi mudada, fazer o update
        - Deleted: deletar entidade no banco
        - Detached: a entidade não tá sendo trackeada pelo EF

- Soft delete:
    - HasQueryFilter()

## 4 - Using EF Core in business logic

- Os três tipos de lógica de negócio:
    - Validação
    - Simples
    - Complexa

## 5 - Using EF Core in ASP.NET Core Web Apps

- 






## Part 2 - EF in Depth

## 7 - Configuring nonrelational properties

- Configurações:
    - Convenções: default do EF para mapeamentos, nomes, tipos, conversões...
    - Data Annotations: anotações logo acima das propriedades
    - Fluent API: permite configurar praticamente qualquer coisa

- Global Query Filters: (CHAPTER 6.1.6)
    - Multitenant applications
    - Soft-delete feature

## 8 - Configuring relationships

- Convenções:
- Data Annotations:
- Fluent API:
- 5 outros jeitos:

## 9 - Handling database migrations

- Como mudar a estrutura do banco usando migrations:
    - Criação:
    - Aplicação:



## 10 - Configuring advanced features and handling concurrency conflicts

- Funções:
    - Dá pra criar funções no banco e executar elas via EF Core

- Colunas com valores default e computados:
    - Não dá pra usar dados de outras tabelas

- Conflitos de concorrência:
    - Concurrency token
    - Timestamp

## 11 - Going deeper into the DbContext

- DbContext:
    - ChangeTracker
    - ContextId
    - Database:
        - Transaction control
        - Database creation/migration
        - Raw SQL commands
    - Model

- Change Tracking:
    - Cada entidade possui um State, que define o que deve ser feito como ela quando o SaveChanges() é chamado
    - 











## Part 3 - Using EF Core in Real-World Apps

## 12 - Using entity events to solve business problems

- Tipos de eventos

- Eventos de domínio

- Eventos de integração

- Event Runner

## 13 - 

## 15 - Unit testing EF Core Apps

- Como simular um banco de dados para testes unitários?
    - Banco em memória:
        - SQLite
    - Banco real:
        - Postgresql
    - Mockar repositórios:
        - Retornar objetos prontos
    - Mockar o DbContext:
        - Não vale a pena

- Log


## Referências
- [Learn EF Core](https://www.learnentityframeworkcore.com/)
