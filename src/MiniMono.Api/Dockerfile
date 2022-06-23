FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MiniMono.Api/MiniMono.Api.csproj", "MiniMono.Api/"]
RUN dotnet restore "MiniMono.Api/MiniMono.Api.csproj"
COPY . .
WORKDIR "/src/MiniMono.Api"
RUN dotnet build "MiniMono.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniMono.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniMono.Api.dll"]
