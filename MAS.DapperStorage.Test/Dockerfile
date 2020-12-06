#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MAS.DapperStorage.Test/MAS.DapperStorageTest.csproj", "MAS.DapperStorage.Test/"]
COPY ["MAS.DapperStorage.Test.Models/1. MAS.DapperStorageTest.Models.csproj", "MAS.DapperStorage.Test.Models/"]
COPY ["MAS.DapperStrorageTest.Infrastructure/2. MAS.DapperStorageTest.Infrastructure.csproj", "MAS.DapperStrorageTest.Infrastructure/"]
COPY ["MAS.DappertStorageTest.Cqrs/3. MAS.DappertStorageTest.Cqrs.csproj", "MAS.DappertStorageTest.Cqrs/"]
RUN dotnet restore "MAS.DapperStorage.Test/MAS.DapperStorageTest.csproj"
COPY . .
WORKDIR "/src/MAS.DapperStorage.Test"
RUN dotnet build "MAS.DapperStorageTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MAS.DapperStorageTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MAS.DapperStorageTest.dll"]
