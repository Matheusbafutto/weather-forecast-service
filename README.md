# Weather forecast API

Service to manage weather forecasting based on geografic coordinates.

## Prereqs for local development

For the moment, local development requires a local instalation of the .NET SDK v7.0 and mongodb for the data store. You may find installation guides for both in the links below:

- [.NET SDK 7.0](https://learn.microsoft.com/en-us/dotnet/core/install/)
- [mongodb community](https://www.mongodb.com/docs/manual/administration/install-community/#std-label-install-mdb-community-edition)

## Setup

To start local development, start the mongo instance and the run the .NET app.

### Mongo data store

Running on mac:
```
brew install mongodb-community@7.0
brew services start mongodb-community@7.0
```

Stopping Mongodb: `brew services start mongodb-community@7.0`

Testing the database with mongo shell (usually host = localhost and port = 27017): `mongosh mongodb://<local-mongodb-host>:<local-mongodb-port>`

### .NET application

A successful installation of the .NET SDK should give you access to the `dotnet` CLI which you may then use to start the app with the following commands:

```
git clone git@github.com:Matheusbafutto/weather-forecast-service.git
cd weather_forecast_service
export MONGODB_URI=mongodb://<local-mongodb-host>:<local-mongodb-port>
dotnet run
```

## TODO

- [ ] Work test coverage
- [ ] Setup remote database
- [ ] Replace in memory data store with remote database client
- [ ] Add json validation to open-meteo calls
- [ ] Add input validation to API endpoints (ensure range valid for lat and long)
- [ ] dockerize application and database
- [ ] isolate usage per user session (?)
