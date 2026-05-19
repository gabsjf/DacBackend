# DAC Backend - Análise Educacional MS

Backend desenvolvido em ASP.NET Core para o projeto DAC, com foco na análise histórica de dados educacionais da rede pública de ensino de Mato Grosso do Sul.

O sistema realiza a importação de arquivos CSV do Portal de Dados MS, persiste os dados em SQL Server e disponibiliza endpoints para consumo pelo dashboard frontend.

---

## Objetivo do Projeto

Este backend tem como objetivo apoiar a análise da evolução da educação pública em Mato Grosso do Sul entre os anos de 2018 e 2026.

A aplicação permite:

- Importar dados educacionais a partir de arquivos CSV;
- Armazenar municípios, escolas e indicadores educacionais;
- Consultar dados filtrados por ano, município e escola;
- Gerar informações agregadas para gráficos e cards analíticos;
- Servir dados para um dashboard frontend em Angular.

---

## Tecnologias Utilizadas

- C#
- ASP.NET Core Web API
- SQL Server
- Microsoft.Data.SqlClient
- Scalar para documentação/teste da API
- Arquitetura em camadas
- CSV como fonte de dados

---

## Estrutura do Projeto

```txt
DAC_CSharp
│
├── Application
│   ├── Dtos
│   ├── Interfaces
│   └── Services
│
├── Domain
│   └── Entities
│
├── Infrastructure
│   ├── Database
│   └── Repositories
│
└── Presentation
    └── Controllers
