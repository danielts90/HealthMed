![Badge Status](https://img.shields.io/badge/status-active-brightgreen)
[![.NET Core Build, Test, and SonarCloud Analysis](https://github.com/gustavopalinka/HealthMed/blob/master/.github/workflows/sonar.yml/badge.svg)](https://github.com/gustavopalinka/HealthMed/actions/workflows/sonar.yml)
[![codecov](https://codecov.io/gh/gustavopalinka/HealthMed/branch/master/graph/badge.svg?token=G4NANCJ6XZ)](https://codecov.io/gh/gustavopalinka/HealthMed)

# HACKATHON 4NETT - Health&Med

## 📌 Descrição
A **Health&Med** é uma startup inovadora no setor de saúde que busca revolucionar a **Telemedicina** no país. Atualmente, os agendamentos de consultas e a realização de consultas online são feitos por meio de sistemas terceiros como Google Agenda e Google Meetings. No entanto, após receber um aporte financeiro, a empresa decidiu desenvolver um **sistema proprietário** para melhorar a qualidade do serviço, garantir a **segurança dos dados** dos pacientes e reduzir custos operacionais.

Este projeto tem como objetivo a criação de um sistema **robusto, escalável e seguro** para gerenciar o agendamento e a realização de consultas médicas, utilizando tecnologias como Kubernetes para escalabilidade, PostgreSQL para persistência confiável de dados e JWT para garantir a segurança da autenticação dos usuários.

O desenvolvimento do MVP está sendo realizado pelos alunos do curso **4NETT**, que foram contratados para análise, arquitetura e implementação da solução.

---
## 🚀 Tecnologias Utilizadas
- **Linguagem:** C#
- **Framework:** .NET 8
- **Banco de Dados:** PostgreSQL
- **Mensageria:** RabbitMQ
- **Docker & Kubernetes:** Contêinerização e orquestração
- **CI/CD:** GitHub Actions
- **ORM:** Entity Framework Core
- **Mensageria Avançada:** MassTransit
- **Testes:** XUnit

---
## 💂️ Arquitetura
O projeto segue a arquitetura de **microserviços**, divididos entre **Auth, Doctor e Patient**:

- **AuthAPI:** Gerencia a autenticação e o cadastro de usuários (Médicos e Pacientes). As principais informações do usuário são incluídas no token **JWT**, que é usado pelas outras APIs para autenticação e autorização. A senha do usuário é armazenada de forma segura usando **PasswordHasher**.

- **DoctorAPI:** Responsável por manter o cadastro de médicos e seus respectivos horários de trabalho. Um médico só pode acessar suas próprias informações e consultas, utilizando o **UserId** presente no token JWT. Pacientes podem acessar essa API para listar médicos e verificar a disponibilidade de horários para agendamento de consultas.

- **PatientAPI:** Mantém o cadastro de pacientes e gerencia o **agendamento e cancelamento** de consultas. Apenas usuários com a role de **Paciente** podem acessar esta API. Quando um paciente agenda uma consulta, a informação é salva no banco de dados de **PatientAPI** e uma mensagem é enviada para a fila do **RabbitMQ**. A **DoctorAPI** consome essa fila e notifica o médico por e-mail sobre a nova consulta.

**Fluxo de Mensageria e Concorrência:**
- Se dois pacientes tentarem marcar a mesma consulta ao mesmo tempo, o **RabbitMQ** resolve o problema de concorrência, garantindo que apenas a primeira mensagem processada seja confirmada.
- Caso uma consulta seja marcada para um horário indisponível, a **DoctorAPI** rejeita a solicitação e envia uma mensagem para cancelar a consulta automaticamente.
- Se um paciente cancela uma consulta, a informação é enviada via RabbitMQ para a **DoctorAPI**, que remove a consulta do banco e notifica o médico.
- Quando um médico confirma ou rejeita uma consulta, a **PatientAPI** é notificada para atualizar o status da consulta e enviar um e-mail ao paciente.

**Projeto Shared:**
- Contém componentes reutilizáveis entre os microserviços, como:
  - Serviço de envio de e-mails
  - Leitura do token JWT para extrair informações do usuário (**IUserContext**)
  - Repositórios genéricos

---
## 📺 Estrutura do Projeto
```
/Projeto
├── Microservices/
│   ├── Auth/
│   ├── Doctor/
│   ├── Patient/
├── tests/
│   ├── Auth.Tests/
│   ├── Doctor.Tests/
│   ├── Patient.Tests/
├── Shared/
├── docker-compose.yml
├── README.md
```

---
## 🛠️ Requisitos Funcionais
✅ **Autenticação do Usuário Médico**: Login com número de CRM e senha.  
✅ **Cadastro/Edição de Horários Disponíveis (Médico)**: Cadastro e edição de horários disponíveis.  
✅ **Aceite ou Recusa de Consultas Médicas (Médico)**: O médico pode aceitar ou recusar consultas.  
✅ **Autenticação do Usuário Paciente**: Login com e-mail ou CPF e senha.  
✅ **Busca por Médicos (Paciente)**: Lista de médicos com filtros por especialidade.  
✅ **Agendamento de Consultas (Paciente)**: Agendamento e visualização de horários disponíveis.  
✅ **Cancelamento de Consulta (Paciente)**: Paciente pode cancelar consulta mediante justificativa.  

---
## 🔧 Requisitos Não Funcionais
✅ **Alta Disponibilidade**: O sistema deve estar disponível 24/7.  
✅ **Escalabilidade**: Suporte para até 20.000 usuários simultâneos.  
✅ **Segurança**: Proteção dos dados dos pacientes seguindo as melhores práticas.  

---
## 🏰 Configuração do Ambiente
1. Clone este repositório:
   ```sh
   git clone https://github.com/danielts90/HealthMed.git
   ```
2. Instale o **.NET 8 SDK**:
   ```sh
   dotnet --version
   ```
3. Configure o PostgreSQL e RabbitMQ no `docker-compose.yml`:
   ```sh
   docker-compose up -d
   ```
4. Execute as migrações do banco de dados:
   ```sh
   dotnet ef database update
   ```
5. Inicie os microserviços:
   ```sh
   dotnet run --project src/AuthAPI
   dotnet run --project src/DoctorAPI
   dotnet run --project src/PatientAPI
   ```

---
## 🔥 Executando os Testes
```sh
 dotnet test
```

