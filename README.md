# Blog

Desenvolvi uma API para um Blog simples, sempre prezando pela integridade conceitual.

Usei como inspiração o [Medium](medium.com) e o [dev.to](dev.to) para desenvolver os casos de uso.

A API foi feita com DotNet Core e C#. <br>
Na parte de banco de dados, usei o EF Core, o Dapper e o Postgres. <br>
Na de autenticação e autorização, usei o Identity Core com JWT. <br>
Na de testes, usei o NUnit com Fluent Assertions. <br>
E para documentação, o Swagger. <br>

Este projeto ainda não está finalizado, ainda pretendo desenvolver os seguintes pontos:

- [ ] Adicionar pipelines de CI/CD usando o GitHub Actions CI
- [ ] Dockerizar o projeto
- [ ] Fazer o front-end
- [ ] Refatorar código das controllers usando services
- [ ] Adicionar mais testes

## Modelagem de dados

A seguir, uma breve descrição das entidades e de seus relacionamentos.

- **Bloggers**:
	- Quem escreve os Posts, são os produtores de conteúdo.

- **Readers**:
	- Quem lê os Posts, são os consumidores de conteúdo.

- **Users**:
	- Quando um **Blogger** ou um **Reader** se cadastra no Blog, um **User** é atrelado à ele.
	- Possui informações de login, como e-mail e senha.

- **Posts**:
	- Cada **Post** possui um único autor, isto é, um **Blogger**.
	- Ele também está dentro de uma única **Category**.
	- E pode ou não estar atrelado à várias **Tags**.
	- Dado um **Post**, tanto **Readers** quanto **Bloggers** podem fazer **Comments** sobre seu conteúdo.
	- Em um **Post**, o autor tem a opção de fixar um **Comment** no topo, o destacando dos demais.

- **Categories**:
	- Um tópico/assunto principal abordado em um **Post**.
	- Por exemplo: EF Core, JWT, Dapper, Docker, Postgres...

- **Tags**:
	- Parece com uma **Category**, sendo um tópico/assunto secundário abordado no **Post**.
	- Como dito antes, um **Post** pode estar atrelado à várias **Tags**.

- **Comments**:
	- Cada **Comment** está vinculado à um **Post**.
	- Um **Comment** pode ter vários **Replies**.
	- Um **Comment** pode ter vários **Likes**.

- **Replies**:
	- É tipo um **Comment**, só que feito sobre um **Comment**.

- **Likes**:
	- Dispensa apresentações, pode ser dado por um **User** em um **Comment**.

- **Networks**:
	- Representa um link para uma rede social de um **User**.
	- Exemplo: LinkedIn, GitHub, Twitter, YouTube, Instagram...

Usando o **EF Core**, é extremamente fácil configurar esses relacionamentos via **FluentApi**.

- **Um-Para-Um**:
	- Um Blogger está vinculado à um User
	- Um Reader está vinculado à um User

- **Um-Para-Muitos**:
	- Uma Category pode conter vários Posts
	- Um Blogger pode escrever vários Posts
	- Um Post pode ter muitos Comments
	- Um Comment pode ter vários Replies
	- Um Comment pode ter vários Likes
	- Um User pode escrever vários Comments
	- Um User pode escrever vários Replies
	- Um User pode dar vários Likes
	- Um User pode ter várias Networks

- **Muitos-Para-Muitos**:
	- Um Post pode ter várias Tags, e uma mesma Tag pode estar vinculada à vários Posts
