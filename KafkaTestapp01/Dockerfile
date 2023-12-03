FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["KafkaTestapp01/KafkaTestapp01.csproj", "KafkaTestapp01/"]
RUN dotnet restore "KafkaTestapp01/KafkaTestapp01.csproj"
COPY . .
WORKDIR "/src/KafkaTestapp01"
RUN dotnet build "KafkaTestapp01.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KafkaTestapp01.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KafkaTestapp01.dll"]
