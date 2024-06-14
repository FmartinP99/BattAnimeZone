using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.DatabaseModels;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel;
using BattAnimeZone.Client.Pages;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {
        public async Task<AnimePageDTO?> GetAnimePageDTOByID(int mal_id)
        {

            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {

                var q1 = (from ag in _context.AnimeGenres
                              join a in _context.Animes on ag.AnimeId equals a.Mal_id
                              join g in _context.Genres on ag.GenreId equals g.Mal_id
                              where a.Mal_id == mal_id
                              group new { ag, a, g } by new
                              {
                                  ag.AnimeId,
                                  a.Title,
                                  a.TitleEnglish,
                                  a.TitleJapanese,
                                  a.MediaType,
                                  a.Episodes,
                                  a.Status,
                                  a.Duration,
                                  a.Score,
                                  a.Rank,
                                  a.Popularity,
                                  a.Synopsis,
                                  a.Background,
                                  a.Season,
                                  a.Year,
                                  a.ImageLargeWebpUrl,
                                  a.AiredString

                              } into grp
                              select new
                              {
                                  AnimeId = grp.Key.AnimeId,
                                  Title = grp.Key.Title,
                                  TitleEnglish = grp.Key.TitleEnglish,
                                  TitleJapanese = grp.Key.TitleJapanese,
                                  MediaType = grp.Key.MediaType,
                                  Episodes = grp.Key.Episodes,
                                  Status = grp.Key.Status,
                                  Duration = grp.Key.Duration,
                                  Score = grp.Key.Score,
                                  Rank = grp.Key.Rank,
                                  Popularity = grp.Key.Popularity,
                                  Synopsis = grp.Key.Synopsis,
                                  Background = grp.Key.Background,
                                  Season = grp.Key.Season,
                                  Year = grp.Key.Year,
                                  ImageLargeWebpUrl = grp.Key.ImageLargeWebpUrl,
                                  AiredString = grp.Key.AiredString,
                                  Genres = grp.Where(x => !x.ag.IsTheme).Select(x => new Entry
                                  {
                                      Mal_id = x.ag.GenreId,
                                      Name = x.g.Name,
                                  }).ToList(),

                                  Themes = grp.Where(x => x.ag.IsTheme).Select(x =>
                                  new Entry
                                  {
                                      Mal_id = x.ag.GenreId,
                                      Name = x.g.Name,
                                  }
                                  ).ToList()
                              })
                               .AsSplitQuery()
                               .AsNoTracking()
                               .FirstOrDefault();


                var q2 = (from ap in _context.AnimeProductionEntities
                             join a in _context.Animes on ap.AnimeId equals a.Mal_id
                             join p in _context.ProductionEntities on ap.ProductionEntityId equals p.Id
                             join pt in _context.ProductionEntityTitles on p.Id equals pt.ParentId
                              where a.Mal_id == mal_id
                              group new { ap, a, p, pt } by new {
                                 a.Mal_id,
                             } into grp
                             select new
                             {
                                 mal_id = grp.Key.Mal_id,
                                 Studios = grp.Where(x => x.ap.Type == "S" && x.pt.Type == "Default").Select(x => new Entry
                                 {
                                     Mal_id = x.ap.ProductionEntityId,
                                     Name = x.pt.Title
                                 }).ToList(),
                                 Licensors = grp.Where(x => x.ap.Type == "L" && x.pt.Type == "Default").Select(x => new Entry
                                 {
                                     Mal_id = x.ap.ProductionEntityId,
                                     Name = x.pt.Title
                                 }).ToList(),
                                 Producers = grp.Where(x => x.ap.Type == "P" && x.pt.Type == "Default").Select(x => new Entry
                                 {
                                     Mal_id = x.ap.ProductionEntityId,
                                     Name = x.pt.Title
                                 }).ToList(),

                             })
                                 .AsSplitQuery()
                                 .AsNoTracking()
                                 .FirstOrDefault();

                if (q1 == null) return null;

                var returnDTO = new AnimePageDTO
                {
                    Mal_id = q1.AnimeId,
                    Title = q1.Title,
                    TitleEnglish = q1.TitleEnglish,
                    TitleJapanese = q1.TitleJapanese,
                    MediaType = q1.MediaType,
                    Episodes = q1.Episodes,
                    Status = q1.Status,
                    Duration = q1.Duration,
                    Score = q1.Score,
                    Rank = q1.Rank,
                    Popularity = q1.Popularity,
                    Synopsis = q1.Synopsis,
                    Background = q1.Background,
                    Season = q1.Season,
                    Year = q1.Year,
                    ImageLargeWebpUrl = q1.ImageLargeWebpUrl,
                    AiredString = q1.AiredString,
                    Genres = q1.Genres,
                    Themes = q1.Themes,
                    Studios = q2?.Studios,
                    Licensors = q2?.Licensors,
                    Producers = q2?.Producers
                };

                return returnDTO;
                }
            }
        }
}
