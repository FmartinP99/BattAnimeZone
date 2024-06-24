using BattAnimeZone.DbContexts;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.Genre;
using BattAnimeZone.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Authentication.PasswordHasher;




namespace BattAnimeZone.DatabaseInitializer
{
    public class CsvToDataBaseHandler
    {

        private readonly static int batchsize = 100;
        private IDbContextFactory<AnimeDbContext> _dbContextFactory;

        public CsvToDataBaseHandler(IDbContextFactory<AnimeDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
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
                Status = anime.Status,
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

            foreach (Anime an in animes)
            {
                AnimeModel animeModel = MapAnimeCsvToAnime(an);
                animeModels.Add(animeModel);
            }


            using (var _context = _dbContextFactory.CreateDbContext())
            {

                for (int i = 0; i < animeModels.Count; i += batchsize)
                {

                    var batch = animeModels.Skip(i).Take(batchsize).ToList();


                    await _context.Animes.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
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



            using (var _context = _dbContextFactory.CreateDbContext())
            {
                for (int i = 0; i < relationModels.Count; i += batchsize)
                {

                    var batch = relationModels.Skip(i).Take(batchsize).ToList();
                    await _context.Relations.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();


                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }

                }
            }

        }


        public async Task SaveExternalsToDatabase(List<Anime> animes)
        {

            List<ExternalModel> externalModels = new List<ExternalModel>();

            using (var _context = _dbContextFactory.CreateDbContext())
            {
                foreach (Anime an in animes)
                {
                    var entity2 = _context.Animes.Find(an.Mal_id);
                    if (entity2 == null) continue;
                    if (an.Externals == null) continue;

                    foreach (External ext in an.Externals)
                    {

                        ExternalModel extm = new ExternalModel
                        {
                            Name = ext.Name,
                            Url = ext.Url,
                            AnimeId = an.Mal_id
                        };
                        externalModels.Add(extm);
                    }
                }


                for (int i = 0; i < externalModels.Count; i += batchsize)
                {

                    var batch = externalModels.Skip(i).Take(batchsize).ToList();

                    await _context.Externals.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }

        public async Task SaveStreamingsToDatabase(List<Anime> animes)
        {

            List<StreamingModel> streamingModels = new List<StreamingModel>();

            HashSet<string> visited = new HashSet<string>();

            foreach (Anime an in animes)
            {
                if (an.Streamings == null) continue;
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
            using (var _context = _dbContextFactory.CreateDbContext())
            {
                for (int i = 0; i < streamingModels.Count; i += batchsize)
                {

                    var batch = streamingModels.Skip(i).Take(batchsize).ToList();
                    await _context.Streamings.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }

        }



        public async Task SaveAnimeStreamingsToDatabase(List<Anime> animes)
        {

            List<AnimeStreamingModel> animeStreamingModels = new List<AnimeStreamingModel>();

            HashSet<string> visited = new HashSet<string>();
            using (var _context = _dbContextFactory.CreateDbContext())
            {
                foreach (Anime anime in animes)
                {
                    if (anime.Streamings == null) continue;
                    foreach (Streaming streaming in anime.Streamings)
                    {
                        StreamingModel? streamingModel = _context.Streamings.FirstOrDefault(s => s.Name == streaming.Name && s.Url == streaming.Url);
                        if (streamingModel == null) continue;

                        var entity2 = _context.Animes.Find(anime.Mal_id);
                        if (entity2 == null) continue;

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


                for (int i = 0; i < animeStreamingModels.Count; i += batchsize)
                {

                    var batch = animeStreamingModels.Skip(i).Take(batchsize).ToList();

                    await _context.AnimeStreamings.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }



        public async Task SaveProductionEntitiesToDatabase(List<ProductionEntity> prodents)
        {

            List<ProductionEntityModel> prodentsModel = new List<ProductionEntityModel>();

            foreach (ProductionEntity prod in prodents)
            {

                prodentsModel.Add(new ProductionEntityModel
                {
                    Id = prod.Mal_id,
                    Url = prod.Url,
                    Favorites = prod.Favorites,
                    Established = prod.Established,
                    About = prod.About,
                    Count = prod.Count,
                    ImageUrl = prod.Image_url

                });
            }

            using (var _context = _dbContextFactory.CreateDbContext())
            {
                for (int i = 0; i < prodentsModel.Count; i += batchsize)
                {

                    var batch = prodentsModel.Skip(i).Take(batchsize).ToList();
                    await _context.ProductionEntities.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }

        public async Task SaveProductionEntityTitlesToDatabase(List<ProductionEntity> prodents)
        {

            List<ProductionEntityTitleModel> prodentsModel = new List<ProductionEntityTitleModel>();

            List<Dictionary<Dictionary<int, int>, string>> visited = new List<Dictionary<Dictionary<int, int>, string>>();

            using (var _context = _dbContextFactory.CreateDbContext())
            {

                foreach (ProductionEntity prod in prodents)
                {

                    var entity = _context.ProductionEntities.Find(prod.Mal_id);
                    if (entity == null) continue;

                    foreach (ProductionEntityTitle title in prod.Titles)
                    {
                        prodentsModel.Add(new ProductionEntityTitleModel
                        {
                            Type = title.Type,
                            Title = title.Title,
                            ParentId = prod.Mal_id

                        });
                    }

                }

                for (int i = 0; i < prodentsModel.Count; i += batchsize)
                {

                    var batch = prodentsModel.Skip(i).Take(batchsize).ToList();
                    await _context.ProductionEntityTitles.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }


        public async Task SaveAnimeProductionEntitiesToDatabase(List<Anime> animes)
        {

            List<AnimeProductionEntityModel> aniprodentsmodel = new List<AnimeProductionEntityModel>();

            List<Dictionary<int, int>> visitedStudios = new List<Dictionary<int, int>>();
            List<Dictionary<int, int>> visitedLicensors = new List<Dictionary<int, int>>();
            List<Dictionary<int, int>> visitedProducers = new List<Dictionary<int, int>>();

            using (var _context = _dbContextFactory.CreateDbContext())
            {
                foreach (Anime anime in animes)
                {
                    foreach (Entry studio in anime.Studios)
                    {
                        Dictionary<int, int> visited = new Dictionary<int, int> { { anime.Mal_id, studio.Mal_id } };

                        var entity = _context.ProductionEntities.Find(studio.Mal_id);
                        if (entity == null) continue;

                        var entity2 = _context.Animes.Find(anime.Mal_id);
                        if (entity2 == null) continue;

                        if (visitedStudios.Contains(visited)) continue;
                        visitedStudios.Add(visited);

                        aniprodentsmodel.Add(new AnimeProductionEntityModel
                        {
                            AnimeId = anime.Mal_id,
                            ProductionEntityId = studio.Mal_id,
                            Type = "S"
                        });
                    }


                    foreach (Entry licensor in anime.Licensors)
                    {
                        Dictionary<int, int> visited = new Dictionary<int, int> { { anime.Mal_id, licensor.Mal_id } };

                        var entity = _context.ProductionEntities.Find(licensor.Mal_id);
                        if (entity == null) continue;

                        var entity2 = _context.Animes.Find(anime.Mal_id);
                        if (entity2 == null) continue;

                        if (visitedLicensors.Contains(visited)) continue;
                        visitedLicensors.Add(visited);

                        aniprodentsmodel.Add(new AnimeProductionEntityModel
                        {
                            AnimeId = anime.Mal_id,
                            ProductionEntityId = licensor.Mal_id,
                            Type = "L"
                        });
                    }


                    foreach (Entry producer in anime.Producers)
                    {
                        Dictionary<int, int> visited = new Dictionary<int, int> { { anime.Mal_id, producer.Mal_id } };

                        var entity = _context.ProductionEntities.Find(producer.Mal_id);
                        if (entity == null) continue;

                        var entity2 = _context.Animes.Find(anime.Mal_id);
                        if (entity2 == null) continue;

                        if (visitedProducers.Contains(visited)) continue;
                        visitedProducers.Add(visited);

                        aniprodentsmodel.Add(new AnimeProductionEntityModel
                        {
                            AnimeId = anime.Mal_id,
                            ProductionEntityId = producer.Mal_id,
                            Type = "P"
                        });
                    }
                }


                for (int i = 0; i < aniprodentsmodel.Count; i += batchsize)
                {

                    var batch = aniprodentsmodel.Skip(i).Take(batchsize).ToList();
                    await _context.AnimeProductionEntities.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }

        public async Task SaveAnimeUsersToDatabase()
        {

            List<AnimeUserModel> animeUserModels = new List<AnimeUserModel>();

            using (var _context = _dbContextFactory.CreateDbContext())
            {
                UserAccountModel? user = _context.UserAccounts.FirstOrDefault(u => u.UserName == "user" && u.Email == "user@user.com");
                UserAccountModel? admin = _context.UserAccounts.FirstOrDefault(u => u.UserName == "admin" && u.Email == "admin@admin.com");

                if (user == null || admin == null) return;

                animeUserModels.Add(new AnimeUserModel
                {
                    AnimeId = 52299,
                    UserId = user.Id,
                    favorite = true,
                    Status = "completed",
                    Rating = 10,
                    Date = DateTime.Now.ToUniversalTime().ToString()
                });

                animeUserModels.Add(new AnimeUserModel
                {
                    AnimeId = 52299,
                    UserId = admin.Id,
                    favorite = false,
                    Status = "watching",
                    Date = DateTime.Now.ToUniversalTime().ToString()
                });

                for (int i = 0; i < animeUserModels.Count; i += batchsize)
                {

                    var batch = animeUserModels.Skip(i).Take(batchsize).ToList();
                    await _context.AnimeUserModels.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }



        public async Task SaveGenresToDatabase(List<AnimeGenre> genres)
        {

            List<GenreModel> genreModels = new List<GenreModel>();

            foreach (var genre in genres)
            {
                genreModels.Add(new GenreModel
                {
                    Mal_id = genre.Mal_id,
                    Url = genre.Url,
                    Name = genre.Name

                });
            }

            using (var _context = _dbContextFactory.CreateDbContext())
            {
                for (int i = 0; i < genreModels.Count; i += batchsize)
                {

                    var batch = genreModels.Skip(i).Take(batchsize).ToList();
                    await _context.Genres.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }


        public async Task SaveDistinctMediaTypes()
        {

           
            using (var _context = _dbContextFactory.CreateDbContext())
            {
                var distinctMediaTypes = _context.Animes.Where(a => !string.IsNullOrEmpty(a.MediaType))
                                     .Select(a => a.MediaType)
                                     .Distinct()
                                     .ToList();

                var distinctMediaTypeEntities = distinctMediaTypes.Select(mt => new DistinctMediaTypesModel
                {
                    mediaType = mt
                }).ToList();

                await _context.DistinctMediaTypes.AddRangeAsync(distinctMediaTypeEntities);
                await _context.SaveChangesAsync();

                foreach (var rmod in _context.DistinctMediaTypes)
                {
                    _context.Entry(rmod).State = EntityState.Detached;
                }
            }
        }

		public async Task SaveDistinctYears()
		{


			using (var _context = _dbContextFactory.CreateDbContext())
			{
				var distinctYears = _context.Animes.Where(a => a.Year != -1)
									 .Select(a => a.Year)
									 .Distinct()
									 .ToList();

				var distinctYearEntities = distinctYears.Select(y => new DistinctYearModel
				{
					Year = y
				}).ToList();

				await _context.DistinctYears.AddRangeAsync(distinctYearEntities);
				await _context.SaveChangesAsync();

				foreach (var rmod in _context.DistinctMediaTypes)
				{
					_context.Entry(rmod).State = EntityState.Detached;
				}
			}
		}


		public async Task SaveAnimeGenresToDatabase(List<Anime> animes)
        {

            List<AnimeGenreModel> genreModels = new List<AnimeGenreModel>();

            Dictionary<int, int> visited = new Dictionary<int, int>();

            using (var _context = _dbContextFactory.CreateDbContext())
            {
                foreach (var anime in animes)
                {
                    var entity2 = _context.Animes.Find(anime.Mal_id);
                    if (entity2 == null) continue;
                    foreach (var ang in anime.Genres)
                    {
                        var entity = _context.Genres.Find(ang.Mal_id);
                        if (entity == null) continue;

                        genreModels.Add(new AnimeGenreModel
                        {
                            AnimeId = anime.Mal_id,
                            GenreId = ang.Mal_id,
                            IsTheme = false
                        });
                    }

                    foreach (var ang in anime.Themes)
                    {
                        var entity = _context.Genres.Find(ang.Mal_id);
                        if (entity == null) continue;

                        genreModels.Add(new AnimeGenreModel
                        {
                            AnimeId = anime.Mal_id,
                            GenreId = ang.Mal_id,
                            IsTheme = true
                        });
                    }
                }


                for (int i = 0; i < genreModels.Count; i += batchsize)
                {
                    var batch = genreModels.Skip(i).Take(batchsize).ToList();
                    await _context.AnimeGenres.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();

                    foreach (var rmod in batch)
                    {
                        _context.Entry(rmod).State = EntityState.Detached;
                    }
                }
            }
        }


        public async Task CreateUsers()
        {
            IPasswordHasher hasher = new PasswordHasher();
            string? passwordHashUser = hasher.Hash("user");
            RegisterRequest user = new RegisterRequest { UserName = "user", Password = passwordHashUser, Email = "user@user.com" };
            string? passwordHashAdmin = hasher.Hash("admin");
            RegisterRequest admin = new RegisterRequest { UserName = "admin", Password = passwordHashAdmin, Email = "admin@admin.com" };

            await RegisterUser(user);
            await RegisterUser(admin);
        }

        public async Task<bool> RegisterUser(RegisterRequest user)
        {

            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                string role = user.UserName == "admin" ? "Admin" : "User";
                _context.UserAccounts.Add(new UserAccountModel { UserName = user.UserName, Password = user.Password, Email = user.Email, Role = role, RegisteredAt = DateTime.Now.ToUniversalTime().ToString() });
                await _context.SaveChangesAsync();
                return true;
            }
        }



    }
}
