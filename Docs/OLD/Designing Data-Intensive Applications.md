# Designing Data-Intensive Applications

# Part 1 - Foundations of Data Systems

## 1 - Reliable, Scalable and Maintainable Applications

**Uma aplicação é Data-Intensive se:**
- A quantidade / complexidade de dados que ela gera ou usa **aumenta** rapidamente.
- Os dados que ela gera ou usa **mudam** rapidamente.

**Principais componentes de uma aplicação:**
- Bancos de Dados:
	- São a fonte da verdade.
- Cache:
	- Para leituras rápidas.
- Full-text Index:
	- Para procurar / filtrar dados rapidamente.
- Message Queues:
	- Para gerenciar mensagens entre processos.
- Stream Processig:
	- Para processar dados em tempo real.
- Batch Processing:
	- Para processar grandes quantidades de dados.
- Application Code:
	- Conecta todos os componentes acima.

**Reliability:**
- Tolerança à falhas.
- Política de acesso.
- Testes automatizados.
- Ambiente de staging.
- Roll-back rápido.

**Scalability:**
- Lida com altos volumes de tráfego.
- Grande quantidade de usuários simultâneos.
- Response time vs throughput.
- SLO/A > 95%.
- Escalabilidade horizontal.

**Maintainability:**
- Pessoas rapidamente se tornam produtivas no sistema.
- O sistema é configurável e testável.
- Fácil de entender e mudar.

> Não existe um modelo para criar uma aplicação com essas características, ela deve ser **customizada** para atender cada contexto.

## 2 - Data Models and Query Languages

**Abstrair coisas do mundo real:**
- **Modelar** usando Estruturas de Dados.
- **Persistir** usando um Modelo de Dados (JSON, Tabelas, Documentos, Grafos...).
- A **Representação** desses dados vai permitir que eles sejam buscados, armazenados e processados...

## 3 - Storage and Retrieval

**Como armazenar dados de forma a otimizar uma busca depois?**
- Usando **índices**!
- Trade-off: otimizar a consulta, degradar a escrita.
- Hash indexes / Dictionary / Key-Value / Hash Table... 

## 4 - Encoding and Evolution







# Part II - Distributed Data


## 5 - 


## 6 - 


## 7 - 


## 8 - 


## 9 - 





# Part III - Derived Data

## 10 - 


## 11 - 

## Referências
- [Playlist](https://www.youtube.com/watch?v=PdtlXdse7pw&list=PL4KdJM8LzAMecwInbBK5GJ3Anz-ts75RQ)
