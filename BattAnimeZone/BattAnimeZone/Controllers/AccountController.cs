using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using BattAnimeZone.Shared.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BattAnimeZone.Controllers
{
    [Authorize(Policy = Policies.IsAuthenticated)]
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
        [Route("Logout")]
        public async Task<ActionResult<UserSession>> Logout([FromBody] UserSession userSession)
        {
            var response = await _userAccountService.Logout(userSession);
            if (!response)
                return BadRequest();
            else
                return Ok();
        }

        [HttpPost]
        [Route("Refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<RefreshTokenDTO>> Refresh([FromBody] RefreshTokenDTO refreshTokenDTO)
        {

            var userSession = await _userAccountService.Refresh(refreshTokenDTO);
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
			var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
			var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            bool response = await _userAccountService.RateAnime(aat, token);
            if (response)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("GetInteractedAnimes/{username}")]
        public async Task<ActionResult<UserSession>> GetInteractedAnimes(string username)
        {
			var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
			var token = authorizationHeader.Substring("Bearer ".Length).Trim();
			var response = await _userAccountService.GetInteractedAnimes(username, token);
            if (response != null)
            {
                return Ok(response);
            }
            else
                return BadRequest();
        }


        [HttpGet]
        [Route("GetProfile/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<Dictionary<string, string?>>> GetProfile(string username)
        {

            await Console.Out.WriteLineAsync($"searchign term: {username}");
            var userName = await _userAccountService.GetProfileByUserName(username);
            await Console.Out.WriteLineAsync($"searched term: {userName}");
            if (userName is null)
                return NotFound();
            else
                return userName;
        }

    }
}
