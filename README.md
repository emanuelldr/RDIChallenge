# RDI Software Developer Test #

Dotnet core API for based on credit card data generate a token and validate it.

## Stack 

- Dotnet Core 2.2
- EF Core
- SQLIte
- Swagger

## Requiriments

- .Net Core 2.2 +

## Docs

- Routes and models were documented with Swagger.

## Running Project

- To run the project:
		- With terminal access folder "~RDIChallenge\Services\RDIToken.API"
		
		- Run the following commands

		- dotnet build
		- dotnet run

After runing above commands both (5000 and 5001) ports will be available for service consumption through http and https respectively.
You will also be able to acess Swagger documentation through https://localhost:5001/index.html

## Routes 

		POST https://localhost:5001/api/CreditCards  => Insert Credit Card Data and Generate Token
		GET https://localhost:5001/api/CreditCards/Token/{registrationDate}/{token}/{cvv}/Check => Check if the provided token + registration date is valid
		

## Architecture and Layers
			- Domain -> Responsible for the solution domain.
			- ApplicationCore -> Responsible for business logics.
			- Infrastructures -> Resposible for external data eg. RDI.Token.Infrastructure
			- Services -> Resonsible for web services/apis separation eg. RDI.Token.API