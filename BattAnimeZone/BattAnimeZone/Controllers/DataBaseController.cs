using BattAnimeZone.DatabaseInitializer;
using BattAnimeZone.DbContexts;
using BattAnimeZone.Services;
using BattAnimeZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BattAnimeZone.Controllers
{
	[Route("api/DbController")]
	[ApiController]
	public class DataBaseController : Controller
	{

		private IDataBaseService? _idataBaseService = null;


		public DataBaseController(IServiceScopeFactory serviceScopeFactory)
		{
            DataBaseService? _dataBaseService = null;
            SupaBaseService? _supaBaseService = null;

            using (var serviceScope = serviceScopeFactory.CreateScope())
		    {
			    _dataBaseService = serviceScope.ServiceProvider.GetService<DataBaseService>();
			    _supaBaseService = serviceScope.ServiceProvider.GetService<SupaBaseService>();
		    }
            if(_dataBaseService != null)
            {
                _idataBaseService = _dataBaseService;
            }else if(_supaBaseService != null)
            {
                _idataBaseService = _supaBaseService;
            }
			

			
		}


		[HttpGet("GetAnimesByYear/{year}")]
		public async Task<IActionResult> GetAnimesByYear(int year)
		{
            var result = await _idataBaseService.GetAnimesForHomePageByYear(year);
           
            if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpGet("GetAnime/{mal_id}")]
		public async Task<IActionResult> GetAnimePageDTOByID(int mal_id)
		{
			var result = await _idataBaseService.GetAnimePageDTOByID(mal_id);
		
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}


        [HttpGet("GetRelations/{mal_id}")]
        public async Task<IActionResult> GetRelations(int mal_id)
        {
			var result = await _idataBaseService.GetRelations(mal_id);
			
			if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet("GetMediaTypes")]
        public async Task<IActionResult> GetMediaTypes()
        {
            var result = await _idataBaseService.GetDistinctMediaTypes();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

		[HttpGet("GetYears")]
		public async Task<IActionResult> GetYears()
		{
			var result = await _idataBaseService.GetDistinctYears();
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}


		[HttpGet("GetGenres")]
        public async Task<IActionResult> GetGenres()
        {
			var result = await _idataBaseService.GetGenres();
		

			if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }



        [HttpGet("GetAnimesPerGenreIdCount")]
        public async Task<IActionResult> GetAnimesPerGenreIdCount()
        {
            var result = await _idataBaseService.GetAnimesPerGenreIdCount();
			

			if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet("GetAnimesForListGenreAnimes/{mal_id}")]
        public async Task<IActionResult> GetAnimesForListGenreAnimes(int mal_id)
        {
            var result = await _idataBaseService.GetAnimesForListGenreAnimes(mal_id);
           

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

			var result = await _idataBaseService.GetFilteredAnimes(genres, mediaTypes, yearlower, yearupper);
	
			if (!result.Any() || result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }



        [HttpGet("GetSimilarAnimesForSearchResult")]
        public async Task<IActionResult> GetSimilarAnimesForSearchResult([FromQuery] int similar_number, [FromQuery] string searched_term)
        {

            var result = await _idataBaseService.GetSimilarAnimesForSearchResult(similar_number, searched_term);
          

			if (result == null)
            {
                return NotFound();
            }
            return Ok(result);


        }

        [HttpGet("GetProductionEntities")]
        public async Task<IActionResult> GetProductionEntities()
        {
            var result = await _idataBaseService.GetProductionEntitiesDTO();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetAnimesForProdEnt/{mal_id}")]
        public async Task<IActionResult> GetAnimesForProdEnt(int mal_id)
        {
            var result = await _idataBaseService.GetAnimesForProdEnt(mal_id);


            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


    }
}
