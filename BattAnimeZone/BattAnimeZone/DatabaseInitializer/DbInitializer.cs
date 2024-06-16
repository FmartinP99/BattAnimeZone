using BattAnimeZone.Client.Pages;
using BattAnimeZone.DbContexts;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.Genre;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Utilities;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace BattAnimeZone.DatabaseInitializer
{
    public class DbInitializer
	{
		private  IConfiguration _configuration;
		public void Initialize(IConfiguration configuration, IDbContextFactory<AnimeDbContext> _contextFactory)
		{
			_configuration = configuration;

			using (var _context = _contextFactory.CreateDbContext())
			{
				bool db_exists = (File.Exists(_configuration.GetConnectionString("DatabasePath")));

				if (!db_exists)
				{
					if (!File.Exists(_configuration.GetConnectionString("DatabaseInitFilePath")))
					{
                        Console.WriteLine("DB Initializer file does not exists!");
						return;
                    }
					string sqlCommands = File.ReadAllText(_configuration.GetConnectionString("DatabaseInitFilePath"));
					Console.WriteLine("Reading SQL initialization script!");
					_context.Database.ExecuteSqlRaw(sqlCommands);
                    Console.WriteLine("Database has been created!");
                }

				if (_context.Animes.Any()) {
					Console.WriteLine("Database isn't empty! Writing some AnimeGenres for test.\n");
					var query = (from ag in _context.AnimeGenres
								 join a in _context.Animes on ag.AnimeId equals a.Mal_id
								 join g in _context.Genres on ag.GenreId equals g.Mal_id
                                 group new { ag, a, g } by new { ag.AnimeId, a.Title } into grp
								 select new
								 {
									 AnimeId = grp.Key.AnimeId,
									 AnimeTitle = grp.Key.Title,
									 Genres = grp.Where(x => !x.ag.IsTheme).Select(x => x.g.Name).ToList(),
									 Themes = grp.Where(x => x.ag.IsTheme).Select(x => x.g.Name).ToList()
								 })
								 .AsSplitQuery()
								 .AsNoTracking()
									.Take(3)
									.ToList();

					foreach(var item in query)
					{
						Console.WriteLine($"AnimeId: {item.AnimeId}, AnimeTitle: {item.AnimeTitle} \n" + "\tGenres: " + string.Join(", ", item.Genres) + "\n" + "\tThemes: " + string.Join(", ", item.Themes));
					}
                    Console.WriteLine("\n");
                    return; 
				}
			}
			Console.WriteLine("Database was empty! Now filling the database. This might take a while!\n");
			Dictionary<int, Anime> animes = new Dictionary<int, Anime> { };
			Dictionary<int, ProductionEntity> productionEntities = new Dictionary<int, ProductionEntity> { };
			Dictionary<int, AnimeGenre> genres = new Dictionary<int, AnimeGenre> { };

			FillAnimes(animes);
			FillProductionEntities(productionEntities);
			FillGenres(genres);
			FillDatabase(_contextFactory, animes, productionEntities, genres);
			

		}

		private void FillAnimes(Dictionary<int, Anime> animes)
		{
			using (var reader = new StreamReader("Files/mal_data_full_sfw_updated_20240615_2021plus_subset.csv"))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
                    List<string>? title_synonyms = JsonConvert.DeserializeObject<List<string>>(csv.GetField("title_synonyms"));
					List<Entry>? producers = JsonConvert.DeserializeObject<List<Entry>>(csv.GetField("producers"));
					List<Entry>? licensors = JsonConvert.DeserializeObject<List<Entry>>(csv.GetField("licensors"));
					List<Entry>? studios = JsonConvert.DeserializeObject<List<Entry>>(csv.GetField("studios"));
					List<Entry>? genres = JsonConvert.DeserializeObject<List<Entry>>(csv.GetField("genres"));
					List<Entry>? themes = JsonConvert.DeserializeObject<List<Entry>>(csv.GetField("themes"));

					string? relationship_str = csv.GetField("relations");
					relationship_str = JsonStringProcessor.DecodeJSString(relationship_str);
					List<Relations>? relations = JsonConvert.DeserializeObject<List<Relations>>(relationship_str);

					List<External>? externals = JsonConvert.DeserializeObject<List<External>>(csv.GetField("external"));
					List<Streaming>? streamings = JsonConvert.DeserializeObject<List<Streaming>>(csv.GetField("streaming"));

					Anime new_anime = new Anime
					{
						Mal_id = (int)csv.GetField<float>("mal_id"),
						Url = csv.GetField("url"),
						Title = csv.GetField("title"),
						TitleEnglish = csv.GetField("title_english"),
						TitleJapanese = csv.GetField("title_japanese"),
						TtileSynonyms = title_synonyms,
						MediaType = csv.GetField("type"),
						Source = csv.GetField("source"),
						Episodes = (int)csv.GetField<float>("episodes"),
						Status = csv.GetField("status"),
						Duration = csv.GetField("duration"),
						Rating = csv.GetField("rating"),
						Score = csv.GetField<float>("score"),
						ScoredBy = (int)csv.GetField<float>("scored_by"),
						Rank = (int)csv.GetField<float>("rank"),
						Popularity = (int)csv.GetField<float>("popularity"),
						Members = (int)csv.GetField<float>("members"),
						Favorites = (int)csv.GetField<float>("favorites"),
						Synopsis = csv.GetField("synopsis"),
						Background = csv.GetField("background"),
						Season = csv.GetField("season"),
						Year = (int)csv.GetField<float>("aired.prop.from.year"),
						Producers = producers,
						Licensors = licensors,
						Studios = studios,
						Genres = genres,
						Themes = themes,
						Relations = relations,
						Externals = externals,
						Streamings = streamings,
						ImageJpgUrl = csv.GetField("images.jpg.image_url"),
						ImageSmallJpgUrl = csv.GetField("images.jpg.small_image_url"),
						ImageLargeJpgUrl = csv.GetField("images.jpg.large_image_url"),
						ImageWebpUrl = csv.GetField("images.webp.image_url"),
						ImageSmallWebpUrl = csv.GetField("images.webp.small_image_url"),
						ImageLargeWebpUrl = csv.GetField("images.webp.large_image_url"),
						TrailerUrl = csv.GetField("trailer.url"),
						TrailerEmbedUrl = csv.GetField("trailer.embed_url"),
						TrailerImageUrl = csv.GetField("trailer.images.image_url"),
						TrailerImageSmallUrl = csv.GetField("trailer.images.small_image_url"),
						TrailerImageMediumUrl = csv.GetField("trailer.images.medium_image_url"),
						TrailerImageLargeUrl = csv.GetField("trailer.images.large_image_url"),
						TrailerImageMaximumUrl = csv.GetField("trailer.images.maximum_image_url"),
						AiredFromDay = (int)csv.GetField<float>("aired.prop.from.day"),
						AiredFromMonth = (int)csv.GetField<float>("aired.prop.from.month"),
						AiredFromYear = (int)csv.GetField<float>("aired.prop.from.year"),
						AiredToDay = (int)csv.GetField<float>("aired.prop.to.day"),
						AiredToMonth = (int)csv.GetField<float>("aired.prop.to.month"),
						AiredToYear = (int)csv.GetField<float>("aired.prop.to.year"),
						AiredString = csv.GetField("aired.string"),
					};
					animes.Add(new_anime.Mal_id, new_anime);
				}
			}
		}

		public void FillProductionEntities(Dictionary<int, ProductionEntity> productionEntities)
		{
			using (var reader = new StreamReader("Files/mal_producers.csv"))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					List<ProductionEntityTitle>? producerTitle = JsonConvert.DeserializeObject<List<ProductionEntityTitle>>(csv.GetField("titles"));

					ProductionEntity new_producer = new ProductionEntity
					{
						Mal_id = csv.GetField<int>("mal_id"),
						Url = csv.GetField("url"),
						Titles = producerTitle,
						Favorites = csv.GetField<int>("favorites"),
						Established = csv.GetField("established"),
						About = csv.GetField("about"),
						Count = csv.GetField<int>("count"),
						Image_url = csv.GetField("images.jpg.image_url"),

					};
					productionEntities.Add(new_producer.Mal_id, new_producer);

				}
			}
		}


		public void FillGenres(Dictionary<int, AnimeGenre> genres)
		{
			using (var reader = new StreamReader("Files/mal_anime_genres.csv"))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					AnimeGenre new_genre = new AnimeGenre
					{
						Mal_id = csv.GetField<int>("mal_id"),
						Name = csv.GetField("name"),
						Url = csv.GetField("url"),
						Count = csv.GetField<int>("count"),

					};
					genres.Add(new_genre.Mal_id, new_genre);

				}
			}
		}






		public async void FillDatabase(IDbContextFactory<AnimeDbContext> _contextFactory, Dictionary<int, Anime> animes, Dictionary<int, ProductionEntity> productionEntities, Dictionary<int, AnimeGenre> genres)
		{
			Console.WriteLine("step filldatabase");
			var animesData = animes.Select(a => a.Value).ToList();
			var prodentsData = productionEntities.Select(a => a.Value).ToList();
			var genresData = genres.Select(a => a.Value).ToList();

			CsvToDataBaseHandler _csvToDataBaseHandler = new CsvToDataBaseHandler(_contextFactory);

			Console.WriteLine("queried animes");
			await _csvToDataBaseHandler.SaveAnimesToDatabase(animesData);
			Console.WriteLine("saved animes\n");


			Console.WriteLine("queried relations");
			await _csvToDataBaseHandler.SaveRelationsToDatabase(animes);
			Console.WriteLine("saved relations\n");

			Console.WriteLine("queried externals");
			await _csvToDataBaseHandler.SaveExternalsToDatabase(animesData);
			Console.WriteLine("saved externals\n");

			Console.WriteLine("queried streamings");
			await _csvToDataBaseHandler.SaveStreamingsToDatabase(animesData);
			Console.WriteLine("saved streamings\n");

			Console.WriteLine("queried animestreamings");
			await _csvToDataBaseHandler.SaveAnimeStreamingsToDatabase(animesData);
			Console.WriteLine("saved animestreamings\n");

			Console.WriteLine("queried productionentities");
			await _csvToDataBaseHandler.SaveProductionEntitiesToDatabase(prodentsData);
			Console.WriteLine("saved productionentities\n");

			Console.WriteLine("queried productionentitytitles");
			await _csvToDataBaseHandler.SaveProductionEntityTitlesToDatabase(prodentsData);
			Console.WriteLine("saved productionentitytitles\n");

			Console.WriteLine("queried animeproductionentities");
			await _csvToDataBaseHandler.SaveAnimeProductionEntitiesToDatabase(animesData);
			Console.WriteLine("saved animeproductionentities\n");

			Console.WriteLine("creating user & admin accounts for testing purposes!");
            await _csvToDataBaseHandler.CreateUsers();
            Console.WriteLine("accounts have been created!\n");

			Console.WriteLine("queried animeusers");
			await _csvToDataBaseHandler.SaveAnimeUsersToDatabase();
			Console.WriteLine("saved animeusers\n");

			Console.WriteLine("queried genres");
			await _csvToDataBaseHandler.SaveGenresToDatabase(genresData);
			Console.WriteLine("saved genres\n");

			Console.WriteLine("queried animegenres");
			await _csvToDataBaseHandler.SaveAnimeGenresToDatabase(animesData);
			Console.WriteLine("saved animegenres\n");


            Console.WriteLine("queried distinctmediatypes");
            await _csvToDataBaseHandler.SaveDistinctMediaTypes();
            Console.WriteLine("saved distinctmediatypes\n");

			Console.WriteLine("queried distinctyears");
			await _csvToDataBaseHandler.SaveDistinctYears();
			Console.WriteLine("saved distinctyears\n");

		}

	}
}
