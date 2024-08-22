# PN.SAPB1.API
 Web API ApsNet - PNs SAP B1

 ![image](https://github.com/user-attachments/assets/81bdcbf3-17ce-48d2-aaea-b1c417fbe145)

 
📋 **Descrição**

Este projeto é uma Web API RestFul que realiza operações de consulta e cadastro de clientes e fornecedores em um bando de dados SQL SERVER por meio do Entity Framework, novos registros são replicados no cadastro de parceiros de negócio do SAP Business One por meio da service layer.

🚀 **Funcionalidades**

- Cadastro de clientes e fornecedores
  - Por meio JSON conforme modelo
  - Por meio do número CNPJ da empresa, será consultado os dados do CNPJ informado por meio da API gratuíta da empresa [ReceitaWs](https://developers.receitaws.com.br/#/operations/queryCNPJFree), se o CNPJ for válido os dados são cadastrados no banco da API
- Consulta de clientes e fornecedores no banco de dados da API

🛠️ **Tecnologias Utilizadas**

- **Linguagem:** C#
- **Framework:** ASP.NET Core 8
- **Banco de Dados:** Microsoft SQL Server
- **ERP:** SAP Business One
- **Ferramentas:** Entity Framework Core, Swagger, RestSharp, Newtonsoft.Json, SAP B1 Service Layer
