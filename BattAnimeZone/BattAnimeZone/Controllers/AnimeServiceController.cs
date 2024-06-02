using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

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

	}
}
