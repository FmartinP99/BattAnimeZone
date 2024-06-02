using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using F23.StringSimilarity;

namespace BattAnimeZone.Services
{
    public partial class AnimeService
    {
        public async Task<List<AnimeSearchResultDTO>> GetSimilarAnimesForSearchResult(int n, string name)
        {
            name = name.ToLower();

            var distance_metric = new JaroWinkler();

            Dictionary<int, double> distances = new Dictionary<int, double>();

            foreach (Anime anime in this.animes.Values)
            {
                double default_distance = double.MaxValue;
                double eng_distance = double.MaxValue;
                double jp_distance = double.MaxValue;
                if (anime.Title != "") default_distance = distance_metric.Distance(name, anime.Title_english.ToLower());
                if (anime.Title_english != "") eng_distance = distance_metric.Distance(name, anime.Title_japanese.ToLower());
                if (anime.Title_japanese != "") jp_distance = distance_metric.Distance(name, anime.Title_japanese.ToLower());
                double min_distance = Math.Min(jp_distance, Math.Min(eng_distance, default_distance));
                if (min_distance < 0.7) distances.Add(anime.Mal_id, min_distance);
            }

            var sorted_distances = distances.OrderBy(kv => kv.Value);
            var top_n = sorted_distances.Take(n).Select(kv => kv.Key);

            List<Anime> return_Animes = new List<Anime>();
            foreach (int id in top_n)
            {
                return_Animes.Add(await this.GetAnimeByID(id));
            }
            return animeMapper.Map<List<AnimeSearchResultDTO>>(return_Animes);
        }
    }
}
