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

		public DataBaseController(DataBaseService dataBaseService)
		{
			_dataBaseService = dataBaseService;
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

	}
}
