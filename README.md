# identifiers-api
Simple api for getting unique identifiers from the Academies Database

## Docker

Launch a local copy of the app with Docker compose.

Enter the `docker` folder in the root of the project

```
cd docker
```

You will need to populate a .env file with the necessary values.

```
cp .env.example .env
```

Edit the file and populate any missing values

Now launch a Docker container which will bind to `localhost:80`

```
docker compose up --build
```

## API Key Management

Api Key management is done through the use of config files. There are currently placeholder entries in the various forms of `appsettings.json`, but new keys should *not* be added to these files, or committed to this repository.

Api Keys are provisioned at the environment level, and are stored as JSON objects in the following format:

```json
{
    "userName": "<the user name>",
    "apiKey": "<the unique api key>"
}
```

If injected through the environment, use `ApiKeys__x` naming conventions for the variables, as .NET will automatically configure this for us. e.g. `export ApiKeys__0=xxxx` will define the first API in the array.
