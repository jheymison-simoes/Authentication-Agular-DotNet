﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Authentication.Api/Authentication.Api.csproj", "Authentication.Api/"]
COPY ["src/Authentication.Business/Authentication.Business.csproj", "Authentication.Business/"]
COPY ["src/Authentication.Domain/Authentication.Domain.csproj", "Authentication.Domain/"]
COPY ["src/Authentication.Data/Authentication.Data.csproj", "Authentication.Data/"]
RUN dotnet restore "Authentication.Api/Authentication.Api.csproj"
COPY . .
WORKDIR "src/Authentication.Api"
RUN dotnet build "Authentication.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authentication.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.Api.dll"]
