# PostgreSQL Query Optimization

The Ultimate Guide to Building Efficient Queries

## 1 - Why Optimize?

- Além de otimizar as queries, devemos otimizar o design do banco e da aplicação.

- SQL é uma linguagem declarativa. "Eu escrevo o resultado, não como ele vai ser obtido."

- OLTP (Online Transaction Processing) to short queries vs OLAP (Online Analytical Processing) to short and long queries.

- O design do banco impacta diretamente na sua performace.

## 2 - Theory: Yes, We Need It!

- Como queries são processadas pelo banco de dados?
    - SQL -> Compilado e transformado em uma Expressão (Logical Plan).
    - Logical Plan -> Otimizado e convertido em um Execution Plan.
    - Execution Plan -> Interpretado e retorna os resultados.

- Operações relacionais:
    - Filter: condições / restrições sobre os dados.
    - Project: remoção de dados duplicados (DISTINCT).
    - Product: produto cartesiano entre tabelas.

## 3 - Even More Theory: Algorithms

- Algoritmos para acessar dados:
    - O banco de dados possui objetos (tabelas, linhas, colunas, índices...), que são armazenados em blocos.
    - Linhas são armazenadas usando uma Heap, sendo inseridas em qualquer bloco, sem uma ordem específica.
    - Full Scan:
        - Lê a tabela toda, aplicando um filtro.
    - Index-Based Table Access:
        - Estruturas de dados adicionais/redundantes, que otimizam as consultas.
        - Permitem que eu saiba o que tem na tabela, sem efetivamente lê-la. Isso otimiza os filtros.
        - Index Scan:
            - 
        - Bitmap Heap Scan:
            - 
    - Índices:
        - O que é um índice?

## 4 - 



## 5 - 

## 8 - Optimizing Data Modification

- Data Definition Language (ALTER TABLE, CREATE MATERIALIZED VIEW, DROP INDEX...)
- Data Manipulation Language (INSERT, UPDATE, DELETE)

- Two Ways to Optimize Data Modification





## 9 - Design Matters

- Tipos de banco de dados:
    - Entity-Attribute-Value Model
    - Key-Value Model
    - Hierarchical Model

- Relacionamentos e constraints ajudam.

## 10 - Application Development and Performance

- 'Please wait...' pode matar uma aplicação.

- Impedance Mismatch:
    - Aplicação e banco de dados usam paradigmas diferentes.
    - Essa tradução entre mundos degrada a performace.
    - ORMs são fundamentais aqui. (Dapper, EF...).

- PostgreSQL features:
    - Is an object-relational database.
    - Allows the creation of **custom types**.
    - PostgreSQL functions can return sets, including sets of records.

## 11 - Functions

- Diferenças entre funções no PostgreSQL vs outras linguagens:
    - Built-in Functions:
        - Associadas a cada tipo nativamente suportado pelo PostgreSQL, escritas em C.
    - User-Defined Functions:
        - Escritas em SQL vs em C (ou C++) vs Procedural language (PL/pgSQL, PL/Python...).

## 12 - Dynamic SQL

- Um jeito de armazenar SQL em variáveis (texto) e executar depois.

- Utilizado em funções e procedures para uma maior flexibilidade e eficiência.

- Foreign Data Wrappers (FDWs)

## 13 - Avoiding the Pitfalls of Object-Relational Mapping

- App-Model <-> Transfer-Model <-> Database-Model

- 




## Referências
- 
