FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app


COPY ./Core /app/Core
COPY ./Modules/CoinFlipper.Tracer/ /app/Modules/CoinFlipper.Tracer/

RUN dotnet restore "./Modules/CoinFlipper.Tracer/CoinFlipper.Tracer.Api/CoinFlipper.Tracer.Api.csproj"
RUN dotnet publish "./Modules/CoinFlipper.Tracer/CoinFlipper.Tracer.Api/CoinFlipper.Tracer.Api.csproj" -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "CoinFlipper.Tracer.Api.dll"]