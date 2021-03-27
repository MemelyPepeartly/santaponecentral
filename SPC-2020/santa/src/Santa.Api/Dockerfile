FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Santa.Api/Santa.Api.csproj", "Santa.Api/"]
RUN dotnet restore "Santa.Api/Santa.Api.csproj"
COPY . .
WORKDIR "/src/Santa.Api"
RUN dotnet build "Santa.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Santa.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Santa.Api.dll"]