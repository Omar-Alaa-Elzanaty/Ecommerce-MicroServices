ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION AS build

WORKDIR /src

COPY ["./Ecommerce.AuthenticationApiSolution/AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj","./Ecommerce.AuthenticationApiSolution/AuthenticationApi.Presentation/"]

RUN dotnet restore "./Ecommerce.AuthenticationApiSolution/AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj"

COPY ./Ecommerce.AuthenticationApiSolution/ ./Ecommerce.AuthenticationApiSolution/
COPY ./E-Commerce.SharedLibrarySolution/ ./E-Commerce.SharedLibrarySolution/

WORKDIR /src/Ecommerce.AuthenticationApiSolution/AuthenticationApi.Presentation

RUN dotnet publish "AuthenticationApi.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION AS run
WORKDIR /app

COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y krb5-user libgssapi-krb5-2

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT [ "dotnet","AuthenticationApi.Presentation.dll" ]
