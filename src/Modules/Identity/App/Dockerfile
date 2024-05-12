FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Source/Astrum.Module.IdentityServer.App/Astrum.Module.IdentityServer.App.csproj", "Astrum.Module.IdentityServer.App/"]
RUN dotnet restore "Source/Astrum.Module.IdentityServer.App/Astrum.Module.IdentityServer.App.csproj"
COPY . .
WORKDIR "/src/Astrum.Module.IdentityServer.App"
RUN dotnet build "Astrum.Module.IdentityServer.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Astrum.Module.IdentityServer.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Astrum.Module.IdentityServer.App.dll"]
