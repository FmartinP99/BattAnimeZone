using Microsoft.EntityFrameworkCore;
using BattAnimeZone.DatabaseModels;
using BattAnimeZone.Shared.Models.Anime;

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
        public DbSet<DistinctMediaTypesModel> DistinctMediaTypes { get; set; }
        public DbSet<DistinctYearModel> DistinctYears { get; set; }

        public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<RelationModel>()
           .HasOne(r => r.ParentAnime)
           .WithMany()
           .HasForeignKey(r => r.ParentId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelationModel>()
           .HasOne(r => r.ChildAnime)
           .WithMany()
           .HasForeignKey(r => r.ChildId)
           .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<AnimeProductionEntityModel>()
           .HasOne(r => r.Anime)
           .WithMany()
           .HasForeignKey(r => r.AnimeId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnimeProductionEntityModel>()
           .HasOne(r => r.ProductionEntity)
           .WithMany()
           .HasForeignKey(r => r.ProductionEntityId)
           .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<AnimeGenreModel>()
           .HasOne(r => r.Anime)
           .WithMany()
           .HasForeignKey(r => r.AnimeId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnimeGenreModel>()
           .HasOne(r => r.Genre)
           .WithMany()
           .HasForeignKey(r => r.GenreId)
           .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<AnimeStreamingModel>()
          .HasOne(r => r.Anime)
          .WithMany()
          .HasForeignKey(r => r.AnimeId)
          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnimeStreamingModel>()
           .HasOne(r => r.Streaming)
           .WithMany()
           .HasForeignKey(r => r.StreamingId)
           .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<AnimeUserModel>()
           .HasOne(r => r.Anime)
           .WithMany()
           .HasForeignKey(r => r.AnimeId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnimeUserModel>()
           .HasOne(r => r.UserAccount)
           .WithMany()
           .HasForeignKey(r => r.UserId)
           .OnDelete(DeleteBehavior.Cascade);





            modelBuilder.Entity<ProductionEntityTitleModel>()
          .HasOne(r => r.ProductionEntity)
          .WithMany()
          .HasForeignKey(r => r.ParentId)
          .OnDelete(DeleteBehavior.Cascade);





            modelBuilder.Entity<ExternalModel>()
             .HasOne(r => r.Anime)
             .WithMany()
             .HasForeignKey(r => r.AnimeId)
             .OnDelete(DeleteBehavior.Cascade);


        }



        internal void DetachAll()
        {
			this.ChangeTracker.Clear();
		}



    }
}