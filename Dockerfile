# Multi-stage build for QuestBoard API
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy solution and restore dependencies
COPY QuestBoard.sln ./
COPY QuestBoard.Api/QuestBoard.Api.csproj QuestBoard.Api/
COPY QuestBoard.Application/QuestBoard.Application.csproj QuestBoard.Application/
COPY QuestBoard.Domain/QuestBoard.Domain.csproj QuestBoard.Domain/
COPY QuestBoard.Infrastructure/QuestBoard.Infrastructure.csproj QuestBoard.Infrastructure/
COPY QuestBoard.Tests/QuestBoard.Tests.csproj QuestBoard.Tests/
RUN dotnet restore

# Copy the remainder of the source and publish the API
COPY . .
RUN dotnet publish QuestBoard.Api/QuestBoard.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080

ENTRYPOINT ["dotnet", "QuestBoard.Api.dll"]
