ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION AS build

WORKDIR /src

COPY ["./Ecommerce.ApiGatewaySolution/ApiGateway.Presentation/ApiGateway.Presentation.csproj","./Ecommerce.ApiGatewaySolution/ApiGateway.Presentation/"]

RUN dotnet restore "./Ecommerce.ApiGatewaySolution/ApiGateway.Presentation/ApiGateway.Presentation.csproj"

COPY ./Ecommerce.ApiGatewaySolution ./Ecommerce.ApiGatewaySolution/
COPY ./E-Commerce.SharedLibrarySolution ./E-Commerce.SharedLibrarySolution/

WORKDIR /src/Ecommerce.ApiGatewaySolution/ApiGateway.Presentation

RUN dotnet publish "./ApiGateway.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION AS run
WORKDIR /app

COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y krb5-user libgssapi-krb5-2

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT [ "dotnet","ApiGateway.Presentation.dll" ]
