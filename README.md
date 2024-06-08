# BattAnimeZone (Work in Progress)

### Blazor (Full Stack) Web App written in C# with .NET8

This webapp is essentially a online-library capable of searching and displaying animes and studios with/based on their attributes,<br>
The program uses string-similarity comparsion to find similar animes for the <br> searched-string so there's no need for an exact search term.<br>
The program also features an Authentication & Authorization system. (various further features (i.e.: favorite/seen animes) will be added in the future.)


- The data was fetched from MAL using the Jikan API to build the database.
- Missing numerical data was filled with 0.
- You can find the python script for it in the /Files folder.
- All the data the program runs on is in the /Files folder.
- The frontend was made by using [Radzen](https://blazor.radzen.com) and [BlazorBootstrap](https://demos.blazorbootstrap.com) components.
- Currently the entire database is in memory. You can change the file it uses in the AnimeService's constructor to decrease memory consumption if needed.
- The standalone Web server only version of the program available [here](https://github.com/FmartinP99/BattAnimeZone_WebServer).
	


### Run

- .NET8 Required
- Download repository
- Open the .sln
- Run


