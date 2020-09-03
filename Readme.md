# Peoples DB
Peoples DB implements a microservice architecture whereby the client, database and apis are all seperated.
The structure is as followed:
- PeoplesDb.Api.People
- PeoplesDb.Client
- PeoplesDb.Shared

# Todos:
- Add tests for Api layer up to front end
	- https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps
	- XUnit for testing repository etc.
	- https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-3.1
	- https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1
- Follow SOLID princibles
- Communicate with sql server instance spun up either by Tye or by dev server
- Implement front end client with Vue.js