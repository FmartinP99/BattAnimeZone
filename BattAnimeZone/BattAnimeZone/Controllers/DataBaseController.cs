using BattAnimeZone.DbContexts;
using BattAnimeZone.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BattAnimeZone.Controllers
{
	[Route("api/DbController")]
	[ApiController]
	public class DataBaseController : Controller
	{
		private DataBaseService _dataBaseService;
        private SupabaseService _supabaseService;


		public DataBaseController(DataBaseService dataBaseService, SupabaseService supabaseService)
		{
			_dataBaseService = dataBaseService;
			_supabaseService = supabaseService;
		}


		[HttpGet("GetAnimesByYear/{year}")]
		public async Task<IActionResult> GetAnimesByYear(int year)
		{
            var result = await _dataBaseService.GetAnimesForHomePageByYear(year);
            if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpGet("GetAnime/{mal_id}")]
		public async Task<IActionResult> GetAnimePageDTOByID(int mal_id)
		{
			var result = await _dataBaseService.GetAnimePageDTOByID(mal_id);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}


        [HttpGet("GetRelations/{mal_id}")]
        public async Task<IActionResult> GetRelations(int mal_id)
        {
            var result = await _dataBaseService.GetRelations(mal_id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet("GetMediaTypes")]
        public async Task<IActionResult> GetMediaTypes()
        {
            var result = await _dataBaseService.GetDistinctMediaTypes();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

		[HttpGet("GetYears")]
		public async Task<IActionResult> GetYears()
		{
			var result = await _dataBaseService.GetDistinctYears();
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}


		[HttpGet("GetGenres")]
        public async Task<IActionResult> GetGenres()
        {
            var result = await _dataBaseService.GetGenres();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }



        [HttpGet("GetAnimesPerGenreIdCount")]
        public async Task<IActionResult> GetAnimesPerGenreIdCount()
        {
            var result = await _dataBaseService.GetAnimesPerGenreIdCount();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet("GetAnimesForListGenreAnimes/{mal_id}")]
        public async Task<IActionResult> GetAnimesForListGenreAnimes(int mal_id)
        {
            var result = await _dataBaseService.GetAnimesForListGenreAnimes(mal_id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet("GetFilteredAnimes")]
        public async Task<IActionResult> GetFilteredAnimes([FromQuery] List<int>? genres, [FromQuery] List<string>? mediaTypes,
            [FromQuery] int? yearlower, [FromQuery] int? yearupper)
        {
            var result = await _dataBaseService.GetFilteredAnimes(genres, mediaTypes, yearlower, yearupper);
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }



        [HttpGet("GetSimilarAnimesForSearchResult")]
        public async Task<IActionResult> GetSimilarAnimesForSearchResult([FromQuery] int similar_number, [FromQuery] string searched_term)
        {

            var result = await _dataBaseService.GetSimilarAnimesForSearchResult(similar_number, searched_term);

			await _supabaseService.fillDatabase();

			if (result == null)
            {
                return NotFound();
            }
            return Ok(result);


        }

        [HttpGet("GetProductionEntities")]
        public async Task<IActionResult> GetProductionEntities()
        {
            var result = await _dataBaseService.GetProductionEntitiesDTO();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetAnimesForProdEnt/{mal_id}")]
        public async Task<IActionResult> GetAnimesForProdEnt(int mal_id)
        {
            var result = await _dataBaseService.GetAnimesForProdEnt(mal_id);


            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


    }
}
