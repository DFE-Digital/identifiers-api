# identifiers-api
Simple api for getting unique identifiers from the Academies Database

## Docker

### On Windows

There can be some issues with running Docker on Windows. If your container doesn't start or you see an error like this `/bin/bash^M: bad interpreter: No such file or directory` it may be caused by line endings in the bash shell script. To solve this you can run this command in git bash (or another shell of your choice) from the playwright directory:

 `sed -i -e 's/\r$//' ../../docker/web-docker-entrypoint.sh`

This will re write your line endings. The repo will ignore this so checking in the updated file will not fix it unfortunately.

### On macOS

If you are running on Apple M1 chip the SQL Server image (used by the test db) may not work. This can be fixed by:

- Docker Settings > General: [X] Use virtualization framework and
- Docker Settings > Features in Development: [X] Use Rosetta...


### Launching the containers

Launch a local copy of the app with Docker compose.

Enter the `docker` folder in the root of the project

```
cd docker
```

You will need to populate two .env files, one for the database, and one for the app, set a sensible SQL password in both files.

```
cp .env.example .env
cp .env.database.example .env.database
```

Before you can launch the containers, you will need to be authenticated to GitHub Container Registry so that you can pull down the image used for the mock academies database.

Please create a GitHub Personal Access Token (classic) with `read:packages` scope.

Save your PAT as an environment variable

```
export CR_PAT=YOUR_TOKEN
```

Now login to GitHub Container Registry

```
echo $CR_PAT | docker login ghcr.io -u USERNAME --password-stdin
> Login Succeeded
```

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
