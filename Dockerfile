# Runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5268

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Clothing_Store_BE.sln .
COPY ClothingStore.Application/*.csproj ClothingStore.Application/
COPY ClothingStore.BE.API/*.csproj ClothingStore.BE.API/
COPY ClothingStore.Domain/*.csproj ClothingStore.Domain/
COPY ClothingStore.Infrastructure/*.csproj ClothingStore.Infrastructure/
COPY Shared/*.csproj Shared/

RUN dotnet restore

COPY . .
WORKDIR /src/ClothingStore.BE.API
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClothingStore.BE.API.dll"]
