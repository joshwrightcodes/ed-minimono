FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MiniMono.WorkerService/MiniMono.WorkerService.csproj", "MiniMono.WorkerService/"]
RUN dotnet restore "MiniMono.WorkerService/MiniMono.WorkerService.csproj"
COPY . .
WORKDIR "/src/MiniMono.WorkerService"
RUN dotnet build "MiniMono.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniMono.WorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniMono.WorkerService.dll"]
