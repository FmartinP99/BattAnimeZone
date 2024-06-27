
namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
    public interface IAnimeStreamingModel
    {
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public int StreamingId { get; set; }
    }
}
