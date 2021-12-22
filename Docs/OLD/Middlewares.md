# Middlewares

- Um dos elementos centrais da arquitetura do .Net Core

- Faz parte do pipeline/fluxo de execução de uma aplicação

- Serve como um **filtro** no processamento de requisições HTTP

- Podem ser aplicados a todas as requisições ou apenas a partes específicas da aplicação

- Exemplos:
    - UseHttpsRedirection:
        - Faz o redirecionamento automático de HTTP para HTTPS
    - UseSwagger e UseSwaggerUI:
        - Habilitam a documentação da API
    - UseAuthorization e UseAuthentication:
        - O próprio nome já diz
