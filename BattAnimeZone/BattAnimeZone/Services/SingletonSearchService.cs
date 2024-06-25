using BattAnimeZone.DbContexts;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using F23.StringSimilarity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq.Dynamic.Core;

namespace BattAnimeZone.Services
{
    public class SingletonSearchService
    {

        private class AnimeTitleContainer
        {
            public string Title { get; set; } = string.Empty;
            public string TitleEnglish { get; set; } = string.Empty;
            public string TitleJapanese { get; set; } = string.Empty;
        }

        //the search service compares these titles to the search values instead of querying the anime titles every time from the database
        private Dictionary<int, AnimeTitleContainer> animes = new Dictionary<int, AnimeTitleContainer> { };

        //stores the similar anime id's for the past {recently_max_size} different searches.
        private Dictionary<string, int[]> recently_searched_strings = new Dictionary<string, int[]> { };

        int recently_max_size = 2000;


        private IDbContextFactory<AnimeDbContext> _dbContextFactory;
        public SingletonSearchService(IDbContextFactory<AnimeDbContext> dbContextFactory) {

            _dbContextFactory = dbContextFactory;
            FillOrRefreshAnimes();
        }


        public void FillOrRefreshAnimes()
        {
            using (var _context = _dbContextFactory.CreateDbContext())
            {
                var animeDictionary = _context.Animes
                 .Select(a => new { a.Mal_id, a.Title, a.TitleEnglish, a.TitleJapanese })
                 .ToDictionary(
                     a => a.Mal_id,
                     a => new AnimeTitleContainer
                     {
                         Title = a.Title,
                         TitleEnglish = a.TitleEnglish,
                         TitleJapanese = a.TitleJapanese
                     });

                this.animes = animeDictionary;
            }
        }

        public int[] GetSimilarAnimesForSearchResult(int n, string name)
        {
          
            List<int> foundAnimes = new List<int>();

            if (recently_searched_strings.ContainsKey(name.ToLower())) return recently_searched_strings[name.ToLower()];

           
            foreach (KeyValuePair<int, AnimeTitleContainer> anime in this.animes)
            {

                bool substring1 = anime.Value.TitleJapanese.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0;
                bool substring2 = anime.Value.TitleEnglish.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0;
                bool substring3 = anime.Value.Title.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0;


                if (substring1 || substring2 || substring3) foundAnimes.Add(anime.Key);
                if (foundAnimes.Count() >= n) break;
            }
          

            //this can die if there r multiple concurrent workers trying to remove from the dictionary. need to do something about it later.
            // update: hope lock prevents it 
            lock (recently_searched_strings)
            {
                if (recently_searched_strings.Count >= recently_max_size)
                {
                    string key_to_Remove = recently_searched_strings.Keys.ElementAt(0);
                    try
                    {
                        recently_searched_strings.Remove(key_to_Remove);
                        recently_searched_strings.Add(name.ToLower(), foundAnimes.ToArray());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception while removing random key: {ex.Message}");
                    }

                }
                else
                {
                    recently_searched_strings.Add(name.ToLower(), foundAnimes.ToArray());
                }
            }
           
            return foundAnimes.ToArray();
        }
    }
}
