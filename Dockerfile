# Используем официальный образ .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 5100

# Сборка проекта
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файл проекта (без вложенной папки)
COPY BackTelega.csproj ./
RUN dotnet restore "./BackTelega.csproj"
RUN mkdir -p /app/Uploads

# Копируем все файлы проекта
COPY . ./
WORKDIR "/src"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Публикация проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный контейнер
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackTelega.dll"]
