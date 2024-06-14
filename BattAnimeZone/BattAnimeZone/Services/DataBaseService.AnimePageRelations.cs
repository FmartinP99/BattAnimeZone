using BattAnimeZone.Shared.Models.AnimeDTOs;
using Microsoft.EntityFrameworkCore;


namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {

        public async Task<AnimeRelationsKeyDTO?> GetRelations(int mal_id)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {

                var query =  await (from r in _context.Relations
                             join p in _context.Animes on r.ParentId equals p.Mal_id
                             join c in _context.Animes on r.ChildId equals c.Mal_id
                             where r.ParentId == mal_id
                             group new { r, p, c } by new { p.Mal_id, p.TitleEnglish, p.TitleJapanese } into grp
                             select new AnimeRelationsKeyDTO
                             {
                                 Mal_id = grp.Key.Mal_id,
                                 TitleEnglish = grp.Key.TitleEnglish,
                                 TitleJapanese = grp.Key.TitleJapanese,
                                 Relations = grp.Select(x => new AnimeRelationDTO
                                 {
                                     Mal_id = x.c.Mal_id,
                                     TitleEnglish = x.c.TitleEnglish,
                                     TitleJapanese = x.c.TitleJapanese,
                                     MediaType = x.c.MediaType,
                                     Episodes = x.c.Episodes,
                                     Score = x.c.Score,
                                     Popularity = x.c.Popularity,
                                     Year = x.c.Year,
                                     ImageLargeWebpUrl = x.c.ImageLargeWebpUrl,
                                     RelationType = x.r.RelationType
                                 }).ToList()
                             })
                    .FirstOrDefaultAsync();

                return query;

            }
        }
    }
}
