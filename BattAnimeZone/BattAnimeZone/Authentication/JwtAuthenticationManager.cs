using BattAnimeZone.DatabaseModels;
using BattAnimeZone.Services;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BattAnimeZone.Authentication
{
    public class JwtAuthenticationManager
	{
        internal readonly int JWT_TOKEN_VALIDITY_MINS = 2;
		internal readonly  int JWT_REFRESH_TOKEN_VALIDITY_MINS = 60 * 24;
		private string JWT_SECURITY_KEY;
		private string VALID_ISSUER;
		private string VALID_AUDIENCE;

		public JwtAuthenticationManager()
		{
            JWT_SECURITY_KEY = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY");
            VALID_ISSUER = Environment.GetEnvironmentVariable("ValidIssuer");
            VALID_AUDIENCE = Environment.GetEnvironmentVariable("ValidAudience");
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
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
				SigningCredentials = signingCredentials,
                Audience = VALID_AUDIENCE,
                Issuer = VALID_ISSUER
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
                TokenExpiryTimeStamp = tokenExpiryTimeStamp,
				RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiryTimestamp = DateTime.Now.ToUniversalTime().AddMinutes(JWT_REFRESH_TOKEN_VALIDITY_MINS),
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now.ToUniversalTime()).TotalSeconds
			};

            return userSession;
		}


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = VALID_AUDIENCE,
                ValidateIssuer = true,
                ValidIssuer = VALID_ISSUER,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(JWT_SECURITY_KEY)),
                ValidateLifetime = false,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }



        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(JWT_SECURITY_KEY);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<List<Claim>> GetClaims(UserAccountModel user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };
            return claims;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: VALID_ISSUER,
                audience: VALID_AUDIENCE,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(JWT_TOKEN_VALIDITY_MINS)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }



    }
}
