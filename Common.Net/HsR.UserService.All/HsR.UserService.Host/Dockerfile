FROM mcr.microsoft.com/dotnet/aspnet:9.0

ARG VERSION=unknown
ARG COMMIT_SHA=unknown
ARG BUILD_TIMESTAMP=unknown

LABEL version=$VERSION
LABEL commit=$COMMIT_SHA
LABEL build_time=$BUILD_TIMESTAMP

ENV APP_VERSION=$VERSION
ENV COMMIT_SHA=$COMMIT_SHA
ENV BUILD_TIMESTAMP=$BUILD_TIMESTAMP

WORKDIR /app
COPY ./publish/ ./

HEALTHCHECK --interval=60s --timeout=5s --start-period=15s --retries=3 \
  CMD curl --fail http://localhost/hsr-api/v1/info || exit 1

ENTRYPOINT ["dotnet", "UserService.Host.dll"]
