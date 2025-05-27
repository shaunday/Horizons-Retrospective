FROM mcr.microsoft.com/dotnet/aspnet:9.0

ARG VERSION=unknown

LABEL version=$VERSION
ENV APP_VERSION=$VERSION
ENV COMMIT_SHA=$COMMIT_SHA

WORKDIR /app
COPY ./publish/ ./

HEALTHCHECK --interval=60s --timeout=5s --start-period=15s --retries=3 \
  CMD curl --fail http://localhost/hsr-api/v1/info || exit 1

ENTRYPOINT ["dotnet", "AssetsFlowWeb.API.dll"]
