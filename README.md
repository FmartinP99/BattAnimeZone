# BattAnimeZone (Work in Progress)

### Blazor (Full Stack, web server & web assembly) Web App written in C# with .NET8

This webapp is essentially a digital-library built on the [MyAnimeList](https://myanimelist.net)'s data. 
It is highly advised to use it via a desktop computer/laptop and not with mobil devices, because the UI currently not supported for mobile devices.

### Features from the User's perspective

- It's capable of searching and displaying animes and studios with/based on their various attributes,<br>
- Easy to use/Intuitive UI.
- User accounts.
- Rating/Categorizing animes. (Planned, Watched, Dropped, Favorite, etc...).



### Features from a technical perspective
- MVC pattern with Service & EF layer  (View <-> Controller <-> Service <-> DbContext/EF <-> Model/Database).
- Database queries are written with EntityFramework.
- Database layout is in 3NF.
- Code first approach.
- JWT based Authentication & Authorization system.
- Refresh Tokens and token blacklisting.
- Policy based authorization.
- Encrypted Session / Local storage.
- Custom HttpContext.
- Uses an in-memory cache for the past N search to speed up the popular database queries. See: `SingletonSearachService`.
- Custom Form validator.



### About the program
- The data was fetched from [MAL](https://myanimelist.net) using the [Jikan API](https://docs.api.jikan.moe) to build the database.
- Missing numerical data was filled with 0.
- You can find the python script for it in the /Files folder.
- All the data the program uses to create the database is in the /Files folder.
- The frontend was made by using [Radzen](https://blazor.radzen.com) and [BlazorBootstrap](https://demos.blazorbootstrap.com) components.
- Currently the program uses Sqlite3. 
- Because of this, and the usage of EntityFramework, it is easy to replace the current SQlite3 database with some other relational database.
- Based on the `appsettings.json`'s `ConnectionStrings`, if the database does not exists, it creates one based on the `Data.sql` in the `/Files` folder.
- The standalone Web server only version (older version) of the program available [here](https://github.com/FmartinP99/BattAnimeZone_WebServer).


### Run

- .NET8 Required.
- Download repository.
- Open the .sln.
- Create a `.env` file in the same folder where the `.env.example` file is located and put `JWT_SECURITY_KEY={your-string-whatever-you-want}` into the file. <br>
- Also you can change the other enviroment variables however you want based on the `.env.example` file.
- If you want to use a different database, or different paths, change the `appsettings.json`'s `ConnectionStrings` part.
- Run.


