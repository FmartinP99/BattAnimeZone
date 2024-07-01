# BattAnimeZone (Work in Progress)

### Blazor (Full Stack, web server & web assembly) Web App written in C# with .NET8

This webapp is essentially a digital-library built on the [MyAnimeList](https://myanimelist.net)'s data. 
It is highly advised to use it via a desktop computer or a laptop and not with mobile devices, as the UI is currently not supported on them.
<hr>

## Demo

The demo version of the web app is available [here](http://battanimezone.com).
<hr>

### Features from the User's perspective

- It's capable of searching and displaying animes and studios with/based on their various attributes,<br>
- Easy to use/Intuitive UI.
- User accounts.
- Rating/Categorizing animes. (Planned, Watched, Dropped, Favorite, etc...).



### Features from a technical perspective
- MVC pattern with Service & EF layer  (View <-> Controller <-> Service <-> DbContext/EF <-> Model/Database).
- Local database queries are written with EntityFramework, SupaBase queries are written with rpc functions.
- Database layout is in 3NF.
- Code first approach.
- JWT based Authentication & Authorization system.
- Refresh Tokens and token blacklisting.
- Policy based authorization.
- Encrypted Session / Local storage.
- Custom HttpContext.
- Uses an in-memory cache for the past N search to speed up the popular database queries. See: `SingletonSearchService`.
- Custom Form validator.



### About the program
- The data was fetched from [MAL](https://myanimelist.net) using the [Jikan API](https://docs.api.jikan.moe) to build the database.
- Missing numerical data was filled with 0.
- You can find the python script for it in the /Files folder.
- All the data the program uses to create the database is in the /Files folder.
- The frontend was made by using [Radzen](https://blazor.radzen.com) and [BlazorBootstrap](https://demos.blazorbootstrap.com) components.
- Currently the program uses SQLite3. 
- Because of this, and the usage of EntityFramework, it is easy to replace the current SQLite3 database with some other relational database.
- Based on the `appsettings.json`'s `ConnectionStrings`, if the database does not exists, it creates one based on the `Data.sql` in the `/Files` folder.
- The standalone Web server only version (older version) of the program available [here](https://github.com/FmartinP99/BattAnimeZone_WebServer).


### Run

- .NET8 Required.
- SQLite3 or Supabase is required.
- Download repository.
- Open the .sln.
- Create a `.env` file in the same folder where the `.env.example` file is located and put `JWT_SECURITY_KEY={your-string-whatever-you-want}` into the file. <br>
- Also you can change the other enviroment variables however you want based on the `.env.example` file.
- If you want to use a different local database, or different paths, change the `appsettings.json`'s `ConnectionStrings` part.
- Run.


### Using Supabase instead of local (currently SQLite3) database.

- If you want to use the SupaBase implementation, inside the .env file, change the `USE_SQLite3_DATABASE=` to `false`, the `USE_SUPABASE_DATABASE=` to `true`, and provide the necessary `SUPABASE_URL,SUPABASE_KEY` variables.
- Copy all of the `postgres_data.sql` file's content into your Supabase SQL editor and run it.<br> These commands make the necessary tables, and the necessary database functions that the program needs to call. 


### About the ConnectionStrings inside the appsettings.json

- `Database` Data source of the local database.
- `DatabasePath` Path of the local database.
- `DatabaseInitFilePath` The sql script that will run when the local database is being initialized.
- `CsvFileToInitDataForDbAnimes` .csv file path where the Animes will be read from on DbInit.
- `CsvFileToInitDataForProductionEntities` .csv file path where the ProductionEntities (Studios on the UI) will be read from on DbInit.
- `CsvFileToInitDataForDbGenres` .csv file path where the Genres will be read from on DbInit.

### About the .env variables

- `JWT_TOKEN_VALIDITY_MINS=` validity of the jwt token in minutes.
- `JWT_REFRESH_TOKEN_VALIDITY_MINS=` validity of the refresh token in minutes.
- `ValidIssuer=` for authorized controllers/function the entity that issued the token.
- `ValidAudience=` for authorized controllers/function where the recieving controller is.
- `DbInit=` should the program pre-create and prefill the database. (for SupaBase you need to pre-create it by hand, using the steps mentioned above).
- `MakeMockUsersOnDbInit=` should the program make mock users for testing purposes. (not supported for supabase, it will be ignored).
- `USE_SQLite3_DATABASE=` program will use SQLite3 database if set to `true`.
- `USE_SUPABASE_DATABASE=` program will use SupaBase if set to `true` and `USE_SQLite3_DATABASE=` is set to `false`.
- `SUPABASE_URL=` the url of your supabase database. Will be ignored if SupaBase is not in use.
- `SUPABASE_KEY=` the key of your supabase database. Will be ignored if SupaBase is not in use.


### Using docker

There's a Dockerimage provided in the root folder. If you want to use it via docker, you need to use the following:

- Open your terminal
- `docker build -t <image_name> .`  for example: `docker build -t battanimezone .` The `<image_name>` can be whatever you want.
-  `docker run -p 8080:8080 -p 8081:8081 --name <container_name> --env-file BattAnimeZone/BattAnimeZone/.env <image_name>`
	-  the port mappings can be anything (8080:8080), it depends on what ports you want to use for your docker and for your machine
	-  the `<container_name>` is the name of your container. It can be anything you want.
	-  the `--env-file BattAnimeZone/BattAnimeZone/.env` is the location of the enviroment file.
	-  the `<image_name>` at the end is the name of your image you set during the `build` command. If you changed it, it is something else.

If you want to save the built image to transfer it to somewhere else, you need to use the following commands:
- `docker save -o <image_name>.tar <image_name>:latest`

Now you can move the <image_name>.tar file.<br>
To load and run the image, use the following command:

- `docker load -i <image_name>.tar`
- Then use the run command mentioned above.
