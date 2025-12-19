ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION AS build

WORKDIR /src

COPY ["./Ecommerce.ProductApiSolution/ProductApi.Presentation/ProductApi.Presentation.csproj","./Ecommerce.ProductApiSolution/ProductApi.Presentation/"]
COPY ["./E-Commerce.SharedLibrarySolution/eCommerce.ShardLibrary/eCommerce.SharedLibrary.csproj", "E-Commerce.SharedLibrarySolution/eCommerce.ShardLibrary/"]

RUN dotnet restore "./Ecommerce.ProductApiSolution/ProductApi.Presentation/ProductApi.Presentation.csproj"

COPY ./Ecommerce.ProductApiSolution ./Ecommerce.ProductApiSolution
COPY ./E-Commerce.SharedLibrarySolution/ ./E-Commerce.SharedLibrarySolution

WORKDIR /src/Ecommerce.ProductApiSolution/ProductApi.Presentation/

RUN dotnet publish "ProductApi.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION AS run
WORKDIR /app

COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y krb5-user libgssapi-krb5-2

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT [ "dotnet","ProductApi.Presentation.dll" ]
