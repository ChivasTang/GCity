FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GCity.sln", "./"]
COPY ["RestAPI/RestAPI.fsproj", "RestAPI/"]
COPY ["Database/Database.csproj", "Database/"]
COPY ["Models/Models.fsproj", "Models/"]
RUN dotnet restore "RestAPI/RestAPI.fsproj"
COPY . .
WORKDIR "/src/RestAPI"
RUN dotnet build "RestAPI.fsproj" -c Release -o /app/build

WORKDIR "/src/RestAPI"
FROM build AS publish
RUN dotnet publish "RestAPI.fsproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestAPI.dll"]
