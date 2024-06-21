﻿using BattAnimeZone.Authentication;
using BattAnimeZone.Authentication.PasswordHasher;
using BattAnimeZone.DatabaseModels;
using BattAnimeZone.DbContexts;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using LoginRequest = BattAnimeZone.Shared.Models.User.LoginRequest;
using RegisterRequest = BattAnimeZone.Shared.Models.User.RegisterRequest;

namespace BattAnimeZone.Services
{
    public partial class UserAccountService
	{
        private IDbContextFactory<AnimeDbContext> _dbContextFactory;
		private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenBlacklistingService _tokenBlacklistingService;

        public UserAccountService(IDbContextFactory<AnimeDbContext> dbContextFactory, ITokenBlacklistingService tokenBlacklistingService)
		{
			_dbContextFactory = dbContextFactory;
			_passwordHasher = new PasswordHasher();
            _tokenBlacklistingService = tokenBlacklistingService;
		}



		public async Task<UserSession?> Login(LoginRequest loginRequest)
		{
			UserAccountModel? userAccount = null;
			using (var _context = await _dbContextFactory.CreateDbContextAsync())
			{
				userAccount = await _context.UserAccounts.Where(x => x.UserName == loginRequest.UserName).FirstOrDefaultAsync();
			}
			if (userAccount == null) return null;


 

            var jwtAuthenticationManager = new JwtAuthenticationManager();
            UserSession? userSession;
			bool isMatching = _passwordHasher.Verify(userAccount.Password, loginRequest.Password);
			if (!isMatching) return null;
			userSession = jwtAuthenticationManager.GenerateJwtToken(userAccount);

            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                userAccount.Token = userSession.Token;
                userAccount.RefreshToken = userSession.RefreshToken;
                userAccount.RefreshTokenExpiryTime = DateTime.Now.ToUniversalTime().AddMinutes(jwtAuthenticationManager.JWT_REFRESH_TOKEN_VALIDITY_MINS).ToString("o");

                _context.UserAccounts.Update(userAccount);
                await _context.SaveChangesAsync();
            }

            return userSession;
        }



        public async Task<bool> Logout(UserSession uSession)
        {

            UserAccountModel? userAccount = null;
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                userAccount = await _context.UserAccounts.Where(x => x.UserName == uSession.UserName).FirstOrDefaultAsync();
                if (userAccount == null) return false;
                userAccount.RefreshToken = null;
                userAccount.Token = null;
                userAccount.RefreshTokenExpiryTime = null;
                _tokenBlacklistingService.BlacklistToken(uSession.Token);
                _context.UserAccounts.Update(userAccount);
                await _context.SaveChangesAsync();
            }
            return true;

        }


        public async Task<RefreshTokenDTO?> Refresh(RefreshTokenDTO refreshTokenDTO)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager();
            var principal = jwtAuthenticationManager.GetPrincipalFromExpiredToken(refreshTokenDTO.Token);
            var username = principal.Identity.Name;

           

            UserAccountModel? userAccount = null;
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                userAccount = await _context.UserAccounts.Where(x => x.UserName == username).FirstOrDefaultAsync();
                if (userAccount == null) return null;

                DateTime refreshTokenExpiryDate = DateTime.Parse(userAccount.RefreshTokenExpiryTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
                if (userAccount.RefreshToken != refreshTokenDTO.RefreshToken || refreshTokenExpiryDate < DateTime.Now.ToUniversalTime()) return null;
                
                var signingCredentials = jwtAuthenticationManager.GetSigningCredentials();
                var claims = await jwtAuthenticationManager.GetClaims(userAccount);

                var tokenOptions = jwtAuthenticationManager.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                var refreshToken = jwtAuthenticationManager.GenerateRefreshToken();
                userAccount.RefreshToken = refreshToken;

                _context.UserAccounts.Update(userAccount);
                await _context.SaveChangesAsync();

                return new RefreshTokenDTO
                {
                    Token = token,
                    TokenExpiryTimeStamp = DateTime.Now.ToUniversalTime().AddMinutes(jwtAuthenticationManager.JWT_TOKEN_VALIDITY_MINS),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTimeStamp = refreshTokenExpiryDate
                };
            }         
  
        }
        


        public async Task<bool> RegisterUser(RegisterRequest user)
        {

            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                bool db_UserExists = await _context.UserAccounts.AnyAsync(x => x.UserName == user.UserName);
                if (db_UserExists) return false;
                bool db_EmailExists = await _context.UserAccounts.AnyAsync(x => x.Email == user.Email);
                if (db_EmailExists) return false;
                string? passwordHash = _passwordHasher.Hash(user.Password);
				_context.UserAccounts.Add(new UserAccountModel { UserName = user.UserName, Password = passwordHash, Email = user.Email.ToLower(), Role = "User" });
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> RateAnime(AnimeActionTransfer aat, string? token)
        {

            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                UserAccountModel? db_User = await _context.UserAccounts.Where(x => x.UserName == aat.UserName && x.Token == token).FirstOrDefaultAsync();
                if (db_User == null) return false;

                AnimeUserModel? db_animeUser = await _context.AnimeUserModels.Where(x => x.AnimeId == aat.AnimeId && x.UserId == db_User.Id).FirstOrDefaultAsync();

                AnimeUserModel updated_entry;

                if (db_animeUser != null && aat.Status == null)
                {
                    _context.AnimeUserModels.Remove(db_animeUser);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else if (db_animeUser == null && aat.Status == null)
                {
                    return true;
                }

                if (db_animeUser == null)
                {
                    updated_entry = new AnimeUserModel
                    {
                        AnimeId = aat.AnimeId,
                        UserId = db_User.Id,
                        favorite = aat.Favorite,
                        Status = aat.Status,
                        Rating = aat.Rating
                    };
                    _context.AnimeUserModels.Add(updated_entry);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    db_animeUser.favorite = aat.Favorite;
                    db_animeUser.Status = aat.Status;
                    db_animeUser.Rating = aat.Rating;

                    _context.AnimeUserModels.Update(db_animeUser);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
        }


        public async Task<Dictionary<int, InteractedAnime>?> GetInteractedAnimes(string UserName, string? token)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                bool db_UserExists = await _context.UserAccounts.AnyAsync(x => x.UserName == UserName && x.Token == token);
                if (db_UserExists == false) return null;

                var query = await (from au in _context.AnimeUserModels
                             join u in _context.UserAccounts on au.UserId equals u.Id
                             join a in _context.Animes on au.AnimeId equals a.Mal_id
                             where u.UserName == UserName
                                   select new
                                   {
                                       a.Mal_id,
                                       InteractedAnime = new InteractedAnime()
                                       {
                                           Rating = au.Rating,
                                           Status = au.Status,
                                           Favorite = au.favorite
                                       }
                                   }).ToListAsync();

                Dictionary<int, InteractedAnime> interactedAnimesDictionary = query.ToDictionary(
                x => x.Mal_id,
                x => x.InteractedAnime
                );
                return interactedAnimesDictionary;
            }

        }

    }
}
