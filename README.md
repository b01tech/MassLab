# Projeto: MassLab

## Visão Geral

O **MassLab** é um sistema de gestão dedicado a laboratórios de calibração de balanças.
Seu escopo abrange o controle dos padrões de calibração, a gestão de ordens de serviço, a realização e registro de ensaios técnicos e a emissão de certificados de calibração em conformidade com a norma ISO 17025.
O sistema atenderá inicialmente um laboratório único, operando em ambiente on-premise, com possibilidade de crescimento futuro.

---

## Stack

- **Frontend**: Angular 21
- **Backend**: .NET 10
- **Banco de dados**: PostgreSQL

---

## Documentação

A documentação do projeto se encontra na pasta `docs/`.

- [README em Português](docs/README.pt-BR.md)
- [Diagrama de Arquitetura](diagrams/architecture.png)
- [Licença](LICENSE.md)
- Documentação da API via Swagger: `localhost:5000/docs`

## Arquitetura

### Arquitetura do Backend

A arquitetura Monolito Modular com Clean Architecture no backend

| **Decisão**                | **Justificativa**                                                                                            |
| :------------------------- | ------------------------------------------------------------------------------------------------------------ |
| **Monolito Modular**       | Deployment on-premise; laboratório único. Monolito reduz complexidade operacional e facilita debug.          |
| **Clean Architecture**     | Separa regras de negócio da infraestrutura. Facilita testes unitários e evolução futura para microsserviços. |
| **PostgreSQL**             | Suporte nativo a auditoria via triggers, transações ACID robustas, excelente suporte em .NET com EF Core.    |
| **ASP.NET Identity + JWT** | Stack nativa do .NET, sem dependência externa, adequada para on-premise sem servidor de identidade separado. |
| **Angular (SPA)**          | Comunicação via REST API com o backend. Lazy loading para módulos.                                           |

A aplicação backend organizada em quatro camadas com dependências sempre apontando para o centro:

- Domain: Entidades, Value Objects, interfaces de repositório, regras de negócio puras. Sem dependência de frameworks.
- Application: Casos de uso, DTOs, validações de entrada, orquestração.
- Infrastructure: Implementações de repositório (EF Core), envio de e-mail, geração de PDF, jobs agendados, configurações de banco.
- Presentation: Controllers REST, middlewares, configuração de autenticação JWT, documentação Swagger.

### Estrutura de pastas

```txt
MassLab/
├── apps/
│ ├── frontend/
│ │ ├── src/
│ │ │ ├── app/
│ │ │ ├── environments/
│ │ ├── public/
│ └── backend/
│ └── MassLab.API/ # Controllers, Program.cs, DI root
│
├── pkgs/
│ ├── Identity/
│ │ ├── src/
│ │ │ ├── MassLab.Identity.Domain/
│ │ │ ├── MassLab.Identity.Application/
│ │ │ └── MassLab.Identity.Infrastructure/
│ │ └── tests/
│ │ ├── MassLab.Identity.Domain.Tests/
│ │ └── MassLab.Identity.Application.Tests/
│ ├── Registry/
│ │ └── (mesma estrutura)
│ ├── Standards/
│ │ └── (mesma estrutura)
│ ├── Order/
│ │ └── (mesma estrutura)
│ ├── Calibration/
│ │ └── (mesma estrutura)
│ └── Audit/
│ └── (mesma estrutura)
│
├── shared/
│ └── MassLab.Shared/
│
├── docs/
│ └──diagrams
├── docker/
├── .editorconfig
├── gitignore
└── MassLab.slnx
```

### Detalhamento dos Módulos

1. **Identity (Autenticação e Autorização)**
   Gerencia o ciclo de vida de usuários e o controle de acesso ao sistema.
   • Cadastro, edição e desativação de usuários (sem exclusão física para garantir rastreabilidade)
   • Roles: Admin, Gerente, Técnico
   • Autenticação com JWT (access token de curta duração + refresh token)
   • Autorização baseada em policies no ASP.NET Core
   • Recomendação: usar ASP.NET Identity como base — solução madura, nativa no .NET, sem dependência de servidores externos, ideal para on-premise
2. **Registry (Cadastros)**
   Centraliza os dados cadastrais utilizados por todos os demais módulos.
   • Emissor do sistema (owner): razão social, CNPJ, logotipo, dados para certificado
   • Clientes: dados completos com histórico
   • Equipamentos dos clientes: modelo, capacidade, número de série, tipo
   • Nota: equipamentos são vinculados ao cliente e referenciados na OS e nos ensaios
3. **Standards (Controle de Padrões)**
   Controla os instrumentos de referência utilizados nos ensaios.
   • Padrões gravimétricos (pesos): classe OIML, valor nominal, faixa
   • Padrões ambientais (termo-baro-higrômetros): modelo, faixas de medição
   • Certificados dos padrões: número, data de emissão, data de vencimento, laboratório rastreador
   • Alerta automático de vencimento: notificação quando certificado estiver a X dias do vencimento (configurável)
   • Padrões com certificado vencido não podem ser selecionados em novos ensaios
4. **Order (Ordem de Serviço)**
   Controla o fluxo de trabalho desde a entrada do equipamento até a liberação do certificado.
   • Criação de OS vinculando cliente e equipamento
   • Fluxo de status: ABERTA → EM_ENSAIO → AGUARDANDO_APROVAÇÃO → APROVADA | REPROVADA
   • Apenas Gerentes aprovam ou reprovam OS
   • OS aprovada libera a emissão do certificado final
   • OS reprovada exige registro de justificativa
5. **Calibration (Ensaios e Certificados)**
   Módulo central e mais complexo do sistema. Registra os ensaios técnicos e gera os certificados ISO 17025.
   • Ensaio de Excentricidade: leituras nas posições padronizadas, cálculo do erro máximo de excentricidade
   • Ensaio de Calibração: repetições de leitura por ponto de carga, cálculo de erro e incerteza
   • Cálculo de Incerteza: implementado com interface extensível por tipo de equipamento (ver Seção 6)
   • Emissão de Certificado: vinculado à OS aprovada, padrões utilizados e usuário responsável
   • Versionamento de certificados: revisões com histórico imutável
   • Aprovação do certificado: fluxo de revisão técnica antes da entrega ao cliente
   • Geração de PDF: layout conforme ISO 17025 via QuestPDF
   • Indicadores: taxa de aprovação, tempo médio de calibração, padrões mais utilizados
6. **Audit (Rastreabilidade e Logs)**
   Módulo transversal que registra toda atividade relevante do sistema.
   • Log de ações: quem fez, o quê, quando, em qual entidade, IP de origem
   • Payload before/after em JSON para operações de escrita em entidades críticas
   • Acesso restrito ao Admin
   • Implementado como middleware no Application Layer — não intrusivo para os demais módulos

### Atores e Matriz de Permissões

| **Funcionalidade**                  | **Admin** | **Gerente** | **Técnico** |
| ----------------------------------- | :-------: | :---------: | :---------: |
| Cadastro de usuários                |     ✓     |      ✓      |      –      |
| Cadastro de clientes e equipamentos |     ✓     |      ✓      |      –      |
| Gerenciar padrões de calibração     |     –     |      ✓      |      –      |
| Criar / editar Ordem de Serviço     |     –     |      ✓      |      –      |
| Visualizar Ordem de Serviço         |     ✓     |      ✓      |      ✓      |
| Aprovar / reprovar OS               |     –     |      ✓      |      –      |
| Realizar ensaios técnicos           |     –     |      –      |      ✓      |
| Gerar certificado                   |     –     |      ✓      |      ✓      |
| Validar / aprovar certificado       |     –     |      ✓      |      –      |
| Visualizar indicadores              |     ✓     |      ✓      |      –      |
| Auditar cálculos                    |     ✓     |      –      |      –      |
| Acessar logs do sistema             |     ✓     |      –      |      –      |

_Observação_: as permissões são implementadas via ASP.NET Core Policies, não apenas por role string

---

## Comandos

### Como executar

1. Clone o repositório: `git clone `
2. Navegue até o diretório do projeto: `cd MassLab/apps/backend/MassLib.Api`
3. Restaure as dependências: `dotnet restore`
4. Compile o projeto: `dotnet build`
5. Execute o projeto: `dotnet run`

### Testes

1. Navegue até o diretório do projeto de testes: `cd MassLib`
2. Execute os testes: `dotnet test`

---
