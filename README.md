# 🚀 Minha API CRUD

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-API-green?style=flat&logo=csharp)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)

> API simples de CRUD para gerenciar recursos de forma rápida e escalável.  
> Documentada com **Swagger/OpenAPI**.

---

## 📑 Sumário
- [Sobre](#-sobre)
- [Tecnologias](#-tecnologias)
- [Arquitetura](#-arquitetura)
- [Endpoints](#-endpoints)
- [Como Rodar](#-como-rodar)
- [Exemplos de Requisições](#-exemplos-de-requisições)
- [Contribuição](#-contribuição)
- [Licença](#-licença)

---

## 📖 Sobre
Essa API implementa operações de **Create, Read, Update e Delete** para [seu recurso principal, ex.: Usuários, Produtos].  
Ela segue boas práticas de REST, validações e versionamento de endpoints.

---

## 🛠 Tecnologias
- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [Swagger / Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [PostgreSQL / SQL Server / MySQL] (escolha o que usar)
- [Docker](https://www.docker.com/) (opcional)

---

## 🏗 Arquitetura

TODO: 
- Endpoint de recomendação
- Endpoint de atualização de nota
- Endpoint de gerar primeiras notas

✅ 1. Pre filter
    Sexual preference
    Height
    Wieght
    Distance..

✅ 2. Elo rating
    Each profile have an elo rating. This rating means the Rizz.

✅ 3. Atualizando o Elo com as interações
    
    **Eventos que podem gerar atualização:**
    ✅ Like → recebe vitória parcial (0,6)
    ✅ Match → vitória “completa” (1)
    ❌ Dislike → derrota (0)
    ✅ Ignorado/skip → derrota leve ou neutra (0.3)

✅ 4. If I use only elo rating algorith the top profiles will always be showed. 
    
    **Better uses**
    70% com base no Elo (exploit)
    20% perfis novos/recém-criados (explore)
    10% aleatórios controlados para não engessar

✅ 5. The game starts with the current user and the recommendation.

O Elo funciona como em jogos: quando há interação entre dois “jogadores” (usuários), o rating muda.

Eventos que podem gerar atualização:

# Alternative
ordem = (elo * 0.6) + (afinidade/interesses em comum * 0.4)
