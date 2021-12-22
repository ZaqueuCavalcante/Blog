# ASP.NET Core In Action

# Part 1 - Getting started with ASP.NET Core

## 1 - 

## 2 - 

## 3 - 

## 4 - 

## 5 - 

## 6 - 

## 7 - 

## 8 - 

## 9 - 

# Part 2 - Building complete applications

Dependency Injection, Configurations, EF Core, Identity and Deploy. 

## 10 - Dependency Injection




## 15 - Authorization: Securing your application

- Policies:
    - Uma **Policy** pode ter vários **Requirements**, cada um com vários **Handlers**.
    - O ASP.NET Core já traz uma implementação default na forma de **Roles** e **Claims**.
    - Para passar por uma Policy, todos os Requirements (apenas 1 Handler já serve) devem ser satisfeitos.
    - Dá pra montar qualquer Policy com isso:
        - [Req01 (Hand01 OR Hand02)] AND [Req02 (Hand01 OR Hand02 OR Hand03)] AND [Req03 (Hand01)]

## 16 - Publishing and deploying your application


# Part 3 - Extending your applications

## 18 - Improving your application’s security


## 19 - Building custom components

- Custom middlewares:
    - Manipular o request dentro do pipeline.
    - Map, Use and Run extension methods.


## 22 - Building background tasks and services

- Windows Services and Linux Daemons

- Quartz.NET



