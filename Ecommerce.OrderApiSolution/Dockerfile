ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION AS build

WORKDIR /src

COPY ["./Ecommerce.OrderApiSolution/OrderApi.Presentation/OrderApi.Presentation.csproj","Ecommerce.OrderApiSolution/OrderApi.Presentation/"]
COPY ["./E-Commerce.SharedLibrarySolution/eCommerce.ShardLibrary/eCommerce.SharedLibrary.csproj", "E-Commerce.SharedLibrarySolution/eCommerce.ShardLibrary/"]

RUN dotnet restore "./Ecommerce.OrderApiSolution/OrderApi.Presentation/OrderApi.Presentation.csproj"

COPY ./Ecommerce.OrderApiSolution ./Ecommerce.OrderApiSolution
COPY ./E-Commerce.SharedLibrarySolution ./E-Commerce.SharedLibrarySolution

WORKDIR /src/Ecommerce.OrderApiSolution/OrderApi.Presentation/

RUN dotnet publish "./OrderApi.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION AS run
WORKDIR /app

COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y krb5-user libgssapi-krb5-2

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT [ "dotnet","OrderApi.Presentation.dll" ]
