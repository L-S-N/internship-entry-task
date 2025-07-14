# Используем официальный .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.sln .
COPY TicTacToe.Core/*.csproj ./TicTacToe.Core/
COPY TicTacToe.Api/*.csproj ./TicTacToe.Api/
RUN dotnet restore

# Копируем остальные файлы и собираем проект
COPY TicTacToe.Core/ ./TicTacToe.Core/
COPY TicTacToe.Api/ ./TicTacToe.Api/
WORKDIR /app/TicTacToe.Api
RUN dotnet publish -c Release -o out

# Финальный контейнер
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/TicTacToe.Api/out .

# Приложение будет слушать порт 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TicTacToe.Api.dll"]