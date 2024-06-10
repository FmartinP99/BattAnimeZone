using BattAnimeZone.DbContexts;
using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Utilities.CsvToDatabase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace BattAnimeZone.Controllers
{

    [Route("api/FillDatabaseController")]
    [ApiController]
    public class FillDatabaseController : ControllerBase
    {
        private readonly AnimeDbContext _context;
        private readonly AnimeService _animeService;
        public FillDatabaseController(AnimeDbContext context, AnimeService animeservice)
        {
            _context = context;
            _animeService = animeservice;
        }

        [HttpGet]
        [Route("FillDatabase")]
        public async Task<IActionResult> FillDatabase()
        {
            if (_animeService.isDatabaseFilled()) return Ok();

            await Console.Out.WriteLineAsync("step filldatabase");
            var animes = await _animeService.GetAllAnimes();
            var animesData = animes.Select(a => a.Value).ToList();


			await Console.Out.WriteLineAsync("queried animes");
			CsvToDataBaseHandler _csvToDataBaseHandler = new CsvToDataBaseHandler(_context);
            await _csvToDataBaseHandler.SaveAnimesToDatabase(animesData);
			await Console.Out.WriteLineAsync("saved animes\n");


			await Console.Out.WriteLineAsync("queried relations");
			await _csvToDataBaseHandler.SaveRelationsToDatabase(animes);
			await Console.Out.WriteLineAsync("saved relations\n");

			await Console.Out.WriteLineAsync("queried externals");
			await _csvToDataBaseHandler.SaveExternalsToDatabase(animesData);
			await Console.Out.WriteLineAsync("saved externals\n");

			await Console.Out.WriteLineAsync("queried streamings");
			await _csvToDataBaseHandler.SaveStreamingsToDatabase(animesData);
			await Console.Out.WriteLineAsync("saved streamings\n");

			await Console.Out.WriteLineAsync("queried animestreamings");
			await _csvToDataBaseHandler.SaveAnimeStreamingsToDatabase(animesData);
			await Console.Out.WriteLineAsync("saved animestreamings\n");




			_animeService.changeFilledFlag();
            return Ok();
        }
    }
}
