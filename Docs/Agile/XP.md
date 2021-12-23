# eXtreme Programming - Práticas para o dia a dia no desenvolvimento ágil de software

- Integração Contínua
- Histórias de Usuário
- Ritmo Sustentável
- Teste de Aceitação
- Test Driven Development
- Metáforas de Sistema
- Feedback
- Design Simples
- Pequenas Entregas
- Refatoração
- Time Coeso
- Simplicidade
- Posse Coletiva
- Respeito
- Coragem
- Pair Programming
- Comunicação
- Spikes
- Planning Game

## 1 - Por que projetos falham?
	- Falta de comunicação com o Cliente:
		- Dúvidas / incertezas que travam a implementação.
		- O melhor a fazer é falar diretamente com o Cliente!
		- Buscamos o feedback do Cliente o mais rápido possível.

	- A cereja do bolo:
		- O time deve desenvolver o que foi pedido, nem mais, nem menos.
		- Simplicidade: maximizar o trabalho que não deve ser feito.
		- Desenvolver funcionalidades de alto valor e alto Retorno Sobre Investimento.

	- Testes no final:
		- Testar deve fazer parte do processo.

	- Trabalho empurrado:
		- Prazos malucos, tensão, estresse, caos, código ruim, testes nem pensar.
		- O trabalho deve ser priorizado e puxado pelo time ao longo da iteração.

	- Débito técnico:
		- Código mal feitos, muitas vezes sem testes nem padrões, gambiarras, arrumo depois.
		- Usar as práticas da XP pra previnir isso.

	- Ágil == Melhoria adaptável e contínua

## 2 - Métodos Ágeis
	- Valores do manifesto:
		- Indivíduos e interações mais que processos e ferramentas
		- Software em funcionamento mais que documentação abrangente
		- Colaboração com o cliente mais que negociação de contratos
		- Responder a mudanças mais que seguir um plano

	- Princípios do manifesto:
		- Nossa maior prioridade é satisfazer o cliente por meio da entrega contínua e adiantada de software com valor agregado.
		- Mudanças nos requisitos são bem-vindas no desenvolvimento, mesmo tardiamente. Processos ágeis tiram vantagem das mudanças, visando vantagem competitiva para o cliente.
		- Entregar frequentemente software funcionando, de poucas semanas a poucos meses, com preferência à menor escala de tempo.
		- Pessoas de negócio e desenvolvedores devem trabalhar diariamente em conjunto por todo o projeto.

		- Construa projetos em torno de indivíduos motivados. Dê a eles o ambiente e o suporte necessários e confie neles para fazer o trabalho.
		- O método mais eficiente e eficaz de transmitir informações para (e entre) uma equipe de desenvolvimento é por meio de conversa face a face.
		- Software funcionando é a medida primária de progresso.
		- Os processos ágeis promovem desenvolvimento sustentável. Os patrocinadores, desenvolvedores e usuários devem ser capazes de manter um ritmo constante indefinidamente.

		- Contínua atenção à excelência técnica e bom design aumentam a agilidade.
		- Simplicidade: a arte de maximizar a quantidade de trabalho não realizado é essencial.
		- As melhores arquiteturas, requisitos e designs emergem de equipes auto-organizáveis.
		- Em intervalos regulares, a equipe reflete sobre como tornar-se mais eficaz e, então, refina e ajusta seu comportamento de acordo.

	- Lean Software Development:
		- Eliminar disperdícios
		- Incluir qualidade no processo
		- Criar conhecimento
		- Adiar comprometimentos
		- Entregar rápido
		- Respeitar as pessoas
		- Otimizar o todo

	- Scrum:
		- Backlog, sprint, daily, review...

	- eXtreme Programming:
		- Criação de software de alta qualidade, abandonando todo tipo de overhead de processo que não suporte diretamente a entrega de valor.
		- Orientado explicitamente às pessoas e vai contra o senso comum do gerenciamento de que elas são peças intercambiáveis dentro do processo de desenvolvimento.

## 3 - Valores
	- Essenciais para que seus papéis e práticas funcionem em sinergia de acordo com sua essência ágil.

	- Comunicação:
		- Principal forma de transmitir e trocar informações e conhecimentos.
	- Feedback:
		- É realizado a todo momento (Cliente, Produto, CI, Testes, Pairs, Reviews...).
		- Descobrir e reparar erros o mais rápido possível.
	- Simplicidade:
		- Feijão com arroz, sem cereja do bolo.
	- Coragem:
		- Confiança mútua é essencial.
	- Respeito:
		- Fundamental para uma relação transparente e duradoura.

## 4 - Papéis
	- Dev:
		- Coração do XP.
		- Profissional multidisciplinar, capaz de trabalhar em todas as etapas do desenvolvimento de software:
			- Escrita de histórias de usuário até o deploy em produção.
			- Testes, automatização de processos, aprimoramento do design do sistema.
			- Especificação, prototipação, design, desenvolvimento, testes e atividades ligadas ao DevOps.
	- Cliente:
		- Dono do ouro.
		- Define e prioriza as histórias de usuário, validando o produto desenvolvido por meio de testes de aceitação.
	- Coach:
		- NÃO!
	- Testador:
		- Auxilia o cliente a escolher e escrever testes de aceitação, para, então, automatizá-los.
	- Cleaner:
		- Garí de código?
	- Tracker:
		- Responsável por coletar as métricas de projeto.
		- Pode gerar métricas que mostram o desempenho do time.
	- Gerente:
		- Facilita a comunicação dentro de um time XP e coordena a comunicação com clientes, fornecedores e com o resto da organização. 

## 5 - Time Coeso
	- Indivíduos e interações mais que processos e ferramentas.
	- Um time efetivamente coeso maximiza os valores do XP:
		- Possui maior comunicação por estar integrado.
		- Busca continuamente a simplicidade e constrói confiança para ter-se coragem e gerar feedback por meio do respeito mútuo.
		- A multidisciplinaridade, a auto-organização, a proximidade física e a melhoria contínua são características essenciais para um time efetivamente coeso.
	- Em desenvolvimento de software, existe apenas o aperfeiçoar, e não a perfeição.

## 6 - Cliente Presente
	- Agiliza o trabalho, a comunicação e o feed-back.
	- Faça frequentemente pequenas entregas. Essa é uma boa estratégia para alinhar o software com o negócio. Se o cliente não pode ver o time, que ao menos veja o sistema. ;)

## 7 - Histórias de Usuário
	- Paralelo direto com os valores do manifesto ágil:
		- Indivíduos e interações mais que processos e ferramentas:
			- Elas enfatizam a conversação, não dependendo de processo ou de ferramenta.
		- Software em funcionamento mais que documentação abrangente:
			- Elas vão direto ao ponto com uma forma flexível de documentação para o que há de maior valor para o negócio.
		- Colaboração com o cliente mais que negociação de contratos:
			- Por serem sucintas e necessitarem da conversação do cliente com os desenvolvedores, permitem a negociação e colaboração com o negócio.
		- Responder a mudanças mais que seguir um plano:
			- Elas respondem às mudanças por serem flexíveis em seu ciclo de vida, dando base para as alterações quando necessárias.

## 8 - Testes de Aceitação
	- São mapas da estrada para a iteração, dizendo ao time aonde é preciso ir e quais pontos de referência olhar.

	- O propósito dos testes de aceitação é a comunicação, a transparência e a precisão.
		- Quando os desenvolvedores, os testadores e o cliente concordarem com eles, todos entenderão qual é o plano para o comportamento do sistema. Chegar a esse ponto é responsabilidade de todas as partes. Eles validam como o cliente aceitará as funcionalidades prontas, pois são testes funcionais que guiam o time no desenvolvimento para, então, colocar em produção o que foi decidido que o sistema deve conter.

	- Automatização:
		- Testes devem ser automatizando porque custam dinheiro e tempo.

	- Validando com critérios de aceitação:
		- Dado que: precondição, são os passos para preparar para ação a ser validada.
		- Quando: ação que vai disparar o resultado a ser validado.
		- Então: resultado a ser validado.

## 9 - Liberação frequente de pequenas entregas
	- Cada release deve ser tão pequena quanto possível e com o maior valor de negócio.
		- Entrega de valor adiantado e contínuo.
		- O processo é aprimorado rapidamente por falhar mais cedo (conceito fail fast).
		- Feedback do cliente mais cedo para confirmação ou adaptação dos requisitos.
		- Satisfação dos usuários por ter respostas rápidas às suas necessidades.
		- Maior qualidade, confiança e estabilidade.
		- Reduz a taxa de defeitos por precisar realizar testes completos em ciclos menores.
		- Realiza um design simplificado e suficiente apenas para a entrega em questão.
		- O código é atualizado e integrado com maior frequência.
		- O software não fica ultrapassado.
		- Maior engajamento do time por ver seu trabalho sendo útil e empoderado.
		- Facilita enxergar os diversos desperdícios ocultados nas grandes entregas.
		- Evita a procrastinação de prazos.
	- Foco na qualidade:
		-  Testes automatizados + integração contínua + feedback + aprendizado.

## 10 - O jogo do planejamento
	- Este jogo:
		- Envolve os clientes e os desenvolvedores para planejarem as entregas de uma forma colaborativa.
		- Trata o planejamento como um jogo que contém um objetivo, jogadores, peças e regras.
		- Seu resultado é maximizado por meio da colaboração de todos os jogadores.
		- Envolve a escrita de histórias de usuário, a priorização pelo cliente e a criação das estimativas.

	- Objetivo:
		- Maximizar o valor do software produzido pelo time.
		- Colocar em produção a maior quantidade de histórias de usuário com maior valor ao longo do projeto.

	- Estratégia:
		- O time deve investir o menor esforço para colocar a funcionalidade de maior valor em produção tão rápido quanto possível, em conjunto com estratégias de projeto e programação para reduzir o risco.
		- Dividir para conquistar é a estratégia utilizada ao dividir o software em entregas frequentes, e cada entrega em iterações pequenas.
	
	- As peças:
		- As peças básicas do jogo são histórias de usuário e as tarefas de implementação.
		- As histórias são as peças no planejamento das releases.
		- Já as tarefas, no planejamento das iterações.
	
	- Os jogadores:
		- As pessoas do Desenvolvimento e as pessoas de Negócio.

	- Os movimentos:
		- As regras dos movimentos existem para lembrar a todos de como eles gostariam de agir em um melhor ambiente com confiança mútua e dar uma referência para quando as coisas não estiverem indo bem.

	- Movimentos para as entregas (planejamento de releases):
		- Exploração:
			- Ambos os jogadores descobrem novos itens que o sistema pode fazer, dividindo-se em três movimentos:
				- Escrever uma história de usuário:
					- O Negócio escreve algo que o sistema deverá fazer.
				- Estimar uma história de usuário:
					- O Desenvolvimento estima o tempo ideal de programação para implementar a história.
				- Quebrar uma história de usuário:
					- Caso o Desenvolvimento não consiga estimar a história ou sua estimativa for muito grande, o Negócio identifica sua parte mais importante para que o Desenvolvimento possa estimá-la e desenvolvê-la.
					- Spikes podem ser utilizados para melhorar a estimativa da história.
		- Comprometimento:
			- O Negócio decide o escopo e o tempo para a próxima entrega de acordo com as estimativas, e o Desenvolvimento se compromete com a entrega, dividindo-se em quatro movimentos:
				- Ordenar por valor:
					- O Negócio separa as histórias em três categorias:
						- As essenciais; as menos essenciais, mas de alto valor; e as que seriam agradáveis de se ter.
				- Ordenar por risco:
					- O Desenvolvimento separa as histórias em três categorias:
						- Aquelas que podem ser estimadas com precisão, aquelas que podem ser estimadas razoavelmente bem, e aquelas que não podem ser estimadas de qualquer modo.
				- Ordenar por velocidade:
					- O Desenvolvimento mostra ao Negócio quanto de tempo ideal de programação o time poderá trabalhar em um mês do calendário.
				- Selecionar escopo:
					- O Negócio escolhe as histórias para a entrega (trabalhando por tempo) ou uma data para as histórias a serem desenvolvidas (trabalhando por escopo), utilizando as estimativas realizadas pelo Desenvolvimento.
		- Direcionamento:
			- 


	- Movimentos para as iterações (planejamento de iterações):
		- 
	

## Referências
- 
