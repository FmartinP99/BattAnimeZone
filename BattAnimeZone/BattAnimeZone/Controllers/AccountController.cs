using BattAnimeZone.Authentication;
using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BattAnimeZone.Controllers
{
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
            Console.WriteLine("belépett ide");
            var jwtAuthenticationManager = new JwtAuthenticationManager(_userAccountService);
			var userSession = jwtAuthenticationManager.GenerateJwtToken(loginRequest.UserName, loginRequest.Password);
			if (userSession is null)
				return Unauthorized();
			else
				return userSession;
		}
	}
}
