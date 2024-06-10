using Microsoft.EntityFrameworkCore;
using BattAnimeZone.Shared.DatabaseModels.Anime;
using BattAnimeZone.Shared.DatabaseModels.Relation;
using BattAnimeZone.Shared.DatabaseModels.External;
using BattAnimeZone.Shared.DatabaseModels.Streaming;
using BattAnimeZone.Shared.DatabaseModels.AnimeStreaming;

namespace BattAnimeZone.DbContexts
{
    public class AnimeDbContext : DbContext
    {
        public DbSet<AnimeModel> Animes { get; set; }
        public DbSet<RelationModel> Relations { get; set; }
        public DbSet<ExternalModel> Externals { get; set; }
        public DbSet<StreamingModel> Streamings { get; set; }
        public DbSet<AnimeStreamingModel> AnimeStreamings { get; set; }

        public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
            : base(options)
        {
        }
    }
}