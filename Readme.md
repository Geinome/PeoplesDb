# Peoples DB
Peoples DB is a simple CRUD application to manage a persons name within a database.
The Architecture is loosely based on the microservice pattern and should be container ready.
The structure is as followed:
- PeoplesDb.Client: A simple razor client implementation which serves the CRUD interface to talk with the api.
- PeoplesDb.Shared: Shared library of code - Likely this would be split up into different domains if more code is added.
- PeoplesDb.Api.People: Api layer which manages a persons data and stores it in a common database.
- PeoplesDb.Api.People.Tests: Unit testing project for the api layer

# Building / Debug

There are two ways to run and debug the application; One using `tye`, and the other using `dotnet`.
First clone the repository and change directory:
```powershell
git clone https://github.com/Geinome/PeoplesDb.git peoplesdb
cd .\peoplesdb
```
## Dotnet

To build using dotnet run the following commands:
```powershell
dotnet build 
```
Both the api and client should now be built.
To run tests execute:
```powershell
dotnet test
```

And finally to run the application you can do the following:
```powershell
#Starts the api in a new window
Start-Process dotnet "run", "--project", ".\PeoplesDb.Api.People\PeoplesDb.Api.People.csproj"
#Starts the client server in a new window
Start-Process dotnet "run", "--project", ".\PeoplesDb.Client\PeoplesDb.Client.csproj"
```

## Tye

[Tye](https://github.com/dotnet/tye) is an experimental tool by Microsoft which provides a way to build, debug and deploy containerized applications/services. Tye makes it much easier to build and develop microservice architectures. 

Firstly to use Tye you need to install the tool as a global .Net tool:
```powershell
dotnet tool install -g Microsoft.Tye --version "0.4.0-alpha.20371.1"
```
>For more information see: [Tye getting started](https://github.com/dotnet/tye/blob/master/docs/getting_started.md)

To build using tye you can simply do:
```powershell
tye build
```

> `tye build` builds the projects and prepares applications container. [See More](https://github.com/dotnet/tye/blob/master/docs/reference/commandline/tye-build.md) 

Alternatively the following can be used:
```powershell
tye run
```

This will build and run the solution based off the projects defined in the `.\tye.yaml`.
It will take care of wiring up the service and client and any other services like databases.

Tye also provides a dashboard which can be used to manage the individual applications it starts up.
> Navigate to: `http://localhost:8000`

# Running

Regardless of how the application is built and run, it should always be available at `http://localhost:5000` or `https://localhost:5001`.
The Api will either be available on `https://localhost:6001` or on a dynamic port assigned by Tye - If used.