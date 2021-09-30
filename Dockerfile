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

# Build da aplicacao
COPY . ./
 RUN dotnet build "Apis/BancoApi/BancoApi.csproj" -c Release -o out 
RUN dotnet publish -c Release -o out

COPY . ./
 RUN dotnet build "BancoApi.Domain/BancoApi.Domain.csproj" -c Release -o out
RUN dotnet publish -c Release -o out

COPY . ./
 RUN dotnet build "BancoApi.Service/BancoApi.Service.csproj" -c Release -o out
RUN dotnet publish -c Release -o out

COPY . ./
 RUN dotnet build "BancoApi.Data/BancoApi.Data.csproj" -c Release -o out
RUN dotnet publish -c Release -o out


# Build da imagem
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "BancoApi.dll"]
