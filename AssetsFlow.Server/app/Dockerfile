FROM mcr.microsoft.com/dotnet/aspnet:9.0

ARG VERSION=unknown

LABEL version=$VERSION
ENV APP_VERSION=$VERSION

WORKDIR /app
COPY ./publish/ ./

ENTRYPOINT ["dotnet", "AssetsFlowWeb.API.dll"]