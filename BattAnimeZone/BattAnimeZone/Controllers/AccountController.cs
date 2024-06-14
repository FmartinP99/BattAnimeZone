using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
            await Console.Out.WriteLineAsync("LOGIN!!");
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
    }
}
