using BattAnimeZone.Authentication;

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
			return _userAccountList.FirstOrDefault(x => x.UserName == userName);
		}

	}
}
