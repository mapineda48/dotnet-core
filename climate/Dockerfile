#https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0.403-buster-slim-amd64 AS builder

WORKDIR /home/builder

COPY . .

RUN bash ./install.sh && \
    dotnet publish \
      --runtime alpine-x64 \
      --self-contained true \
      /p:PublishTrimmed=true \
      /p:PublishSingleFile=true \
      -c Release \
      -o ./dist

#https://hub.docker.com/_/microsoft-dotnet-runtime-deps/
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0.12-alpine3.14-amd64

RUN apk add icu-libs && \
    adduser \
      --disabled-password \
      --home /home/app \
      --gecos '' app \
      && chown -R app /home/app

USER app

WORKDIR /home/app

COPY --from=builder /home/builder/dist .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
  DOTNET_RUNNING_IN_CONTAINER=true

CMD [ "./Climate" ]