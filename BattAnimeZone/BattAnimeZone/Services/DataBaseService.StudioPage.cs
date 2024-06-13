using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;
using Microsoft.EntityFrameworkCore;

namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {

        public async Task<ProductionEntityPageDTO>? GetAnimesForProdEnt(int mal_id)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                var prodEntQuery = (from pe in _context.ProductionEntities
                                    join pet in _context.ProductionEntityTitles on pe.Id equals pet.ParentId
                                    where pe.Id == mal_id
                                    group new { pe, pet} by new {pe.Id, pe.Url, pe.Favorites, pe.Established, pe.About, pe.Count, pe.ImageUrl} into grp
                                    select new ProductionEntity
                                    {
                                        Mal_id = grp.Key.Id,
                                        Url = grp.Key.Url,
                                        Favorites = grp.Key.Favorites,
                                        Established = grp.Key.Established,
                                        About = grp.Key.About,
                                        Count = grp.Key.Count,
                                        Image_url = grp.Key.ImageUrl,
                                        Titles = grp.Select(t => new ProductionEntityTitle
                                        {
                                            Type = t.pet.Type,
                                            Title = t.pet.Title
                                        }).ToList()
                                    }).FirstOrDefaultAsync();

                var producedAnimesQuery = (from ape in _context.AnimeProductionEntities
                                           join a in _context.Animes on ape.AnimeId equals a.Mal_id
                                           where ape.ProductionEntityId == mal_id && ape.Type == "P"
                                           select new AnimeStudioPageDTO
                                           {
                                               Mal_id = a.Mal_id,
                                               TitleEnglish = a.TitleEnglish,
                                               TitleJapanese = a.TitleJapanese,
                                               ImageLargeWebpUrl = a.ImageLargeWebpUrl
                                           }).ToListAsync();

                var licensedAnimesQuery = (from ape in _context.AnimeProductionEntities
                                           join a in _context.Animes on ape.AnimeId equals a.Mal_id
                                           where ape.ProductionEntityId == mal_id && ape.Type == "L"
                                           select new AnimeStudioPageDTO
                                           {
                                               Mal_id = a.Mal_id,
                                               TitleEnglish = a.TitleEnglish,
                                               TitleJapanese = a.TitleJapanese,
                                               ImageLargeWebpUrl = a.ImageLargeWebpUrl
                                           }).ToListAsync();

                var studioAnimesQuery = (from ape in _context.AnimeProductionEntities
                                         join a in _context.Animes on ape.AnimeId equals a.Mal_id
                                         where ape.ProductionEntityId == mal_id && ape.Type == "S"
                                         select new AnimeStudioPageDTO
                                         {
                                             Mal_id = a.Mal_id,
                                             TitleEnglish = a.TitleEnglish,
                                             TitleJapanese = a.TitleJapanese,
                                             ImageLargeWebpUrl = a.ImageLargeWebpUrl
                                         }).ToListAsync();

                await Task.WhenAll(prodEntQuery, producedAnimesQuery, licensedAnimesQuery, studioAnimesQuery);

                var result = new ProductionEntityPageDTO
                {
                    ProdEnt = await prodEntQuery,
                    ProducedAnimes = await producedAnimesQuery,
                    LicensedAnimes = await licensedAnimesQuery,
                    StudioAnimes = await studioAnimesQuery
                };
                return result;
            }

        }
    }
}
