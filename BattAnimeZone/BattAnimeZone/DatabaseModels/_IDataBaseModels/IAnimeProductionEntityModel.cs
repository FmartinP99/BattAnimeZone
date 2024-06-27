
namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
    public interface IAnimeProductionEntityModel
    {
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public int ProductionEntityId { get; set; }
        public string Type { get; set; }
    }
}
