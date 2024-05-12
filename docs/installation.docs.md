# ASTRUM - Installation Guide

1) Run Docker containers

   ```bash
   # production mode
   docker-compose up -d 
   ```
   ```bash
   # development mode
   docker-compose -f .\docker-compose.dev.yml up -d 
   ```

2) You need to add migrations for relevant DbContexts if migrations haven't installed yet or DbContexts don't match to
   current models state.
   > **Note**: You don't need to run **database update**, because it will happen automatically <br>

3) if you need to auto migrate database and seed data, you need to add args to command `dotnet run`  
   For example:
   ```bash
   dotnet run --project .\src\Presentation\Astrum.Api\ /init
   ```   
   ```bash
   dotnet run --project .\src\Presentation\Astrum.Api\ /seed
   ```   
   ```bash
   dotnet run --project .\src\Presentation\Astrum.Api\ /migrate
   ```   

4) _**dev:**_ For work you should start 2 microservices:
    * Identity Server
    * Api

5) DbContexts bash commands:
   #### PersistedGrantDbContext
   ```bash
   # Add Migration
   -c PersistedGrantDbContext 
   -o Data/Migrations/IdentityServer/PersistedGrantDb `
   -s ./src/Astrum.Module.Identity.App/ `
   -p ./src/Astrum.Module.Identity.Persistence/ `
   dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration `
   ```
   ```bash
   # Update Database
   -c PersistedGrantDbContext 
   -s ./src/Astrum.Module.Identity.App/ `
   -p ./src/Astrum.Module.Identity.Persistence/ `
   dotnet ef database update `
   ```
   #### ConfigurationDbContext
   ```bash
   # Add Migration
   -c ConfigurationDbContext 
   -o Data/Migrations/IdentityServer/ConfigurationDb `
   -s ./src/Astrum.Module.Identity.App/ `
   -p ./src/Astrum.Module.Identity.Persistence/ `
   dotnet ef migrations add InitialIdentityServerConfigurationDbMigration `
   ```
   ```bash
   # Update Database
   -c ConfigurationDbContext 
   -s ./src/Astrum.Module.Identity.App/ `
   -p ./src/Astrum.Module.Identity.Persistence/ `
   dotnet ef database update `
   ```
   #### IdentityDbContext
   ```bash
   # Add Migration
   -c IdentityDbContext 
   -o Data/Migrations/IdentityDb ` 
   -s ./src/Astrum.Module.Identity.App/ `
   -p ./src/Astrum.Module.Identity.Persistence/ `
   dotnet ef migrations add InitialIdentityDbMigration `
   ```
   ```bash
   # Update Database
   -c IdentityDbContext 
   -s ./src/Astrum.Module.Identity.App/ `
   -p ./src/Astrum.Module.Identity.Persistence/ `
   dotnet ef database update `
     ```
   #### ApplicationDbContext
   ```bash
   # Add Migration
   -c ApplicationDbContext 
   -o Data/Migrations/Application ` 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Infrastructure/Astrum.Infrastructure.Persistence `
   dotnet ef migrations add InitialApplicationDbMigration `
   ```   
   ```bash
   # Update Database
   -c ApplicationDbContext 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Infrastructure/Astrum.Infrastructure.Persistence `
   dotnet ef database update `
   ```
   #### EventStoreDbContext
   ```bash
   # Add Migration
   -c EventStoreDbContext 
   -o Data/Migrations/EventStore ` 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Infrastructure/Astrum.Infrastructure.Persistence `
   dotnet ef migrations add InitialEventStoreDbMigration `
   ```
   ```bash
   # Update Database
   -c EventStoreDbContext 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Infrastructure/Astrum.Infrastructure.Persistence `
   dotnet ef database update `
   ```
   #### AccountDbContext
   ```bash
   # Add Migration
   -c AccountDbContext 
   -o Data/Migrations/Account ` 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Astrum.Module.Account.Persistence `
   dotnet ef migrations add InitialAccountDbMigration `
   ```
   ```bash
   # Update Database
   -c AccountDbContext 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Astrum.Module.Account.Persistence `
   dotnet ef database update `
   ```
   #### OrderingDbContext
   ```bash
   # Add Migration
   -c OrderingDbContext 
   -o Data/Migrations/Ordering ` 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Astrum.Module.Ordering.Persistence `
   dotnet ef migrations add InitialOrderingDbMigration `
   ```
   ```bash
   # Update Database
   -c OrderingDbContext 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Astrum.Module.Ordering.Persistence `
   dotnet ef database update `
   ```
   #### NewsDbContext
   ```bash
   # Add Migration
   -c NewsDbContext 
   -o Data/Migrations/News ` 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Astrum.Module.Ordering.Persistence `
   dotnet ef migrations add InitialNewsDbMigration `
   ```
   ```bash
   # Update Database
   -c NewsDbContext 
   -s ./src/Presentation/Astrum.Api/ `
   -p ./src/Astrum.Module.News.Persistence `
   dotnet ef database update `
   ```
