using BattAnimeZone.DatabaseModels;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core.Tokenizer;

namespace BattAnimeZone.Services.DataBase
{
    public partial class UserAccountService
    {

        public async Task<ProfilePageDTO?> GetProfileByUserName(string UserName)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                var db_User = await _context.UserAccounts.Where(x => x.UserName == UserName).FirstOrDefaultAsync();
                if (db_User == null) return null;

                var query = await (from au in _context.AnimeUserModels
                                   join u in _context.UserAccounts on au.UserId equals u.Id
                                   join a in _context.Animes on au.AnimeId equals a.Mal_id
                                   where u.UserName == UserName && au.Status != null
                                   select new AnimeProfilePageDTO()
                                   {
                                      Mal_id = a.Mal_id,
                                      Title = a.Title,
                                      MediaType = a.MediaType,
                                      Episodes = a.Episodes,
                                      Status = a.Status,
                                      Rating = a.Rating,
                                      Score = a.Score,
                                      Popularity = a.Popularity,
                                      Year = a.Year,
                                      ImageLargeWebpUrl = a.ImageLargeWebpUrl,
                                      UserStatus = au.Status,
                                      UserRating = au.Rating,
                                      UserFavorite = au.favorite,
                                      Date = au.Date,

                                   }).ToListAsync();

                var return_data = new ProfilePageDTO
                {
                    Name = db_User.UserName,
                    RegisteredAt = db_User.RegisteredAt,
                    Animes = query
                };

                return return_data;
            }
        }

     }
}

