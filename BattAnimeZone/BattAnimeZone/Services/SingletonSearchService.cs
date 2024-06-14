using BattAnimeZone.DbContexts;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using F23.StringSimilarity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        private Dictionary<int, AnimeTitleContainer> animes = new Dictionary<int, AnimeTitleContainer> { };

        private Dictionary<string, List<int>> recently_searched_distances = new Dictionary<string, List<int>> { };

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

        public List<int> GetSimilarAnimesForSearchResult(int n, string name)
        {
            name = name.ToLower();
            Dictionary<int, double> distances = new Dictionary<int, double>();

            if (recently_searched_distances.ContainsKey(name)) return recently_searched_distances[name];

            var distance_metric = new JaroWinkler();
            foreach (KeyValuePair<int, AnimeTitleContainer> anime in this.animes)
            {
                double default_distance = double.MaxValue;
                double eng_distance = double.MaxValue;
                double jp_distance = double.MaxValue;
                if (anime.Value.Title != "") default_distance = distance_metric.Distance(name, anime.Value.TitleEnglish.ToLower());
                if (anime.Value.TitleEnglish != "") eng_distance = distance_metric.Distance(name, anime.Value.TitleJapanese.ToLower());
                if (anime.Value.TitleJapanese != "") jp_distance = distance_metric.Distance(name, anime.Value.TitleJapanese.ToLower());
                double min_distance = Math.Min(jp_distance, Math.Min(eng_distance, default_distance));
                if (min_distance < 0.4) distances.Add(anime.Key, min_distance);
            }
            var top_n_ids = distances.OrderBy(kv => kv.Value).Take(n).Select(kv => kv.Key).ToList();

            //this can die if there r multiple concurrent workers trying to remove from the dictionary. need to do something about it later.
            // update: hope lock prevents it 
            lock (recently_searched_distances)
            {
                if (recently_searched_distances.Count >= recently_max_size)
                {
                    string key_to_Remove = recently_searched_distances.Keys.ElementAt(0);
                    try
                    {
                        recently_searched_distances.Remove(key_to_Remove);
                        recently_searched_distances.Add(name, top_n_ids);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception while removing random key: {ex.Message}");
                    }

                }
                else
                {
                    recently_searched_distances.Add(name, top_n_ids);
                }
            }
           
            return top_n_ids;
        }
    }
}
