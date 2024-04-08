ARG ASPNET_IMAGE_TAG=7.0-bullseye-slim-amd64
ARG DOTNET_SDK=7.0

# Stage 2 - Build and publish dotnet application
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK} AS publish
COPY . .
RUN dotnet restore DfE.IdentifiersApi
RUN dotnet build DfE.IdentifiersApi -c Release
RUN dotnet publish DfE.IdentifiersApi -c Release -o /app --no-build

COPY ./docker/docker-entrypoint.sh /app/docker-entrypoint.sh

# Stage 3 - Put into Docker container that will actually be run
FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG} AS final
COPY --from=publish /app /app
WORKDIR /app
RUN chmod +x ./docker-entrypoint.sh
EXPOSE 80/tcp
