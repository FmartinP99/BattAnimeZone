﻿using BattAnimeZone.DatabaseModels;
using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BattAnimeZone.Authentication
{
    public class JwtAuthenticationManager
	{
		private const int JWT_TOKEN_VALIDITY_MINS = 60;
		private string JWT_SECURITY_KEY;

		public JwtAuthenticationManager()
		{
            JWT_SECURITY_KEY = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY");
        }

		public UserSession? GenerateJwtToken(UserAccountModel? userAccount)
		{
            /* Generating JWT Token */
            var tokenExpiryTimeStamp = DateTime.Now.ToUniversalTime().AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
			var claimsIdentity = new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.Name, userAccount.UserName),
					new Claim(ClaimTypes.Role, userAccount.Role)
				});
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(tokenKey),
				SecurityAlgorithms.HmacSha256Signature);
			var securityTokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claimsIdentity,
				Expires = tokenExpiryTimeStamp,
				SigningCredentials = signingCredentials
			};

			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
			var token = jwtSecurityTokenHandler.WriteToken(securityToken);

			/* Returning the User Session object */
			var userSession = new UserSession
			{
				UserName = userAccount.UserName,
				Role = userAccount.Role,
				Token = token,
				ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now.ToUniversalTime()).TotalSeconds
			};

            return userSession;
		}
	}
}
