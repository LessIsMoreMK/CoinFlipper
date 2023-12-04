FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app


COPY ./Core /app/Core
COPY ./Modules/CoinFlipper.SwissArmy/ /app/Modules/CoinFlipper.SwissArmy/

RUN dotnet restore "./Modules/CoinFlipper.SwissArmy/CoinFlipper.SwissArmy.Api/CoinFlipper.SwissArmy.Api.csproj"
RUN dotnet publish "./Modules/CoinFlipper.SwissArmy/CoinFlipper.SwissArmy.Api/CoinFlipper.SwissArmy.Api.csproj" -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "CoinFlipper.SwissArmy.Api.dll"]