FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine

ENV DOTNET_CLI_HOME=/tmp/

COPY _out /tests
WORKDIR /tests

ENTRYPOINT ["tail", "-f", "/dev/null"]
