﻿using BattAnimeZone.Shared.Models.Anime;
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
                if (anime.Title != "") default_distance = distance_metric.Distance(name, anime.TitleEnglish.ToLower());
                if (anime.TitleEnglish != "") eng_distance = distance_metric.Distance(name, anime.TitleJapanese.ToLower());
                if (anime.TitleJapanese != "") jp_distance = distance_metric.Distance(name, anime.TitleJapanese.ToLower());
                double min_distance = Math.Min(jp_distance, Math.Min(eng_distance, default_distance));
                if (min_distance < 0.4) distances.Add(anime.Mal_id, min_distance);
            }

            var sorted_distances = distances.OrderBy(kv => kv.Value);

            var top_n = sorted_distances.Take(n).Select(kv => kv.Key);

			List<Task<Anime>> tasks = top_n.Select(id => this.GetAnimeByID(id)).ToList();
			Anime[] fetchedAnimes = await Task.WhenAll(tasks);
            List<Anime> return_Animes = fetchedAnimes.ToList();

            return animeMapper.Map<List<AnimeSearchResultDTO>>(return_Animes);
        }
    }
}
