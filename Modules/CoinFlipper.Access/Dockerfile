FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app


COPY ./Core /app/Core
COPY ./Modules/CoinFlipper.Access/ /app/Modules/CoinFlipper.Access/

RUN dotnet restore "./Modules/CoinFlipper.Access/CoinFlipper.Access.Api/CoinFlipper.Access.Api.csproj"
RUN dotnet publish "./Modules/CoinFlipper.Access/CoinFlipper.Access.Api/CoinFlipper.Access.Api.csproj" -c Release -o /app/out

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out ./

RUN apt-get update
RUN apt-get --assume-yes install curl

ENTRYPOINT ["dotnet", "CoinFlipper.Access.Api.dll"]