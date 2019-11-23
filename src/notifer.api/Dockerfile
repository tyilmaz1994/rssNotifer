FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
ENV ASPNETCORE_URLS http://+:80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/notifer.api/notifer.api.csproj", "src/notifer.api/"]
COPY ["src/notifier.dal/notifier.dal.csproj", "src/notifier.dal/"]
COPY ["src/notifier.bl/notifier.bl.csproj", "src/notifier.bl/"]
RUN dotnet restore "src/notifer.api/notifer.api.csproj"
COPY . .
WORKDIR "/src/src/notifer.api"
RUN dotnet build "notifer.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "notifer.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "notifer.api.dll"]