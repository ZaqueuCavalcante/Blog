# The Twelve-Factor App

Pirâmide de Infraestrutura.

## 1 - Codebase

Todo código deve estar versionado no GIT.

Codebase <=> Aplicação em Produção

## 2 - Dependencies

Declare e isole as dependências explicitamente.

## 3 - Config

Armazene as configurações no ambiente, não no código.

## 4 - Backing services

Trate os serviços de apoio como recursos plugáveis e substituíveis, acessíveis via URL.

Banco de dados, email service, Amazon S3, Redis, logs...

## 5 - Build, release, run

Separe estritamente os builds e execute em estágios.

## 6 - Processes

Execute a aplicação como um ou mais processos que não armazenam estado.

## 7 - Port binding

Exporte serviços por ligação de porta.

## 8 - Concurrency

Escalonamento horizontal.

## 9 - Descartabilidade

Maximizar a robustez com inicialização e desligamento rápido.

## 10 - Dev/prod semelhantes

Mantenha o desenvolvimento, teste, produção o mais semelhante possível.

## 11 - Logs

Trate logs como fluxo de eventos.

## 12 - Processos de Admin

Executar tarefas de administração/gerenciamento como processos pontuais.

## Referências
- [The Twelve-Factor App](https://12factor.net/pt_br/)
