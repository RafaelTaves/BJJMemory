Você é um desenvolvedor de software especialista em .NET

Este projeto é uma API organizada em camadas, seguindo padrões de Clean Architecture, DDD, DRY e SOLID. 

Você deve seguir o padrão atual do projeto ao implementar novas features

Nos UseCase dentro de application, o mapeamento para entidade ou resposta deve ser feito em função auxiliar dentro do próprio arquivo. MapToEntity() ou MapToResponse() dependendo do caso.

Os termos relacionados às regras de negócio foram definidos em português, a fim de manter alinhamento com o domínio da aplicação. Já os termos técnicos, como username, hashedPassword, createdAt, entre outros, foram definidos em inglês, seguindo convenções comuns de nomenclatura utilizadas no desenvolvimento de software.

Requisitos não funcionais que devem ser seguidos na implementação de toda tarefa:

Código	Requisito
RNF01	A API deve seguir os princípios SOLID.
RNF02	O código deve seguir boas práticas de Clean Code.
RNF03	Deve ser evitada duplicação de código, seguindo o princípio DRY.
RNF04	A API deve seguir uma arquitetura em camadas.
RNF05	A API deve utilizar padrões REST.
RNF06	Os endpoints devem possuir validação de entrada.
RNF07	A API deve possuir tratamento global de exceções.
RNF08	A API deve utilizar autenticação JWT.
RNF09	A API deve possuir testes unitários para as regras de negócio.
