[![Build Status](https://dev.azure.com/chadjiantoniou/CleanArchitecture/_apis/build/status/thecodewrapper.CH.CleanArchitecture?branchName=master)](https://dev.azure.com/chadjiantoniou/CleanArchitecture/_build/latest?definitionId=2&branchName=master)

# Astrum Project

Corporate portal

## Technologies used

### General

- [ASP.NET Core 6](https://learn.microsoft.com/en-us/aspnet/core/getting-started)
- [Entity Framework Core 6](https://docs.microsoft.com/en-us/aspnet/core/data/entity-framework-6?view=aspnetcore-3.1&viewFallbackFrom=aspnetcore-2.1.)
- [MediatR](https://medium.com/dotnet-hub/use-mediatr-in-asp-net-or-asp-net-core-cqrs-and-mediator-in-dotnet-how-to-use-mediatr-cqrs-aspnetcore-5076e2f2880c)
- [Fluent Validation](https://docs.fluentvalidation.net/en/latest/index.html)
- [MassTransit](https://masstransit-project.com/)
- [AutoMapper](https://docs.automapper.org/en/stable/index.html)
- [GuardClauses](https://github.com/ardalis/GuardClauses)
- [Serilog](https://serilog.net/)

### Tests

- [xUnit](https://xunit.net/)
- [Fluent Assertions](https://fluentassertions.com/introduction)
- [FakeItEasy](https://fakeiteasy.github.io/)

### Infrastructure

- [Docker](https://www.docker.com/)
- [docker-compose](https://docs.docker.com/compose/gettingstarted/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [PostgreSQL](https://www.postgresql.org/docs/)

## Sources
- 

## Features

The features of this particular solution are summarized briefly below, in no particular order:

- Localization for multiple language support
- Event sourcing using Entity Framework Core and SQL Server as persistent storage, including snapshots and retroactive
  events
- EventStore repository and DataEntity generic repository. Persistence can be swapped between them, fine-grained to
  individual entities
- Persistent application configurations with optional encryption
- Data operation auditing built-in (for entities which are not using the EventStore)
- Local user management with ASP.NET Core Identity
- Clean separation of data entities and domain objects and mapping between them for persistence/retrieval using
  AutoMapper
- ASP.NET Core MVC with Razor Components used for presentation
- CQRS using handler abstractions to support MassTransit or MediatR with very little change
- Service bus abstractions to support message-broker solutions like MassTransit or MediatR (default implementation uses
  MassTransit’s mediator)
- Unforcefully promoting Domain-Driven Design with aggregates, entities and domain event abstractions.
- Lightweight authorization framework using ASP.NET Core AuthorizationHandler
- Docker containerization support for SQL Server and Web app

### Some other goodies:

- Password generator implementation based on ASP.NET Core Identity password configuration
- Razor Class Library containing ready-made Blazor components for commonly used features such as CRUD buttons, toast
  functionality, modal components, Blazor Select2, DataTables integration and page loader
- Common library with various type extensions, result wrapper objects, paged result data structures, date format
  converter and more.
