using BattAnimeZone.Authentication;
using BattAnimeZone.Authentication.PasswordHasher;
using BattAnimeZone.Shared.Models.User;

namespace BattAnimeZone.Services
{
	public class UserAccountService
	{
		private List<UserAccount> _userAccountList;
		private readonly IPasswordHasher _passwordHasher;

		public UserAccountService()
		{
			//for testing purposes
			_userAccountList = new List<UserAccount>
				{
					new UserAccount{ UserName = "admin", Password = "admin", Role = "Administrator", Email="admin@temp.com" },
					new UserAccount{ UserName = "user", Password = "user", Role = "User",  Email="user@temp.com" }
				};

			_passwordHasher = new PasswordHasher();
		}

		public UserAccount? GetUserAccountByUserName(string userName)
		{
            foreach(var userAccount in _userAccountList)
			{
                Console.WriteLine($"{userAccount.UserName} - {userAccount.Password}");
            }

            return _userAccountList.FirstOrDefault(x => x.UserName == userName);
		}

		public UserSession? Login(LoginRequest loginRequest)
		{
            var userAccount = this.GetUserAccountByUserName(loginRequest.UserName);
			if (userAccount == null) return null;

            var jwtAuthenticationManager = new JwtAuthenticationManager(this);
            UserSession? userSession;
			

            if (loginRequest.UserName == "admin" || loginRequest.UserName == "user")
			{
				 userSession = jwtAuthenticationManager.GenerateJwtToken(userAccount);
            }
			else
			{
				bool isMatching = _passwordHasher.Verify(userAccount.Password, loginRequest.Password);
				if (!isMatching) return null;
				userSession = jwtAuthenticationManager.GenerateJwtToken(userAccount);
            }
			return userSession;
        }

        public bool RegisterUser(RegisterRequest user)
        {

			var passwordHash = _passwordHasher.Hash(user.Password);

            bool userExists = _userAccountList.Any(account => account.UserName == user.UserName || account.Email == user.Email);
            Console.WriteLine(userExists);
            if (userExists) return false;

            try { 
            _userAccountList.Add(
				new UserAccount { UserName = user.UserName, Password = passwordHash, Role = "User", Email = user.Email }
				);
				return true;
            }catch (Exception e)
			{
                return false;
			}
        }

    }
}
