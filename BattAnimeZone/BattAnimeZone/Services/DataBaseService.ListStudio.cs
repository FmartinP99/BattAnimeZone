using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {
        public async Task<Dictionary<int, LiProductionEntityDTO>?> GetProductionEntitiesDTO()
        {
            using(var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                var query = await (from pe in _context.ProductionEntities
                                   join pet in _context.ProductionEntityTitles on pe.Id equals pet.ParentId
                                   group new { pe, pet } by new { pe.Id, pe.Favorites, pe.Count, pe.ImageUrl } into grp
                                   select new
                                   {
                                       Key = grp.Key.Id,
                                       Value = new LiProductionEntityDTO
                                       {
                                           Mal_id = grp.Key.Id,
                                           Favorites = grp.Key.Favorites,
                                           Count = grp.Key.Count,
                                           Image_url = grp.Key.ImageUrl,
                                           Titles = grp.Select(x => new ProductionEntityTitle
                                           {
                                               Type = x.pet.Type,
                                               Title = x.pet.Title
                                           }).ToList()
                                       }
                                   }).ToDictionaryAsync(x => x.Key, x => x.Value);
                return query;
            }
            
        }
    }
}
