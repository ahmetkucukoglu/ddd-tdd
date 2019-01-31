FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["DDDSample.API/DDDSample.API.csproj", "DDDSample.API/"]
RUN dotnet restore "DDDSample.API/DDDSample.API.csproj"
COPY . .
WORKDIR "/src/DDDSample.API"
RUN dotnet build "DDDSample.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DDDSample.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DDDSample.API.dll"]