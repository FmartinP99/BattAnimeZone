using BattAnimeZone.Services;
using Microsoft.AspNetCore.Mvc;

namespace BattAnimeZone.Controllers
{
	[ApiController]
	[Route("api/AnimeService")]
	public class AnimeServiceController : ControllerBase
	{

		private readonly AnimeService _animeService;

		public AnimeServiceController(AnimeService animeService)
		{
			_animeService = animeService;
		}


		[HttpGet("GetAnimesByYear/{year}")]
		public async Task<IActionResult> GetAnimesByYear(int year)
		{
			var result  =  await _animeService.GetAnimesForHomePageByYear(year);
            if (result == null)  
            {
                return NotFound();
            }
            return Ok(result);
		}



		[HttpGet("GetAnime/{mal_id}")]
		public async Task<IActionResult> GetAnimePageDTOByID(int mal_id)
		{
			var result = await _animeService.GetAnimePageDTOByID(mal_id);
			if (result.Mal_id == -1)
			{
				return NotFound();
			}
			return Ok(result);
		}


        [HttpGet("GetAnimeRelationsKeyByID/{mal_id}")]
        public async Task<IActionResult> GetAnimeRelationsKeyByID(int mal_id)
        {
            var result = await _animeService.GetAnimeRelationsKeyByID(mal_id);
            if (result.Mal_id == -1)
            {
                return NotFound();
            }
            return Ok(result);
        }



        [HttpGet("GetRelations/{mal_id}")]
        public async Task<IActionResult> GetRelations(int mal_id)
        {
            var result = await _animeService.GetRelations(mal_id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetMediaTypes")]
        public async Task<IActionResult> GetMediaTypes()
        {
            var result = await _animeService.GetMediaTypes();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetGenres")]
        public async Task<IActionResult> GetGenres()
        {
            var result = await _animeService.GetGenres();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetAnimesPerGenreIds")]
        public async Task<IActionResult> GetAnimesPerGenreIds()
        {
            var result = await _animeService.GetAnimesPerGenreIds();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetAnimesPerGenreIdCount")]
        public async Task<IActionResult> GetAnimesPerGenreIdCount()
        {
            var result = await _animeService.GetAnimesPerGenreIdCount();
            
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet("GetAnimesPerMediaTypeIds")]
        public async Task<IActionResult> GetAnimesPerMediaTypeIds()
        {
            var result = await _animeService.GetAnimesPerMediaTypeIds();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("GetAnimesForListAnime")]
        public async Task<IActionResult> GetAnimesPerMediaTypeIds([FromBody] HashSet<int> animeids)
        {
            var result = await _animeService.GetAnimesForListAnime(animeids);
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetAnimesForListGenreAnimes/{mal_id}")]
        public async Task<IActionResult> GetAnimesForListGenreAnimes(int mal_id)
        {
            var animes = _animeService.GetAnimesForListGenreAnimes(mal_id);
            var genre_name = _animeService.GetGenreNameById(mal_id);

            await Task.WhenAll(animes, genre_name);

            var result = new
            {
                Animes = animes.Result,
                GenreName = genre_name.Result,
            };
            return Ok(result);
        }

        [HttpGet("GetProductionEntities")]
        public async Task<IActionResult> GetProductionEntities()
        {
            var result = await _animeService.GetProductionEntities();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetSimilarAnimesForSearchResult")]
        public async Task<IActionResult> GetSimilarAnimesForSearchResult([FromQuery] int similar_number, [FromQuery] string searched_term)
        {
            var result = await _animeService.GetSimilarAnimesForSearchResult(similar_number, searched_term);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet("GetAnimesForProdEnt/{mal_id}")]
        public async Task<IActionResult> GetAnimesForProdEnt(int mal_id)
        {
            var prodent = _animeService.GetProductionEntityById(mal_id);
            var producedAnimesTask = _animeService.GetProducedAnimes(mal_id);
            var licensedAnimesTask = _animeService.GetLicensedAnimes(mal_id);
            var studioAnimesTask = _animeService.GetStudioAnimes(mal_id);

            await Task.WhenAll(prodent, producedAnimesTask, licensedAnimesTask, studioAnimesTask);

            var result = new
            {
                ProdEnt = prodent.Result,
                ProducedAnimes = producedAnimesTask.Result,
                LicensedAnimes = licensedAnimesTask.Result,
                StudioAnimes = studioAnimesTask.Result
            };
            return Ok(result);
        }


    }
}
