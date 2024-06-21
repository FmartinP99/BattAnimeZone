using BattAnimeZone.DatabaseModels;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.EntityFrameworkCore;

namespace BattAnimeZone.Services
{
    public partial class UserAccountService
    {

        public async Task<Dictionary<string, string?>> GetProfileByUserName(string UserName)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                UserAccountModel? db_User = await _context.UserAccounts.Where(x => x.UserName == UserName).FirstOrDefaultAsync();
                if (db_User == null) return null;

                Dictionary<string, string?> returnUser = new Dictionary<string, string?> {
                    {
                        "userName", db_User.UserName
                    } };

                return returnUser;
            }

        }
    }
}
