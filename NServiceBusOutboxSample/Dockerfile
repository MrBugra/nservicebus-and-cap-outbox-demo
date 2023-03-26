FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NServiceBusOutboxSample/NServiceBusOutboxSample.csproj", "NServiceBusOutboxSample/"]
RUN dotnet restore "NServiceBusOutboxSample/NServiceBusOutboxSample.csproj"
COPY . .
WORKDIR "/src/NServiceBusOutboxSample"
RUN dotnet build "NServiceBusOutboxSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NServiceBusOutboxSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NServiceBusOutboxSample.dll"]
