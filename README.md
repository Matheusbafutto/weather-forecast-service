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

## Tests

Mocking in C# has proven challenging. I am adding a few test cases while I work to improve test quality and coverage.

### Running tests

`dotnet test`

## API summary

Swagger URL: `<baseURl>/swagger`
In local development this link will look like `http://localhost:5118/swagger`

- `GET /weatherforecats/all`: gets all previous entries with their last forecasts
- `GET /weatherforecats?id=<entryId>`: gets previously forecasted record by its id
- `GET /weatherforecats/location?latitude=<float>&longitude=<float>`: gets previously forecasted record by its coordinates
- `POST /weatherforecats?latitude=<float>&longitude=<float>`: Creates new record with input coordinates and a new approximated forecast for location
- `PUT /weatherforecats?id=<id>`: Overrides existing record forecast with the latest forecast for that location
- `DELETE /weatherforecats?id=<id>`: Removes the record for that location from the database

## TODO

- [x] Add some test coverage
- [x] Setup remote database
- [x] Replace in memory data store with remote database client
- [ ] Add json validation to open-meteo calls
- [ ] Add input validation to API endpoints (ensure range valid for lat and long)
- [ ] dockerize application and database
- [ ] isolate usage per user session (?)
