FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["RecipeManagement/src/RecipeManagement/RecipeManagement.csproj", "./RecipeManagement/src/RecipeManagement/"]
COPY ["SharedKernel/SharedKernel.csproj", "./SharedKernel/"]
RUN dotnet restore "./RecipeManagement/src/RecipeManagement/RecipeManagement.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet build "RecipeManagement/src/RecipeManagement/RecipeManagement.csproj" -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish "RecipeManagement/src/RecipeManagement/RecipeManagement.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "/app/RecipeManagement.dll"]
