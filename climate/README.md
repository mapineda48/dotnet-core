# Climate

This project is a developer technical test.

## Usage

### docker-compose

Do not complicate configuring an environment to run the application, this demo has a [container](https://hub.docker.com/r/mapineda48/climate) available in [dockerhub](https://hub.docker.com/), the only thing you should have installed on your pc is [docker-compose](https://docs.docker.com/compose/), create a directory, in this create a file called `docker-compose.yml` and add the following:

```yml
version: "3.3"
services:
  web:
    image: mapineda48/climate
    environment:
      # Milliseconds, the database service may take a few seconds to be ready, 
      # this value may vary depending on the hardware.
      APP_RUN_DELAY: "10000"
      
      SQLSERVER: "Server=db;User Id=sa;Password=Mypassword12345*;Encrypt=false;Trusted_Connection=False;"
      
      # Register on https://openweathermap.org/ to get it
      WEATHER_KEY_API: "<your api key>"
      
      # Register on https://newsapi.org/ to get it
      NEWS_KEY_API: "<your api key>"
    ports:
      - "3000:80"
    depends_on:
      - db
  db:
    image: "mcr.microsoft.com/mssql/server:2019-CU13-ubuntu-20.04"
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "Mypassword12345*"
    ports:
      - "1433:1433"
```
now open a terminal and navigate to the directory where you created the file and run:

```sh
docker-compose up
```
Wait a few minutes while the dependencies are downloaded, the services are configured and when you see that the services are ready go to the browser and go to http://localhost:3000

### docker

If you prefer you can also use the container without docker-compose:

```sh
docker run \
    --name demo \
    -p 3000:80 \
    -e "APP_RUN_DELAY=<time in ms>" \
    -e "SQLSERVER=<string connection>" \
    -e "WEATHER_KEY_API=<your api key>" \
    -e "NEWS_KEY_API=<your api key>" \
    -d \
    mapineda48/climate
```
### local environment

Make sure you have installed [.NET 5 or newer](https://get.dot.net), [Nodejs 14.x](https://nodejs.org/es/), [Yarn](https://yarnpkg.com/) and set enviroment variables.

## Sources

- [Startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-5.0)
- [WebApi](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code)
- [.NET and CRA](https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-5.0&tabs=visual-studio)
- [EF Core](https://docs.microsoft.com/en-us/ef/core/)
- [Lazy loading with proxies](https://docs.microsoft.com/en-us/ef/core/querying/related-data/lazy)
- [Model Binding](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-5.0)
- [Circular reference Serialization](https://docs.microsoft.com/en-us/ef/core/querying/related-data/serialization)
- [Testing with the EF In-Memory](https://docs.microsoft.com/en-us/ef/core/testing/in-memory)
- [DataBase with EF Core](http://www.binaryintellect.net/articles/87446533-54b3-41ad-bea9-994091686a55.aspx)
- [Containerize your .NET Core app – the right way](https://medium.com/01001101/containerize-your-net-core-app-the-right-way-35c267224a8d)
- [aspnet mssql compose](https://docs.docker.com/samples/aspnet-mssql-compose/)