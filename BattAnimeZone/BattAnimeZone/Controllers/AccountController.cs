using BattAnimeZone.Authentication;
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
		public ActionResult<UserSession> Login([FromBody] LoginRequest loginRequest)
		{
            var jwtAuthenticationManager = new JwtAuthenticationManager(_userAccountService);
			var userSession = jwtAuthenticationManager.GenerateJwtToken(loginRequest.UserName, loginRequest.Password);
			if (userSession is null)
				return Unauthorized();
			else
				return userSession;
		}

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public ActionResult<UserSession> Register([FromBody] RegisterRequest registerRequest)
        {
            bool response = _userAccountService.RegisterUser(registerRequest);

            if (response)
                return Ok();
            else
                return BadRequest();
        }
    }
}
