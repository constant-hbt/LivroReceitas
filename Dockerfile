FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar todos os arquivos para o contêiner
COPY src/ .

# Mudar para o diretório da API
WORKDIR Backend/RecipeBook.API

# Restaurar as dependências
RUN dotnet restore 

# Publicar a aplicação em modo Release
RUN dotnet publish -c Release -o /app/out

# Etapa 2: Build da imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar os arquivos publicados da etapa anterior
COPY --from=build-env /app/out .

# Expor a porta 8080
EXPOSE 8080

# Configurar o EntryPoint para iniciar a aplicação
ENTRYPOINT ["dotnet", "RecipeBook.API.dll"]