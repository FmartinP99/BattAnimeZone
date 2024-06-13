using BattAnimeZone.DbContexts;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BattAnimeZone.Utilities;
using System.ComponentModel;
using BattAnimeZone.Shared.Models.GenreDTOs;
using BattAnimeZone.DatabaseModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {

        private IDbContextFactory<AnimeDbContext> _dbContextFactory;
        private static IMapper dataBaseMapper;

        public DataBaseService(IDbContextFactory<AnimeDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;

            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<MappringProfileDataBase>());
            dataBaseMapper = config.CreateMapper();
        }



        //THIS FUNCTION IS FOR TESTING PURPOSES
        private async Task PrintProperties(object obj, int depth = 0)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                if (value == null) continue;

                if (value is ICollection list)
                {
                    await Console.Out.WriteLineAsync(name);
                    foreach (var ev in list)
                    {
                        int newdepth = depth + 1;

                        await PrintProperties(ev, newdepth);
                    }
                }
                else
                {
                    string baseString = String.Concat(Enumerable.Repeat("\t", depth));
                    await Console.Out.WriteLineAsync($"{baseString}{name}  --  {value}");
                }

            }

        }


        public async Task<List<String>>? GetDistinctMediaTypes()
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                List<String>? distinctMediaTypes = await _context.DistinctMediaTypes.Select(x => x.mediaType).ToListAsync();
                return distinctMediaTypes;
            }
        }

        public async Task<List<AnimeGenreDTO>>? GetGenres()
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                List<AnimeGenreDTO>? animeGenres = await _context.Genres.Select(ag => new AnimeGenreDTO
                {
                    Mal_id = ag.Mal_id,
                    Name = ag.Name
                })
                .ToListAsync();

                return animeGenres;
            }
        }

        public async Task<Dictionary<int, int>> GetAnimesPerGenreIdCount()
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                Dictionary<int, int> genreAnimeCount = await _context.AnimeGenres
                .GroupBy(ag => ag.GenreId)
                .Select(g => new { GenreId = g.Key, AnimeCount = g.Count() })
                .ToDictionaryAsync(g => g.GenreId, g => g.AnimeCount);

                return genreAnimeCount;
            }
        }
    }
}
