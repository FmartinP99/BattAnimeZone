using BattAnimeZone.Authentication;
using BattAnimeZone.Authentication.PasswordHasher;
using BattAnimeZone.DatabaseModels;
using BattAnimeZone.DbContexts;
using BattAnimeZone.Shared.Models.User;
using Microsoft.EntityFrameworkCore;

namespace BattAnimeZone.Services
{
	public class UserAccountService
	{
        private IDbContextFactory<AnimeDbContext> _dbContextFactory;
		private readonly IPasswordHasher _passwordHasher;

		public UserAccountService(IDbContextFactory<AnimeDbContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
			_passwordHasher = new PasswordHasher();
		}



		public async Task<UserSession?> Login(LoginRequest loginRequest)
		{
			UserAccountModel? userAccount = null;
			using (var _context = await _dbContextFactory.CreateDbContextAsync())
			{
				userAccount = await _context.UserAccounts.Where(x => x.UserName == loginRequest.UserName).FirstOrDefaultAsync();
			}
			if (userAccount == null) return null;


            await Console.Out.WriteLineAsync($"a jelszo: {loginRequest.Password}  db-ben userAccount jelszo: {userAccount.Password}");

            var jwtAuthenticationManager = new JwtAuthenticationManager(this);
            UserSession? userSession;
			bool isMatching = _passwordHasher.Verify(userAccount.Password, loginRequest.Password);
			if (!isMatching) return null;
			userSession = jwtAuthenticationManager.GenerateJwtToken(userAccount);
            
			return userSession;
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
				_context.UserAccounts.Add(new UserAccountModel { UserName = user.UserName, Password = passwordHash, Email = user.Email, Role = "User" });
                await _context.SaveChangesAsync();
                return true;
            }
        }

    }
}
