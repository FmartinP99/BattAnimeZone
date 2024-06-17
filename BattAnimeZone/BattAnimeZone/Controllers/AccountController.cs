using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Shared.Models.User.SessionStorageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BattAnimeZone.Controllers
{
    [Authorize]
	[Route("api/AccountController")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private UserAccountService _userAccountService;

		public AccountController(UserAccountService userAccountService)
		{
			_userAccountService = userAccountService;
		}

		[HttpPost]
		[Route("Login")]
		[AllowAnonymous]
		public async Task<ActionResult<UserSession>> Login([FromBody] LoginRequest loginRequest)
		{
           
            var userSession = await _userAccountService.Login(loginRequest);
			if (userSession is null)
				return Unauthorized();
			else
				return userSession;
		}

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserSession>> Register([FromBody] RegisterRequest registerRequest)
        {
            bool response = await _userAccountService.RegisterUser(registerRequest);

            if (response)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("AnimeRating")]
        public async Task<ActionResult<UserSession>> AnimeRating([FromBody] AnimeActionTransfer aat)
        {
            bool response = await _userAccountService.RateAnime(aat);
            if (response)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("GetInteractedAnimes/{username}")]
        public async Task<ActionResult<UserSession>> GetInteractedAnimes(string username)
        {
            var response = await _userAccountService.GetInteractedAnimes(username);
            if (response != null)
            {
                return Ok(response);
            }
            else
                return BadRequest();
        }
    }
}
