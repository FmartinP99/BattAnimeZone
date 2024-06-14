# BattAnimeZone (Work in Progress)

### Blazor (Full Stack, web server & web assembly) Web App written in C# with .NET8

This webapp is essentially a digital-library built on the [MyAnimeList](https://myanimelist.net)'s data capable of searching and displaying animes and studios with/based on their attributes,<br>
The program uses string-similarity comparsion to find similar animes for the <br> searched-string so there's no need for an exact search term.<br>
The program also features an Authentication & Authorization system. (various further features (i.e.: favorite/seen animes) will be added in the future.)


- The data was fetched from [MAL](https://myanimelist.net) using the [Jikan API](https://docs.api.jikan.moe) to build the database.
- Missing numerical data was filled with 0.
- You can find the python script for it in the /Files folder.
- All the data the program runs on is in the /Files folder.
- The frontend was made by using [Radzen](https://blazor.radzen.com) and [BlazorBootstrap](https://demos.blazorbootstrap.com) components.
- Currently the program uses Sqlite3. 
- The standalone Web server only version (older version) of the program available [here](https://github.com/FmartinP99/BattAnimeZone_WebServer).
	


### Run

- .NET8 Required
- Download repository
- Open the .sln
- Run


