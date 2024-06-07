using BattAnimeZone.Authentication;
using BattAnimeZone.Shared.Models.User;

namespace BattAnimeZone.Services
{
	public class UserAccountService
	{
		private List<UserAccount> _userAccountList;
	

		public UserAccountService()
		{
			_userAccountList = new List<UserAccount>
				{
					new UserAccount{ UserName = "admin", Password = "admin", Role = "Administrator", Email="admin@temp.com" },
					new UserAccount{ UserName = "user", Password = "user", Role = "User",  Email="user@temp.com" }
				};
		}

		public UserAccount? GetUserAccountByUserName(string userName)
		{
			foreach (var userAccount in _userAccountList) {
                Console.WriteLine(userAccount.UserName);
            }
			return _userAccountList.FirstOrDefault(x => x.UserName == userName);
		}

        public bool RegisterUser(RegisterRequest user)
        {


            bool userExists = _userAccountList.Any(account => account.UserName == user.UserName || account.Email == user.Email);
            Console.WriteLine(userExists);
            if (userExists) return false;

            try { 
            _userAccountList.Add(
				new UserAccount { UserName = user.UserName, Password = user.Password, Role = "User", Email = user.Email }
				);
				return true;
            }catch (Exception e)
			{
                return false;
			}
        }

    }
}
