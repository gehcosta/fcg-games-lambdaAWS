# FCG Cloud Games

<div align="center">
  
![AWS](https://img.shields.io/badge/AWS-232F3E?style=for-the-badge&logo=amazon-aws&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Lambda](https://img.shields.io/badge/AWS_Lambda-FF9900?style=for-the-badge&logo=aws-lambda&logoColor=white)

**Projeto de Pós-Graduação FIAP**

</div>

## 📋 Sobre o Projeto

O **FCG Cloud Games** é um projeto desenvolvido para a pós-graduação da FIAP, com este repositório focado em arquitetura serverless na AWS. Este repositório contém uma implementação de serviço de envio de emails utilizando AWS Lambda, API Gateway e Amazon SES (Simple Email Service).

O objetivo principal é demonstrar a aplicação de conceitos de computação em nuvem, arquitetura serverless e integração de serviços AWS em um cenário real de aplicação.

## 🎯 Funcionalidades

- ✉️ **Envio de Emails**: Envio de emails através de requisições HTTP
- 📧 **Suporte a HTML**: Envio de emails em formato texto ou HTML
- 👥 **Múltiplos Destinatários**: Capacidade de enviar para vários destinatários simultaneamente
- 🔒 **Validação de Dados**: Validação robusta dos dados de entrada
- 🚀 **Serverless**: Arquitetura totalmente serverless com AWS Lambda
- 🌐 **API REST**: Exposição via API Gateway para fácil integração

## 🏗️ Arquitetura

<img src="https://raw.githubusercontent.com/PauloBusch/fcg-iac-terraform/main/docs/fcg-architecture-microservices-diagram.drawio.png"></img>

### Componentes

- **API Gateway**: Ponto de entrada HTTP/HTTPS para as requisições
- **AWS Lambda**: Função serverless em .NET 8 que processa as requisições
- **Amazon SES**: Serviço de envio de emails
- **.NET 9**: Runtime da aplicação

## 🛠️ Tecnologias Utilizadas

- **Linguagem**: C# (.NET 9)
- **Cloud Provider**: Amazon Web Services (AWS)
- **Serviços AWS**:
  - AWS Lambda
  - API Gateway
  - Amazon SES (Simple Email Service)
- **Bibliotecas**:
  - Amazon.Lambda.Core
  - Amazon.Lambda.APIGatewayEvents
  - AWSSDK.SimpleEmail
  - Amazon.Lambda.Serialization.SystemTextJson

## 📦 Estrutura do Projeto

```
lambdaAWS/
├── LambdaSendEmail/
│   ├── src/
│   │   └── LambdaSendEmail/
│   │       ├── Function.cs              # Lógica principal da Lambda
│   │       ├── LambdaSendEmail.csproj   # Configuração do projeto
│   │       └── aws-lambda-tools-defaults.json
│   ├── test/
│   │   └── LambdaSendEmail.Tests/       # Testes unitários
│   ├── API_USAGE.md                     # Documentação de uso da API
│   ├── test-event.json                  # Evento de teste (texto)
│   └── test-event-html.json             # Evento de teste (HTML)
└── README.md
```

### Formato da Requisição

```json
{
  "from": "no-reply@fcggames.com",
  "to": ["exemplo@dominio.com"],
  "subject": "Assunto do Email",
  "body": "Conteúdo do email",
  "isHtml": false
}
```

## 📝 Parâmetros da API

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-----------|
| `from` | string | Sim | Email do remetente (deve estar verificado no SES) |
| `to` | array | Sim | Lista de emails destinatários |
| `subject` | string | Sim | Assunto do email |
| `body` | string | Sim | Conteúdo do email (texto ou HTML) |
| `isHtml` | boolean | Não | Define se o body é HTML (padrão: false) |

## 📊 Respostas da API

### Sucesso (200)
```json
{
  "message": "Email sent successfully",
  "messageId": "0100018a1234567890-abcd-efgh-ijkl-0123456789ab-000000",
  "from": "no-reply@fcgfiap.com",
  "to": ["usuario@gmail.com"]
}
```

### Erro (400 - Bad Request)
```json
{
  "error": "From address is required"
}
```

### Erro (500 - Internal Server Error)
```json
{
  "error": "Failed to send email",
  "details": "Mensagem de erro detalhada"
}
```

## 🧪 Testes

O projeto inclui testes unitários para validação da funcionalidade.

```bash
# Executar testes
cd LambdaSendEmail/test/LambdaSendEmail.Tests
dotnet test
```

### Testes Locais

Use os arquivos de teste incluídos no projeto:

```bash
# Testar com email de texto
dotnet lambda invoke-function -f LambdaSendEmail --payload test-event.json

# Testar com email HTML
dotnet lambda invoke-function -f LambdaSendEmail --payload test-event-html.json
```

## 🔒 Segurança

- ✅ Validação de entrada de dados
- ✅ Uso de IAM roles para permissões mínimas necessárias
- ✅ Emails verificados no SES para prevenir spam
- ✅ HTTPS obrigatório via API Gateway

## 🎓 Contexto Acadêmico

Este projeto foi desenvolvido como parte do curso de pós-graduação da FIAP, demonstrando:

- Implementação de arquitetura serverless
- Integração de serviços AWS
- Boas práticas de desenvolvimento em nuvem
- Uso de .NET em ambiente AWS
- Conceitos de microserviços e APIs REST

## 👥 Autores


    Paulo
    Geovanne
    Letícia
    Matheus
    Marcelo
Pós-Graduação FIAP - FCG Cloud Games
