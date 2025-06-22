# ClienteAPI
Este é um projeto de API RESTful desenvolvido em C# com .NET Core, projetado para gerenciar operações básicas de CRUD (criar, ler, atualizar e excluir) para entidades de clientes. A API permite cadastrar, consultar, listar, pesquisar e atualizar informações de clientes, incluindo dados de contato e endereço.

Principais funcionalidades:

Cadastro de clientes com validação de CEP via API externa.
Consulta detalhada de CEPs, armazenando os dados no banco de dados para futuras consultas.
Pesquisa de clientes por CEP e listagem completa de todos os registros.
Atualização segura dos dados do cliente com validações específicas.
Exclusão de clientes com feedback claro sobre a operação realizada.
Tecnologias utilizadas:

C# 12 e .NET 8 como base da aplicação.
Entity Framework Core para mapeamento objeto-relacional e persistência de dados.
AutoMapper para facilitar o transporte seguro de dados entre camadas.
SQL Server como banco de dados relacional.
HttpClient para consumo de APIs externas.
A API segue boas práticas de design RESTful, com validações robustas e mensagens personalizadas para garantir uma experiência clara e consistente ao usuário.
