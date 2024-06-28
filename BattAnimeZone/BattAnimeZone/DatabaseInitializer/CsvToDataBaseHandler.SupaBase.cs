using BattAnimeZone.DatabaseModels.SQliteDatabaseModels;
using BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.Genre;
using BattAnimeZone.Shared.Models.ProductionEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BattAnimeZone.DatabaseInitializer
{
    public partial class CsvToDataBaseHandler
    {


        Dictionary<int, AnimeSupabaseModel> supaBaseAnimes = new Dictionary<int, AnimeSupabaseModel>();
        Dictionary<int, StreamingSupabaseModel> supaBaseStreamings = new Dictionary<int, StreamingSupabaseModel>();
        Dictionary<int, ProductionEntitySupabaseModel> supaBaseProdEnts = new Dictionary<int, ProductionEntitySupabaseModel>();
        Dictionary<int, GenreSupabaseModel> supaBaseGenres = new Dictionary<int, GenreSupabaseModel>();


        public async Task SaveAnimesToDatabaseSupaBase(List<Anime> animes)
        {
			List<AnimeSupabaseModel> animeSupabaseModels = new List<AnimeSupabaseModel>();

			foreach (Anime an in animes)
			{
				AnimeSupabaseModel animeModel = MapAnimeCsvToAnimeSupabase(an);
				animeSupabaseModels.Add(animeModel);

                supaBaseAnimes[an.Mal_id] = animeModel;

            }
            var response = await _client.From<AnimeSupabaseModel>().Insert(animeSupabaseModels);
          
        }



        public async Task SaveRelationsToDatabaseSupaBase(Dictionary<int, Anime> animes)
        {
 
            List<RelationSupabaseModel> relationSupabaseModels = new List<RelationSupabaseModel>();


            List<Dictionary<Dictionary<int, int>, string>> visited = new List<Dictionary<Dictionary<int, int>, string>>();

            HashSet<int> existingAnimeIds = animes.Select(pair => pair.Key).ToHashSet();


            foreach (Anime anime in animes.Values)
            {
                var entity2 = animes[anime.Mal_id];
                if (entity2 == null) continue;

                foreach (Relations relations in anime.Relations)
                {
                    foreach (Entry entry in relations.Entry)
                    {
                        if (entry.Type != "anime") continue;

                        Anime entity;
                        if (!animes.TryGetValue(entry.Mal_id, out entity)) continue;

                        var dictionary = new Dictionary<Dictionary<int, int>, string> { { new Dictionary<int, int> { { 1, anime.Mal_id }, { 2, entry.Mal_id } }, relations.Relation }, };
                        bool alreadyVisited = visited.Any(d => AreDictionariesEqual(d, dictionary));
                        if (alreadyVisited)
                        {
                            continue;
                        }

                        bool exists = existingAnimeIds.Contains(anime.Mal_id) && existingAnimeIds.Contains(entry.Mal_id);
                        if (!exists) continue;

                        visited.Add(dictionary);


                        RelationSupabaseModel relsupamod = new RelationSupabaseModel
                        {
                            ParentId = anime.Mal_id,
                            ChildId = entry.Mal_id,
                            RelationType = relations.Relation,
                        };

                        relationSupabaseModels.Add(relsupamod);

                    }
                }
            }
            var response = await _client.From<RelationSupabaseModel>().Insert(relationSupabaseModels);

        }


        public async Task SaveExternalsToDatabaseSupaBase(List<Anime> animes)
        {

            List<ExternalSupabaseModel> externalSupabaseModels = new List<ExternalSupabaseModel>();


                
                foreach (Anime an in animes)
                {
                AnimeSupabaseModel? entity2 = null;
                supaBaseAnimes.TryGetValue(an.Mal_id, out entity2);
                    if (entity2 == null) continue;
                    if (an.Externals == null) continue;

                    foreach (External ext in an.Externals)
                    {

                       
						ExternalSupabaseModel extms = new ExternalSupabaseModel
						{
							Name = ext.Name,
							Url = ext.Url,
							AnimeId = an.Mal_id
						};

                        externalSupabaseModels.Add(extms);
                        
                    }
                }

               var response = await _client.From<ExternalSupabaseModel>().Insert(externalSupabaseModels);

        }
        

        public async Task SaveStreamingsToDatabaseSupaBase(List<Anime> animes)
        {

            List<StreamingSupabaseModel> streamingSupabaseModels = new List<StreamingSupabaseModel>();

            HashSet<string> visited = new HashSet<string>();

            int id_counter = 1;

            foreach (Anime an in animes)
            {
                if (an.Streamings == null) continue;
                foreach (Streaming st in an.Streamings)
                {
                    string key = $"{st.Name}|!!|{st.Url}";
                    if (visited.Contains(key)) continue;
                    visited.Add(key);

					StreamingSupabaseModel stmb = new StreamingSupabaseModel
					{
						Name = st.Name,
						Url = st.Url,
					};
					streamingSupabaseModels.Add(stmb);
                    supaBaseStreamings[id_counter] = stmb;
                    id_counter += 1;
                   

                }
                
            }
          var response = await _client.From<StreamingSupabaseModel>().Insert(streamingSupabaseModels);
            
        }



        public async Task SaveAnimeStreamingsToDatabaseSupaBase(List<Anime> animes)
        {

         

            List<AnimeStreamingSupabaseModel> animeSupabaseStreamingModels = new List<AnimeStreamingSupabaseModel>();

            HashSet<string> visited = new HashSet<string>();
           
                foreach (Anime anime in animes)
                {

                AnimeSupabaseModel? entity2 = null;
                supaBaseAnimes.TryGetValue(anime.Mal_id, out entity2);
                Console.WriteLine("entity2");
                if (entity2 == null) continue;

                Console.WriteLine("anime streaming is null");
                if (anime.Streamings == null) continue;
                    foreach (Streaming streaming in anime.Streamings)
                    {
                        KeyValuePair<int, StreamingSupabaseModel> streamingModel = supaBaseStreamings.FirstOrDefault(s => s.Value.Name == streaming.Name && s.Value.Url == streaming.Url);
                        Console.WriteLine("streamingmodel");

                        if (streamingModel.Equals(default(KeyValuePair<int, StreamingSupabaseModel>))) continue;

                       

                        int streamingId = streamingModel.Key;

                        string key = $"{streamingId}|!!|{anime.Mal_id}";
                        Console.WriteLine("már containeli");
                        if (visited.Contains(key)) continue;
                        visited.Add(key);

                        Console.WriteLine("---------------- ------------------------------ ---------------------------");
						AnimeStreamingSupabaseModel astms = new AnimeStreamingSupabaseModel
						{
							AnimeId = anime.Mal_id,
							StreamingId = streamingId
						};

                    await Console.Out.WriteLineAsync($"{astms.AnimeId} - {astms.StreamingId}");

                    animeSupabaseStreamingModels.Add(astms);
                       

                    }
                }

            var response = await _client.From<AnimeStreamingSupabaseModel>().Insert(animeSupabaseStreamingModels);
        }



        public void SaveUpdatedProductionEntityCountsToDatabaseSupaBase()
        {

            List<ProductionEntitySupabaseModel> updated_models = new List<ProductionEntitySupabaseModel>();

            var prodentvalues = supaBaseProdEnts.Values.ToList();
            var result = prodentvalues.GroupBy(ape => ape.Id)
            .Select(g => new
            {
                ProductionEntityId = g.Key,
                AnimeCount = g.Count()
            })
            .ToList();

            foreach (var res in result)
            {
                var prodent = prodentvalues.Where(x => x.Id == res.ProductionEntityId).FirstOrDefault();
                if (prodent != null)
                {
                    prodent.Count = res.AnimeCount;
                    supaBaseProdEnts[res.ProductionEntityId] = prodent;
                }
            }
        }



        public async Task SaveProductionEntitiesToDatabaseSupaBase(List<ProductionEntity> prodents)
        {



            foreach (ProductionEntity prod in prodents)
            {

                var curr_prodent = new ProductionEntitySupabaseModel
                {
                    Id = prod.Mal_id,
                    Url = prod.Url,
                    Favorites = prod.Favorites,
                    Established = prod.Established,
                    About = prod.About,
                    Count = prod.Count,
                    ImageUrl = prod.Image_url

                };

                this.supaBaseProdEnts[prod.Mal_id] = curr_prodent;
            }
            SaveUpdatedProductionEntityCountsToDatabaseSupaBase();

            var response = await _client.From<ProductionEntitySupabaseModel>().Insert(this.supaBaseProdEnts.Values);

        }

        public async Task SaveProductionEntityTitlesToDatabaseSupaBase(List<ProductionEntity> prodents)
        {

            List<ProductionEntityTitleSupabaseModel> productionEntityTitleSupabaseModels = new List<ProductionEntityTitleSupabaseModel>();
            List<Dictionary<Dictionary<int, int>, string>> visited = new List<Dictionary<Dictionary<int, int>, string>>();

                foreach (ProductionEntity prod in prodents)
                {
                    ProductionEntitySupabaseModel? entity = null;
                    supaBaseProdEnts.TryGetValue(prod.Mal_id, out entity);
                    if (entity == null) continue;

                    foreach (ProductionEntityTitle title in prod.Titles)
                    {
                      
                       
						productionEntityTitleSupabaseModels.Add(new ProductionEntityTitleSupabaseModel
						{
							Type = title.Type,
							Title = title.Title,
							ParentId = prod.Mal_id

						});
                       
                    }

                }

            var response = await _client.From<ProductionEntityTitleSupabaseModel>().Insert(productionEntityTitleSupabaseModels);


        }


        public async Task SaveAnimeProductionEntitiesToDatabaseSupaBase(List<Anime> animes)
        {

            List<AnimeProductionEntitySupabaseModel> animeProductionEntitySupabaseModels = new List<AnimeProductionEntitySupabaseModel>();

            List<Dictionary<int, int>> visitedStudios = new List<Dictionary<int, int>>();
            List<Dictionary<int, int>> visitedLicensors = new List<Dictionary<int, int>>();
            List<Dictionary<int, int>> visitedProducers = new List<Dictionary<int, int>>();

            using (var _context = _dbContextFactory.CreateDbContext())
            {
                foreach (Anime anime in animes)
                {
                    AnimeSupabaseModel? entity2 = null;
                    supaBaseAnimes.TryGetValue(anime.Mal_id, out entity2);
                    if (entity2 == null) continue;

                    foreach (Entry studio in anime.Studios)
                    {
                        Dictionary<int, int> visited = new Dictionary<int, int> { { anime.Mal_id, studio.Mal_id } };

                        ProductionEntitySupabaseModel? entity = null;
                        supaBaseProdEnts.TryGetValue(studio.Mal_id, out entity);
                        if (entity == null) continue;

                        if (visitedStudios.Contains(visited)) continue;
                        visitedStudios.Add(visited);


						animeProductionEntitySupabaseModels.Add(new AnimeProductionEntitySupabaseModel
						{
							AnimeId = anime.Mal_id,
							ProductionEntityId = studio.Mal_id,
							Type = "S"
						});
                        

                    }


                    foreach (Entry licensor in anime.Licensors)
                    {
                        Dictionary<int, int> visited = new Dictionary<int, int> { { anime.Mal_id, licensor.Mal_id } };

                        ProductionEntitySupabaseModel? entity = null;
                        supaBaseProdEnts.TryGetValue(licensor.Mal_id, out entity);
                        if (entity == null) continue;

                        if (visitedStudios.Contains(visited)) continue;
                        visitedStudios.Add(visited);

                        
						animeProductionEntitySupabaseModels.Add(new AnimeProductionEntitySupabaseModel
						{
							AnimeId = anime.Mal_id,
							ProductionEntityId = licensor.Mal_id,
							Type = "L"
						});
                        
                    }


                    foreach (Entry producer in anime.Producers)
                    {
                        Dictionary<int, int> visited = new Dictionary<int, int> { { anime.Mal_id, producer.Mal_id } };

                        ProductionEntitySupabaseModel? entity = null;
                        supaBaseProdEnts.TryGetValue(producer.Mal_id, out entity);
                        if (entity == null) continue;

                        if (visitedStudios.Contains(visited)) continue;
                        visitedStudios.Add(visited);

                        
						animeProductionEntitySupabaseModels.Add(new AnimeProductionEntitySupabaseModel
						{
							AnimeId = anime.Mal_id,
							ProductionEntityId = producer.Mal_id,
							Type = "P"
						});
                        
                    }
                }

            

                var response = await _client.From<AnimeProductionEntitySupabaseModel>().Insert(animeProductionEntitySupabaseModels);
            }
        }



        public async Task SaveAnimeUsersToDatabaseSupaBase()
        {
             
          // not implemented by choice
        }



        public async Task SaveGenresToDatabaseSupaBase(List<AnimeGenre> genres)
        {

       

            List<GenreSupabaseModel> genreSupabaseModels = new List<GenreSupabaseModel>();

            foreach (var genre in genres)
            {

                var currgenre = new GenreSupabaseModel
                {
                    Mal_id = genre.Mal_id,
                    Url = genre.Url,
                    Name = genre.Name

                };

                genreSupabaseModels.Add(currgenre);

                supaBaseGenres[currgenre.Mal_id] = currgenre;


            }

        
           var response = await _client.From<GenreSupabaseModel>().Insert(genreSupabaseModels);

        }



        public async Task SaveAnimeGenresToDatabaseSupaBase(List<Anime> animes)
        {

            List<AnimeGenreModel> genreModels = new List<AnimeGenreModel>();

            List<AnimeGenreSupabaseModel> genreSupabaseModels = new List<AnimeGenreSupabaseModel>();

            Dictionary<int, int> visited = new Dictionary<int, int>();


            foreach (var anime in animes)
            {
                AnimeSupabaseModel? entity2 = null;
                supaBaseAnimes.TryGetValue(anime.Mal_id, out entity2);
                if (entity2 == null) continue;
                foreach (var ang in anime.Genres)
                {
                    GenreSupabaseModel? entity = null;
                    supaBaseGenres.TryGetValue(ang.Mal_id, out entity);
                    if (entity == null) continue;

                    genreSupabaseModels.Add(new AnimeGenreSupabaseModel
                    {
                        AnimeId = anime.Mal_id,
                        GenreId = ang.Mal_id,
                        IsTheme = false
                    });
                   

                }

                foreach (var ang in anime.Themes)
                {
                    GenreSupabaseModel? entity = null;
                    supaBaseGenres.TryGetValue(ang.Mal_id, out entity);
                    if (entity == null) continue;
                    
                    genreSupabaseModels.Add(new AnimeGenreSupabaseModel
                    {
                        AnimeId = anime.Mal_id,
                        GenreId = ang.Mal_id,
                        IsTheme = true
                    });
                    
                }
            }

            var response = await _client.From<AnimeGenreSupabaseModel>().Insert(genreSupabaseModels);

        }
    }
}
