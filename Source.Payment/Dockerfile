FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Source.Payment/Source.Payment.csproj", "Source.Payment/"]
RUN dotnet restore "Source.Payment/Source.Payment.csproj"
COPY . .
WORKDIR "/src/Source.Payment"
RUN dotnet build "Source.Payment.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Source.Payment.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Source.Payment.dll"]