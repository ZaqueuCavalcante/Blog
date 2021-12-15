# Identity

- IdentityServer4
- OpenID Connect
- OAuth 2.0

# 1 - Features

- Authentication as a Service:
    - Centraliza a lógica e o fluxo de login para todas as aplicações (web, native, mobile, services).
    - Implementa o padrão OpenID Connect.
- Single Sign-on / Sign-out:
    - Para múltiplas aplicações, de múltiplos tipos.
- Access Control for APIs:
    - Emite access tokens para APIs, para vários tipos de clients, como server to server, web applications, SPAs and native/mobile apps.
- Federation Gateway:
    - Support for external identity providers like Azure Active Directory, Google, Facebook etc.
    - This shields your applications from the details of how to connect to these external providers.
- Focus on Customization:
    - The most important part - many aspects of IdentityServer can be customized to fit your needs.
    - Since IdentityServer is a framework and not a boxed product or a SaaS, you can write code to adapt the system the way it makes sense for your scenarios.

# 2 - The Big Picture

- APIs possuem recursos que precisam ser protegidos via autenticação e autorização.

- Terceirizar essas funções de segurança para um serviço externo às aplicações evita a duplicação de código.

- Autenticação:
    - Processo de validar a identidade de um usuário.
    - O protocolo de autenticação mais moderno é o **OpenID Connect**.

- API Access:
    - **OAuth2** is a protocol that allows applications to request access tokens from a security token service and use them to communicate with APIs.
    - This delegation reduces complexity in both the client applications as well as the APIs since authentication and authorization can be centralized.

- How IdentityServer4 can help:
    - IdentityServer is **middleware** that adds the spec compliant OpenID Connect and OAuth 2.0 endpoints to an arbitrary ASP.NET Core application.

# 3 - Terminology

- IdentityServer:
    - Is an OpenID Connect provider - it implements the OpenID Connect and OAuth 2.0 protocols.
    - A piece of software that issues security tokens to clients.
    - Features: 
        - Protect your resources
        - Authenticate users using a local account store or via an external identity provider
        - Provide session management and single sign-on
        - Manage and authenticate clients
        - Issue identity and access tokens to clients
        - Validate tokens

- User:
    - A user is a human that is using a registered client to access resources.

- Client:
    - A client is a piece of software that requests tokens from IdentityServer.
    - Either for authenticating a user (requesting an identity token) or for accessing a resource (requesting an access token).
    - A client must be first registered with IdentityServer before it can request tokens.
    - Examples for clients are web applications, native mobile or desktop applications, SPAs, server processes etc.

- Resources:
    - Resources are something you want to protect with IdentityServer - either identity data of your users, or APIs.
    - Every resource has a unique name - and clients use this name to specify to which resources they want to get access to.
    - Identity data are information (aka claims) about a user, e.g. name or email address.
    - APIs resources represent functionality a client wants to invoke - typically modelled as Web APIs, but not necessarily.

- Identity Token:
    - An identity token represents the outcome of an authentication process.
    - It contains at a bare minimum an identifier for the user (called the sub aka subject claim) and information about how and when the user authenticated.
    - It can contain additional identity data.

- Access Token:
    - An access token allows access to an API resource.
    - Clients request access tokens and forward them to the API.
    - Access tokens contain information about the client and the user (if present).
    - APIs use that information to authorize access to their data.

## 4 - Overview

- Adding IdentityServer to an ASP.NET Core application
- Configuring IdentityServer
- Issuing tokens for various clients
- Securing web applications and APIs
- Adding support for EntityFramework based configuration
- Adding support for ASP.NET Identity

## 5 - Protecting an API using Client Credentials

- This first quickstart is the most basic scenario for protecting APIs using IdentityServer.
- In this quickstart you define an API and a Client with which to access it.
- The client will request an **access token** from the Identity Server using its client ID and secret and then use the token to gain access to the API.



## References:
    - https://identityserver4.readthedocs.io/en/latest/index.html
    - https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0
    - http://www.macoratti.net/17/11/aspncore_ident1.htm
    - https://www.tutorialspoint.com/asp.net_core/asp.net_core_identity_configuration.htm
    - https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-6.0

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

# Curso de IdentityServer4 - Heber Pereira

- OpenID Connect + OAuth2

## 1 - Segurança no ASP>NET Core

- ClaimsPrincipal e ClaimsIdentity
- Herdam das respectivas interfaces:
    - IPrincipal:
        - Identidade do usuário
        - Papéis ao qual pertence
    - IIdentity:
        - Usuário propriamente dito

- Autenticação baseada em cookies:
    - Na autenticação, serializa o ClaimsPrincipal dentro de um cookie encriptado
    - No resquest, é feita a validação do cookie
    - O ClaimsPrincipal é recriado e associado ao User do HttpContext

- Autorização:
    - Definir o que um usuário pode ou não acessar
    - Atributo [Authorize]
    - Roles:
        - Papéis do usuário, associados ao ClaimsPrincipal
    - Claims são melhores!

- ASP.NET Core Identity:
    - Sistema de AUTORIZAÇÃO
    - Permite realização de login
    - Senha configurável
    - Two-Factor Authentication
    - Alto acomplamento com o Entity Framework (DbContext)

- Claims:
    - Conjunto de informações associadas à um usuário
    - Nome, sobrenome, endereço, email...
    - Roles

- Policy:
    - Expressa validação de autorização em código
    - Rico, reutilizável e facilmente testável
    - IAuthorizationRequirement, AddAuthorization, AddPolicy

- Resource:
    - Muitas vezes, a autorização depende do RECURSO a ser acessado
    - Exemplos:
        - Pedidos da região sudeste
        - Pagamentos maiores que R$ 500,00
        - Documentos criados pelo próprio usuário
    - IAuthorizationService:
        - Authorize:
            - ClaimsPrincipal user
            - object resource
            - List<IAuthorizationRequirement> requirements OU string policyName

- Token:
    - Obtido via id + senha
    - Contém as informações de acesso do usuário
    - Permite que um recurso seja acessado, sem a necessidade de id + senha

## 2 - OAuth2 and OpenID Connect

- OAuth2:
    - 



## 3 - 




- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

# Curso de IdentityServer4 - desenvolvedor.io

- OpenID Connect + OAuth2
- Single Sign-On
- Federation Gateway
- Single Sign-Out

- Json Web Token:
    - Audience
    - Scopes
    - Claims

- OAuth 2.0:
    - Resource Owner
    - Client
    - Resource Server
    - Authorization Server




