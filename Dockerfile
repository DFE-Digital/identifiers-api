FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /build

ENV DEBIAN_FRONTEND=noninteractive

COPY . .

# Build project...


ARG ASPNET_IMAGE_TAG
FROM mcr.microsoft.com/dotnet/aspnet:7.0.17-bullseye-slim AS final

COPY --from=build /app /app

WORKDIR /app
COPY ./script/web-docker-entrypoint.sh ./docker-entrypoint.sh
RUN chmod +x ./docker-entrypoint.sh
EXPOSE 80/tcp
