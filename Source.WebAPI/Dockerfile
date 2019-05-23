FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Source.WebAPI/Source.WebAPI.csproj", "Source.WebAPI/"]
RUN dotnet restore "Source.WebAPI/Source.WebAPI.csproj"
COPY . .
WORKDIR "/src/Source.WebAPI"
RUN dotnet build "Source.WebAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Source.WebAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Source.WebAPI.dll"]