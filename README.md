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

---
## 📂 Estrutura do Projeto
```
/Projeto
├── src/
│   ├── AuthAPI/
│   ├── DoctorAPI/
│   ├── PatientAPI/
├── tests/
│   ├── AuthAPI.Tests/
│   ├── DoctorAPI.Tests/
│   ├── PatientAPI.Tests/
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
## 🏗️ Configuração do Ambiente
1. Clone este repositório:
   ```sh
   git clone https://github.com/danielts90/HealthMed.git
   ```
2. Instale o **.NET 8 SDK** se ainda não tiver:
   ```sh
   dotnet --version
   ```
3. Configure o banco de dados PostgreSQL e o RabbitMQ no `docker-compose.yml`:
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
Para rodar os testes automatizados:
```sh
 dotnet test
```

---
## 📡 Endpoints Principais
- **AuthAPI:** `/api/auth/login`, `/api/auth/register`
- **DoctorAPI:** `/api/doctors`, `/api/doctors/schedule`
- **PatientAPI:** `/api/patients`, `/api/patients/appointments`


