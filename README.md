# ConexaoSolidaria

Sistema desenvolvido para o **Hackathon FIAP** utilizando arquitetura baseada em microserviços e comunicação assíncrona para gerenciamento de campanhas e processamento de doações.

---

# Tecnologias Utilizadas

- C# / .NET 8
- ASP.NET Core Web API
- Worker Service
- SQL Server
- RabbitMQ
- MassTransit
- Docker
- Docker Compose
- Kubernetes

---

# Arquitetura

O projeto é composto por dois microserviços:

## ConexaoSolidaria.API

Responsável por:

- Autenticação
- Gerenciamento de campanhas
- Cadastro de doações
- Publicação de eventos no RabbitMQ

## ConexaoSolidaria.Worker

Responsável por:

- Consumo das mensagens do RabbitMQ
- Processamento das doações
- Atualização automática do valor arrecadado das campanhas
- Atualização do status das doações

---

# Instruções

## Clonar o repositório

```bash
git clone https://github.com/ChristyanBrenner/ConexaoSolidaria.git

cd ConexaoSolidaria
```

---

## Executar utilizando Docker Compose

```bash
docker compose up --build
```

Após a inicialização estarão disponíveis:

| Serviço | URL |
|---------|-----|
| API | http://localhost:8080 |
| RabbitMQ Management | http://localhost:15672 |
| SQL Server | localhost:1433 |

---

## Executar utilizando Kubernetes

Aplicar todos os manifestos:

```bash
kubectl apply -f k8s/
```

Verificar os recursos:

```bash
kubectl get all
```

---

# Estrutura do Projeto

```text
ConexaoSolidaria
│
├── ConexaoSolidaria.API
│   ├── API
│   ├── Domain
│   ├── Infrastructure
│   ├── Services
│   └── Dockerfile
│
├── ConexaoSolidaria.Worker
│   ├── Worker
│   ├── Domain
│   ├── Infrastructure
│   ├── Services
│   └── Dockerfile
│
├── docker-compose.yml
│
├── k8s
│   ├── api-deployment.yaml
│   ├── worker-deployment.yaml
│   ├── rabbitmq-deployment.yaml
│   ├── sqlserver-deployment.yaml
│   └── services
│
└── README.md
```

---

# Fluxo de Comunicação

```text
                                  Cliente
                                     │
                                     │ HTTP
                                     ▼
                    +----------------------------------+
                    |     ConexaoSolidaria.API         |
                    |----------------------------------|
                    | • Autenticação                   |
                    | • Campanhas                      |
                    | • Doações                        |
                    +----------------+-----------------+
                                     │
                                     │ Publica Evento
                                     │ (MassTransit)
                                     ▼
                          +------------------------+
                          |       RabbitMQ         |
                          |        Queue           |
                          +-----------+------------+
                                      │
                                      │ Consome Evento
                                      ▼
                  +---------------------------------------+
                  |      ConexaoSolidaria.Worker          |
                  |---------------------------------------|
                  | • Processa Doação                    |
                  | • Atualiza Campanha                  |
                  | • Atualiza Status                    |
                  +------------------+--------------------+
                                     │
                                     ▼
                          +----------------------+
                          |      SQL Server      |
                          +----------------------+
```

---

# Fluxo de Processamento

1. O cliente realiza uma doação através da API.

2. A API registra a doação no banco de dados.

3. A API publica um evento no RabbitMQ utilizando MassTransit.

4. O Worker consome a mensagem publicada.

5. O Worker processa a doação.

6. O Worker atualiza automaticamente:

- Status da doação;
- Valor arrecadado da campanha;
- Data de processamento.

---

# Componentes

- ASP.NET Core Web API
- Worker Service
- RabbitMQ
- MassTransit
- SQL Server
- Docker
- Docker Compose
- Kubernetes

---

# Arquitetura da Solução

```text
                  +----------------------+
                  |       Cliente        |
                  +----------+-----------+
                             |
                             | HTTP
                             ▼
               +-------------------------------+
               |     ConexaoSolidaria.API      |
               +---------------+---------------+
                               |
                               | MassTransit
                               ▼
                     +---------------------+
                     |      RabbitMQ       |
                     +----------+----------+
                                |
                                ▼
               +-------------------------------+
               |   ConexaoSolidaria.Worker     |
               +---------------+---------------+
                               |
                               ▼
                     +---------------------+
                     |     SQL Server      |
                     +---------------------+
```

---

# Funcionalidades

- Cadastro de campanhas
- Consulta de campanhas
- Registro de doações
- Processamento assíncrono
- Atualização automática do valor arrecadado
- Atualização do status das doações
- Comunicação desacoplada utilizando eventos
- Orquestração via Docker Compose
- Deploy utilizando Kubernetes

---

# Organização da Solução

- API responsável pelas operações síncronas.
- Worker responsável pelo processamento assíncrono.
- RabbitMQ responsável pelo desacoplamento entre os serviços.
- SQL Server utilizado para persistência dos dados.
- MassTransit utilizado como barramento de mensagens.

---

# Autor

Projeto desenvolvido para o **Hackathon FIAP**.
