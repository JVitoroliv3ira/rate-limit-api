# rate-limit-api

**rate-limit-api** é uma API focada em demonstrar, de forma prática, a aplicação de técnicas de **rate limiting** e **controle de uso baseado em API Keys**. A funcionalidade da API é propositalmente simples, permitindo que o foco esteja na arquitetura do backend, segurança e escalabilidade.

---

## 🎯 Proposta

Este projeto tem como principal objetivo servir como base técnica para estudar e implementar:

- Emissão, autenticação e revogação de **API Keys**
- Limitação de requisições por **chave de API**
- Planos com diferentes quotas e limites (ex: Free, Pro)
- Suporte a múltiplas chaves por usuário
- Observabilidade: logging, métricas e rastreio de consumo

---

## 💡 Objetivo Técnico

Construir uma API minimalista, mas com um backend robusto, utilizando:

- **Autenticação por API Key** com controle por plano
- **Rate limiting via middleware**, usando Redis como armazenamento de contadores
- **Mecanismos de throttling configuráveis** por plano e endpoint
- **Infraestrutura preparada para múltiplos clientes** e ambientes

---

## 🔧 Tecnologias

- [.NET 8](https://dotnet.microsoft.com/)
- PostgreSQL (persistência de usuários e chaves)
- Redis (rate limiting e cache)
- FluentMigrator (versionamento do banco de dados)
- xUnit, Moq, FluentAssertions (testes automatizados)

---

## 🛠️ Em desenvolvimento

- [x] Criação e autenticação de usuários
- [ ] Geração de API Keys por usuário
- [ ] Middleware de autenticação por API Key
- [ ] Middleware de rate limit por plano e endpoint
- [ ] Registro de uso e monitoramento
- [ ] Painel ou endpoint de estatísticas

---

## 📄 Licença

Este projeto está licenciado sob os termos da **GPL v3.0**.  
Veja o arquivo [`LICENSE`](./LICENSE) para mais detalhes.

---

## 📬 Contribuindo

Sinta-se à vontade para abrir issues ou pull requests. Toda contribuição é bem-vinda.
