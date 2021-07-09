FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
WORKDIR /app

#Exibir a vers�o do .NET
RUN dotnet --version

# Copiar csproj e restaurar dependencias
COPY ["Apis/BancoApi/BancoApi.csproj", "Apis/BancoApi/"]
COPY ["BancoApi.Data/BancoApi.Data.csproj", "Apis/BancoApi/"]
COPY ["BancoApi.Domain/BancoApi.Domain.csproj", "Apis/BancoApi/"]
COPY ["BancoApi.Service/BancoApi.Service.csproj", "Apis/BancoApi/"]

RUN dotnet restore "Apis/BancoApi/BancoApi.csproj"
#  RUN dotnet restore "BancoApi.Domain/BancoApi.Domain.csproj"
#  RUN dotnet restore "BancoApi.Service/BancoApi.Service.csproj"
#  RUN dotnet restore "BancoApi.Data/BancoApi.Data.csproj"

# Build da aplicacao
COPY . ./
 RUN dotnet build "Apis/BancoApi/BancoApi.csproj" -c Release -o out 
# RUN dotnet build "BancoApi.Service/BancoApi.Service.csproj" -c Release --no-restore
# RUN dotnet build "BancoApi.Data/BancoApi.Data.csproj" -c Release --no-restore
RUN dotnet publish -c Release -o out

COPY . ./
 RUN dotnet build "BancoApi.Domain/BancoApi.Domain.csproj" -c Release -o out
# RUN dotnet build "BancoApi.Service/BancoApi.Service.csproj" -c Release --no-restore
# RUN dotnet build "BancoApi.Data/BancoApi.Data.csproj" -c Release --no-restore
RUN dotnet publish -c Release -o out

COPY . ./
 RUN dotnet build "BancoApi.Service/BancoApi.Service.csproj" -c Release -o out
# RUN dotnet build "BancoApi.Service/BancoApi.Service.csproj" -c Release --no-restore
# RUN dotnet build "BancoApi.Data/BancoApi.Data.csproj" -c Release --no-restore
RUN dotnet publish -c Release -o out

COPY . ./
 RUN dotnet build "BancoApi.Data/BancoApi.Data.csproj" -c Release -o out
# RUN dotnet build "BancoApi.Service/BancoApi.Service.csproj" -c Release --no-restore
# RUN dotnet build "BancoApi.Data/BancoApi.Data.csproj" -c Release --no-restore
RUN dotnet publish -c Release -o out


# Build da imagem
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "BancoApi.dll"]

# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /src
# COPY ["Apis/BancoApi/BancoApi.csproj", "Apis/BancoApi/"]
# COPY ["BancoApi.Data/BancoApi.Data.csproj", "Apis/BancoApi/"]
# COPY ["BancoApi.Domain/BancoApi.Domain.csproj", "Apis/BancoApi/"]
# COPY ["BancoApi.Service/BancoApi.Service.csproj", "Apis/BancoApi/"]
# RUN dotnet restore "Apis/BancoApi/BancoApi.csproj"
# COPY . .
# WORKDIR "/src/Apis/BancoApi"
# RUN dotnet build "BancoApi.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "BancoApi.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "BancoApi.dll"]