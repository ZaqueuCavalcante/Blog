# Heroku

- Integração com GIT
- Dinos e load balancer
- Add-ons:
    - Ferramentas de log, banco de dados, monitoramento...
- Integração e deploy contínuos
- Rollback trivial

- Aplicação = Code + Framework + Dependencies -> Build anb Run

- Run on Heroku = informar que parte da aplicação é runnable -> Procfiles

- Procfile:
    - Especifica os comandos que são executados pelo app (dynos) no startup
    - Nele podem ser declarados vários 'process types':
        - Web Server
        - Workers
        - Tasks que rodam antes do deploy


- Dynos:
    - Tipo containers




