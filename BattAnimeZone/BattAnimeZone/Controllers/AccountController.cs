using BattAnimeZone.Services.DataBase;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using BattAnimeZone.Shared.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BattAnimeZone.Services.SupaBase;
using BattAnimeZone.Services._Interfaces;
using BattAnimeZone.Services.Interfaces;

namespace BattAnimeZone.Controllers
{
    [Authorize(Policy = Policies.IsAuthenticated)]
	[Route("api/AccountController")]
	[ApiController]
	public class AccountController : ControllerBase
	{
        private IUserAccountService _iuserAccountService;

		public AccountController(IServiceScopeFactory serviceScopeFactory)
		{
            UserAccountService _userAccountService = null;
            SupaBaseUserAccountService _supaBaseUserAccountService = null;

            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                _userAccountService = serviceScope.ServiceProvider.GetService<UserAccountService>();
                _supaBaseUserAccountService = serviceScope.ServiceProvider.GetService<SupaBaseUserAccountService>();
            }
            if (_userAccountService != null)
            {
                _iuserAccountService = _userAccountService;
            }
            else if (_supaBaseUserAccountService != null)
            {
                _iuserAccountService = _supaBaseUserAccountService;
            }
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserSession>> Register([FromBody] RegisterRequest registerRequest)
        {

            bool response = await _iuserAccountService.RegisterUser(registerRequest);

            if (response)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
		[Route("Login")]
		[AllowAnonymous]
		public async Task<ActionResult<UserSession>> Login([FromBody] LoginRequest loginRequest)
		{
            var userSession = await _iuserAccountService.Login(loginRequest);
            if (userSession is null)
				return Unauthorized();
			else
				return userSession;
		}


        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult<UserSession>> Logout([FromBody] UserSession userSession)
        {
           
            var response = await _iuserAccountService.Logout(userSession);
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
          
            var userSession = await _iuserAccountService.Refresh(refreshTokenDTO);
            if (userSession is null)
                return Unauthorized();
            else
                return userSession;
        }

        [HttpPost]
        [Route("ChangeDetails")]
        public async Task<ActionResult<UserSession>> ChangeDetails([FromBody] ChangeDetailsRequest changeDetails)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
       
            var response = await _iuserAccountService.ChangeDetails(changeDetails, token);

            if (response.result == true)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpPost]
        [Route("AnimeRating")]
        public async Task<ActionResult<UserSession>> AnimeRating([FromBody] AnimeActionTransfer aat)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
			var token = authorizationHeader.Substring("Bearer ".Length).Trim();
      
            bool response = await _iuserAccountService.RateAnime(aat, token);
  
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
		
			var response = await _iuserAccountService.GetInteractedAnimes(username, token);
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
        public async Task<ActionResult<ProfilePageDTO>> GetProfile(string username)
        {
            var userName = await _iuserAccountService.GetProfileByUserName(username);

            if (userName is null)
                return NotFound();
            else
                return userName;
        }

        [HttpPost]
        [Route("DeleteAccount")]
        [AllowAnonymous]
        public async Task<ActionResult<DeleteAccountResponse>> DeleteAccount([FromBody] DeleteAccountRequest der)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
      
            var response = await _iuserAccountService.DeleteAccount(der, token);
            if (response.result  == false)
                return NotFound(response);
            else
                return response;
        }

    }
}
