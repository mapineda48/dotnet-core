#https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0.403-buster-slim-amd64 AS builder

# Nodejs
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - &&\
apt-get install -y nodejs &&\
npm install --global yarn

# Application

WORKDIR /home/builder

COPY . .

RUN dotnet publish \
  --runtime alpine-x64 \
  --self-contained true \
  /p:PublishTrimmed=true \
  /p:PublishSingleFile=true \
  -c Release \
  -o ./dist
# chown -R 777 /home/dotnet/app/bin/Debug/net5.0/publish/**/* ./start

#https://hub.docker.com/_/microsoft-dotnet-runtime-deps/
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0.12-alpine3.14-amd64

RUN apk add icu-libs

RUN adduser \
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