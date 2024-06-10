using CsvHelper;
using Newtonsoft.Json;
using System.Globalization;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.Genre;
using BattAnimeZone.Utilities;
using AutoMapper;

namespace BattAnimeZone.Services
{
	public partial class AnimeService
	{

		//mapper for AnimeDTO's
		IMapper animeMapper;
		IMapper productionEntityMapper;
	


		private Dictionary<int, Anime> animes = new Dictionary<int, Anime> { };

        private Dictionary<int, ProductionEntity> productionEntities = new Dictionary<int, ProductionEntity> { };
        private Dictionary<int, HashSet<int>> animesPerProducerIdHash = new Dictionary<int, HashSet<int>> { };
        private Dictionary<int, HashSet<int>> animesPerLicensorIdHash = new Dictionary<int, HashSet<int>> { };
        private Dictionary<int, HashSet<int>> animesPerStudioIdHash = new Dictionary<int, HashSet<int>> { };

        private Dictionary<int, AnimeGenre> genres = new Dictionary<int, AnimeGenre> { };
		private Dictionary<int, HashSet<int>> animesPerGenreIdsHash = new Dictionary<int, HashSet<int>>();
		private Dictionary<int, int> animesPerGenreIdCount = new Dictionary<int, int>();
		private Dictionary<int, List<Anime>> animesPerGenre = new Dictionary<int, List<Anime>>();

		private List<string> mediaTypes = new List<string>();
		private Dictionary<string, HashSet<int>> animesPerMediaTypesHash = new Dictionary<string, HashSet<int>>();

        public void Initialize()
		{
            Console.WriteLine("AnimeService initialization has started!");
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileAnime>());
			animeMapper = config.CreateMapper();

            MapperConfiguration config2 = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileProductionEntity>());
            productionEntityMapper = config2.CreateMapper();

            FillAnimesAndMedia();
			FillProductionEntities();
			FillProductionIdHashes();
			FillGenres();
			FillAnimesPerGenreIdsHash();
			FillAnimesPerGenreIdCount();
			FillAnimesPerGenreList();
			FillAnimesPerMediaTypeIdsHas();

            Console.WriteLine("AnimeService initialization has ended!");
        }



		private void FillAnimesAndMedia()
		{
            using (var reader = new StreamReader("Files/mal_data_2022plus_subset.csv"))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{

					/*
					csv.GetField<int>("mal_id");
					csv.GetField("url");
					csv.GetField("title_english");
					csv.GetField("title_japanese");
					csv.GetField("type");
					csv.GetField("source");
					csv.GetField<float>("episodes");
					csv.GetField("status");
					csv.GetField("duration");
					csv.GetField("rating");
					csv.GetField<float>("score");
					csv.GetField<float>("scored_by");
					csv.GetField<float>("rank");
					csv.GetField<float>("popularity");
					csv.GetField<float>("members");
					csv.GetField<float>("favorites");
					csv.GetField("synopsis");
					csv.GetField("background");
					csv.GetField("season");
					csv.GetField<float>("year");
					new List<Producer> { new Producer() };
						new List<Licensor> { new Licensor() };
					new List<Studio> { new Studio() };
					new List<Genre> { new Genre() };
					new List<Theme> { new Theme() };
					new List<Relation> { new Relation() };
					new List<External> { new External() };
					new List<Streaming> { new Streaming() };
					csv.GetField("images.jpg.image_url");
					csv.GetField("images.jpg.small_image_url");
					csv.GetField("images.jpg.large_image_url");
					csv.GetField("images.webp.image_url");
					csv.GetField("images.webp.small_image_url");
					csv.GetField("images.webp.large_image_url");
					csv.GetField("trailer.url");
					csv.GetField("trailer.embed_url");
					csv.GetField("trailer.images.image_url");
					csv.GetField("trailer.images.small_image_url");
					csv.GetField("trailer.images.medium_image_url");
					csv.GetField("trailer.images.large_image_url");
					csv.GetField("trailer.images.maximum_image_url");
					csv.GetField<float>("aired.prop.from.day");
					csv.GetField<float>("aired.prop.from.month");
					csv.GetField<float>("aired.prop.from.year");
					csv.GetField<float>("aired.prop.to.day");
					csv.GetField<float>("aired.prop.to.month");
					csv.GetField<float>("aired.prop.to.year");
					csv.GetField("aired.string");
					*/

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
						Mal_id = csv.GetField<int>("mal_id"),
						//Url = csv.GetField("url"),
						Title = csv.GetField("title"),
						Title_english = csv.GetField("title_english"),
						Title_japanese = csv.GetField("title_japanese"),
						//Ttile_synonyms = title_synonyms,
						Media_type = csv.GetField("type"),
						Source = csv.GetField("source"),
						Episodes = (int)csv.GetField<float>("episodes"),
						Status = csv.GetField("status"),
						Duration = csv.GetField("duration"),
						Rating = csv.GetField("rating"),
						Score = csv.GetField<float>("score"),
						Scored_by = (int)csv.GetField<float>("scored_by"),
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
						//Externals = externals,
						//Streamings = streamings,
						//Image_jpg_url = csv.GetField("images.jpg.image_url"),
						//Image_small_jpg_url = csv.GetField("images.jpg.small_image_url"),
						//Image_large_jpg_url = csv.GetField("images.jpg.large_image_url"),
						//Image_webp_url = csv.GetField("images.webp.image_url"),
						//Image_small_webp_url = csv.GetField("images.webp.small_image_url"),
						Image_large_webp_url = csv.GetField("images.webp.large_image_url"),
						//Trailer_url = csv.GetField("trailer.url"),
						//Trailer_embed_url = csv.GetField("trailer.embed_url"),
						//Trailer_image_url = csv.GetField("trailer.images.image_url"),
						//Trailer_image_small_url = csv.GetField("trailer.images.small_image_url"),
						//Trailer_image_medium_url = csv.GetField("trailer.images.medium_image_url"),
						//Trailer_image_large_url = csv.GetField("trailer.images.large_image_url"),
						//Trailer_image_maximum_url = csv.GetField("trailer.images.maximum_image_url"),
						Aired_from_day = (int)csv.GetField<float>("aired.prop.from.day"),
						Aired_from_month = (int)csv.GetField<float>("aired.prop.from.month"),
						Aired_from_year = (int)csv.GetField<float>("aired.prop.from.year"),
						Aired_to_day = (int)csv.GetField<float>("aired.prop.to.day"),
						Aired_to_month = (int)csv.GetField<float>("aired.prop.to.month"),
						Aired_to_year = (int)csv.GetField<float>("aired.prop.to.year"),
						Aired_string = csv.GetField("aired.string"),
					};
					animes.Add(new_anime.Mal_id, new_anime);
                    if (!mediaTypes.Contains(new_anime.Media_type)) mediaTypes.Add(new_anime.Media_type);

				}
			}
		}



		public void FillProductionEntities()
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

		public void FillProductionIdHashes()
		{
			Dictionary<int, HashSet<int>> producerhashes = new Dictionary<int, HashSet<int>>();
			Dictionary<int, HashSet<int>> licensorhashes = new Dictionary<int, HashSet<int>>();
            Dictionary<int, HashSet<int>> studiohashes = new Dictionary<int, HashSet<int>>();

			foreach (int prodentkey in productionEntities.Keys) {
				producerhashes[prodentkey] = new HashSet<int>();
                licensorhashes[prodentkey] = new HashSet<int>();
                studiohashes[prodentkey] = new HashSet<int>();
			}

			foreach (Anime anim in this.animes.Values)
			{

				foreach(Entry prod in anim.Producers)
				{
					if(productionEntities.Keys.Contains(prod.Mal_id))
					producerhashes[prod.Mal_id].Add(anim.Mal_id);
				}

                foreach (Entry licensor in anim.Licensors)
                {
                    if (productionEntities.Keys.Contains(licensor.Mal_id))
                        licensorhashes[licensor.Mal_id].Add(anim.Mal_id);
                }

                foreach (Entry studio in anim.Studios)
                {
                    if (productionEntities.Keys.Contains(studio.Mal_id))
                        studiohashes[studio.Mal_id].Add(anim.Mal_id);
                }

            }

			this.animesPerProducerIdHash = producerhashes;
			this.animesPerLicensorIdHash = licensorhashes;
			this.animesPerStudioIdHash = studiohashes;
        }


        public void FillGenres()
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

		public void FillAnimesPerGenreIdsHash()
		{
			Dictionary<int, HashSet<int>> tempApG = new Dictionary<int, HashSet<int>>();

			foreach (var gen in this.genres)
			{
				tempApG.Add(gen.Key, new HashSet<int>());


			}

			foreach (var ani in this.animes)
			{
				foreach (var gen in ani.Value.Genres)
				{
					tempApG[gen.Mal_id].Add(ani.Value.Mal_id);
				}

				foreach (var gen in ani.Value.Themes)
				{
					tempApG[gen.Mal_id].Add(ani.Value.Mal_id);
				}
			}

			this.animesPerGenreIdsHash = tempApG;
		}

		public void FillAnimesPerGenreIdCount()
		{
            this.animesPerGenreIdCount = animesPerGenreIdsHash.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Count
                    );
        }



        public void FillAnimesPerGenreList()
		{
			Dictionary<int, List<Anime>> tempApG = new Dictionary<int, List<Anime>>();

			foreach (var ApGHash in this.animesPerGenreIdsHash)
			{

				var animes = ApGHash.Value.Select(ApGHashId => this.GetAnimeByIDSync(ApGHashId)).ToList();

				tempApG.Add(ApGHash.Key, animes);

			}

			this.animesPerGenre = tempApG;
		}

		public void FillAnimesPerMediaTypeIdsHas()
		{
            Dictionary<string, HashSet<int>> tempApM = new Dictionary<string, HashSet<int>>();

            foreach (var mtype in this.mediaTypes)
            {
                tempApM.Add(mtype, new HashSet<int>());
            }

            foreach (var ani in this.animes)
            {
               tempApM[ani.Value.Media_type].Add(ani.Value.Mal_id);
            }

			this.animesPerMediaTypesHash = tempApM;
        }

		public Task<Dictionary<int, Anime>> GetAllAnimes()
		{
			return Task.FromResult(this.animes);
		}

		public async Task<Anime> GetAnimeByID(int mal_id)
		{
			Anime? return_anime;
			if (this.animes.TryGetValue(mal_id, out return_anime)) return return_anime;
			return new Anime();
		}

		public Anime GetAnimeByIDSync(int mal_id)
		{
			Anime? return_anime;
			if (this.animes.TryGetValue(mal_id, out return_anime)) return return_anime;
			return new Anime();

		}

		public Task<Dictionary<int, AnimeGenre>> GetGenres()
		{
			return Task.FromResult(this.genres);
		}

        public async Task<ProductionEntity> GetProductionEntityById(int prodid)
        {
            ProductionEntity? prodent;
            if (this.productionEntities.TryGetValue(prodid, out prodent)) return prodent;
            return new ProductionEntity();
        }

        public Task<Dictionary<int, HashSet<int>>> GetAnimePerProducerIds()
        {
            return Task.FromResult(this.animesPerProducerIdHash);
        }

        public Task<Dictionary<int, HashSet<int>>> GetAnimePerLicensorIds()
        {
            return Task.FromResult(this.animesPerLicensorIdHash);
        }

        public Task<Dictionary<int, HashSet<int>>> GetAnimePerStudioIds()
        {
            return Task.FromResult(this.animesPerStudioIdHash);
        }


        public Task<HashSet<int>> GetAnimesOfProducer(int prodid)
        {
			HashSet<int>? animes;
            if (this.animesPerProducerIdHash.TryGetValue(prodid, out animes)) return Task.FromResult(animes);
			return Task.FromResult(new HashSet<int>());
        }

        public Task<HashSet<int>> GetAnimesOfLicensor(int prodid)
        {
            HashSet<int>? animes;
            if (this.animesPerLicensorIdHash.TryGetValue(prodid, out animes)) return Task.FromResult(animes);
            return Task.FromResult(new HashSet<int>());
        }

        public Task<HashSet<int>> GetAnimesOfStudio(int prodid)
        {
            HashSet<int>? animes;
            if (this.animesPerStudioIdHash.TryGetValue(prodid, out animes)) return Task.FromResult(animes);
            return Task.FromResult(new HashSet<int>());
        }

		public async Task<Dictionary<int, HashSet<int>>> GetAnimesPerGenreIds()
		{
			return this.animesPerGenreIdsHash;
        }

        public async Task<Dictionary<int, int>> GetAnimesPerGenreIdCount()
        {
            return this.animesPerGenreIdCount;
        }

        public async Task<Dictionary<string, HashSet<int>>> GetAnimesPerMediaTypeIds()
        {
            return this.animesPerMediaTypesHash;
        }

        public async Task<List<string>> GetMediaTypes()
		{
			return this.mediaTypes;
		}

	}		
}

