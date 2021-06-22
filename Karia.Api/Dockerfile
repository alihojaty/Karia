FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Karia.Api/Karia.Api.csproj", "Karia.Api/"]
RUN dotnet restore "Karia.Api/Karia.Api.csproj"
COPY . .
WORKDIR "/src/Karia.Api"
RUN dotnet build "Karia.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Karia.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Karia.Api.dll"]
