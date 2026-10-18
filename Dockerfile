FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY AiAssistApiAzure.sln ./
COPY AiAssist.Api/AiAssist.Api.csproj AiAssist.Api/
COPY AiAssist.Api.Tests/AiAssist.Api.Tests.csproj AiAssist.Api.Tests/
RUN dotnet restore AiAssistApiAzure.sln
COPY . .
RUN dotnet publish AiAssist.Api/AiAssist.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "AiAssist.Api.dll"]
