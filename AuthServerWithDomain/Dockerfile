FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt install -y nodejs

# Copy csproj and restore as distinct layers
COPY ["AuthServerWithDomain/AuthServerWithDomain.csproj", "./AuthServerWithDomain/"]
RUN dotnet restore "./AuthServerWithDomain/AuthServerWithDomain.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet build "AuthServerWithDomain/AuthServerWithDomain.csproj" -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish "AuthServerWithDomain/AuthServerWithDomain.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "/app/AuthServerWithDomain.dll"]
