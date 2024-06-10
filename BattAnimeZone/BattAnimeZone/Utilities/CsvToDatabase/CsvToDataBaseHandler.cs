using BattAnimeZone.DbContexts;
using CsvHelper;
using System.Globalization;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.DatabaseModels.Anime;
using BattAnimeZone.Shared.DatabaseModels.Relation;
using BattAnimeZone.Shared.DatabaseModels.External;
using BattAnimeZone.Shared.DatabaseModels.Streaming;
using BattAnimeZone.Shared.DatabaseModels.AnimeStreaming;
using CsvHelper.Configuration;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System;

namespace BattAnimeZone.Utilities.CsvToDatabase
{
    public class CsvToDataBaseHandler
    {

        private readonly AnimeDbContext _context;

        public CsvToDataBaseHandler(AnimeDbContext context)
        {
            _context = context;
        }

        public AnimeModel MapAnimeCsvToAnime(Anime anime)
        {
            return new AnimeModel
            {
                Mal_id = anime.Mal_id,
                Title = anime.Title,
                TitleEnglish = anime.TitleEnglish,
                TitleJapanese = anime.TitleJapanese,
                TitleSynonyms = string.Join("|!!|", anime.TtileSynonyms),
                MediaType = anime.MediaType,
                Source = anime.Source,
                Episodes = anime.Episodes,
                Duration = anime.Duration,
                Rating = anime.Rating,
                Score = anime.Score,
                ScoredBy = anime.ScoredBy,
                Rank = anime.Rank,
                Popularity = anime.Popularity,
                Members = anime.Members,
                Favorites = anime.Favorites,
                Synopsis = anime.Synopsis,
                Background = anime.Background,
                Season = anime.Season,
                Year = anime.Year,
                ImageJpgUrl = anime.ImageJpgUrl,
                ImageSmallJpgUrl = anime.ImageSmallJpgUrl,
                ImageLargeJpgUrl = anime.ImageLargeJpgUrl,
                ImageWebpUrl = anime.ImageWebpUrl,
                ImageSmallWebpUrl = anime.ImageSmallWebpUrl,
                ImageLargeWebpUrl = anime.ImageLargeWebpUrl,
                TrailerUrl = anime.TrailerUrl,
                TrailerEmbedUrl = anime.TrailerEmbedUrl,
                TrailerImageUrl = anime.TrailerImageUrl,
                TrailerImageSmallUrl = anime.TrailerImageSmallUrl,
                TrailerImageMediumUrl = anime.TrailerImageMediumUrl,
                TrailerImageLargeUrl = anime.TrailerImageLargeUrl,
                TrailerImageMaximumUrl = anime.TrailerImageMaximumUrl,
                AiredFromDay = anime.AiredFromDay,
                AiredFromMonth = anime.AiredFromMonth,
                AiredFromYear = anime.AiredFromYear,
                AiredToDay = anime.AiredToDay,
                AiredToMonth = anime.AiredToMonth,
                AiredToYear = anime.AiredToYear,
                AiredString = anime.AiredString
            };
        }

        public async Task SaveAnimesToDatabase(List<Anime> animes)
        {

            List<AnimeModel> animeModels = new List<AnimeModel>();

            foreach(Anime an in animes)
            {
                AnimeModel animeModel = this.MapAnimeCsvToAnime(an);
                animeModels.Add(animeModel);
            }
            await _context.Animes.AddRangeAsync(animeModels);
            await _context.SaveChangesAsync();
		}





		public static bool AreDictionariesEqual(Dictionary<Dictionary<int, int>, string> dict1, Dictionary<Dictionary<int, int>, string> dict2)
		{
			
			if (dict1.Count != dict2.Count)
				return false;

			foreach (var kvp in dict1)
			{
				if (!dict2.TryGetValue(kvp.Key, out string value) || value != kvp.Value)
					return false;
			}

			return true;
		}





		public async Task SaveRelationsToDatabase(Dictionary<int, Anime> animes)
        {
            List<RelationModel> relationModels = new List<RelationModel>();


            List<Dictionary<Dictionary<int, int>, string>> visited = new List<Dictionary<Dictionary<int, int>, string>>();

			HashSet<int> existingAnimeIds = animes.Select(pair => pair.Key).ToHashSet();


			foreach (Anime anime in animes.Values)
            {
                foreach (Relations relations in anime.Relations)
                {
                    foreach (Entry entry in relations.Entry)
                    {
                        if (entry.Type != "anime") continue;

						var dictionary = new Dictionary<Dictionary<int, int>, string> {{ new Dictionary<int, int> { { 1, anime.Mal_id }, { 2, entry.Mal_id } }, relations.Relation },};
						bool alreadyVisited = visited.Any(d => AreDictionariesEqual(d, dictionary));
						if (alreadyVisited)
                        {
                            await Console.Out.WriteLineAsync($"{relations.Relation} -  parent: {anime.Mal_id} child: {entry.Mal_id}");
                            continue;
                        }

                        bool exists = existingAnimeIds.Contains(anime.Mal_id) && existingAnimeIds.Contains(entry.Mal_id);
                        if (!exists) continue;

						visited.Add(dictionary);
                        
                        RelationModel relmod = new RelationModel
                        {
                            ParentId = anime.Mal_id,
                            ChildId = entry.Mal_id,
							RelationType = relations.Relation,
                        };

                        relationModels.Add(relmod);

                    }
                }
            }
			await _context.Relations.AddRangeAsync(relationModels);
			await _context.SaveChangesAsync();
		}


		public async Task SaveExternalsToDatabase(List<Anime> animes)
		{

			List<ExternalModel> externalModels = new List<ExternalModel>();


            foreach (Anime an in animes)
            {

                foreach (External ext in an.Externals) {

                    ExternalModel extm = new ExternalModel
                    {
                        Name = ext.Name,
                        Url = ext.Url,
                        AnimeId = an.Mal_id
                    };
                    externalModels.Add(extm);
                }
            }


				await _context.Externals.AddRangeAsync(externalModels);
			    await _context.SaveChangesAsync();
		}

		public async Task SaveStreamingsToDatabase(List<Anime> animes)
		{

			List<StreamingModel> streamingModels = new List<StreamingModel>();

			HashSet<string> visited = new HashSet<string>();

			foreach (Anime an in animes)
			{

				foreach (Streaming st in an.Streamings)
				{
					string key = $"{st.Name}|!!|{st.Url}";
					if (visited.Contains(key)) continue;
					visited.Add(key);

					StreamingModel stm = new StreamingModel
					{
						Name = st.Name,
						Url = st.Url,
					};
					streamingModels.Add(stm);
				}
			}


			await _context.Streamings.AddRangeAsync(streamingModels);
			await _context.SaveChangesAsync();
		}



		public async Task SaveAnimeStreamingsToDatabase(List<Anime> animes)
		{

			List<AnimeStreamingModel> animeStreamingModels = new List<AnimeStreamingModel>();

            HashSet<string> visited = new HashSet<string>();

            foreach(Anime anime in animes)
            {
                foreach(Streaming streaming in anime.Streamings)
				{
					StreamingModel? streamingModel = _context.Streamings.FirstOrDefault(s => s.Name == streaming.Name && s.Url == streaming.Url);
                    if (streamingModel == null) continue;

                    int streamingId = streamingModel.Id;

                    string key = $"{streamingId}|!!|{anime.Mal_id}";
                    if (visited.Contains(key)) continue;  
                    visited.Add(key);

                    AnimeStreamingModel astm = new AnimeStreamingModel
                    {
                        AnimeId = anime.Mal_id,
                        StreamingId = streamingId
                    };

					animeStreamingModels.Add(astm);

				}
			}

			await _context.AnimeStreamings.AddRangeAsync(animeStreamingModels);
			await _context.SaveChangesAsync();
		}


	}
}
