using Microsoft.EntityFrameworkCore;
using BattAnimeZone.DatabaseModels;

namespace BattAnimeZone.DbContexts
{
    public class AnimeDbContext : DbContext
    {
        public DbSet<AnimeModel> Animes { get; set; }
        public DbSet<RelationModel> Relations { get; set; }
        public DbSet<ExternalModel> Externals { get; set; }
        public DbSet<StreamingModel> Streamings { get; set; }
        public DbSet<AnimeStreamingModel> AnimeStreamings { get; set; }
        public DbSet<ProductionEntityModel> ProductionEntities { get; set; }
        public DbSet<ProductionEntityTitleModel> ProductionEntityTitles { get; set; }
        public DbSet<AnimeProductionEntityModel> AnimeProductionEntities { get; set; }
        public DbSet<UserAccountModel> UserAccounts { get; set; }
        public DbSet<AnimeUserModel> AnimeUserModels { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<AnimeGenreModel> AnimeGenres { get; set; }

        public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
            : base(options)
        {
        }


        internal void DetachAll()
        {
			this.ChangeTracker.Clear();
		}



    }
}