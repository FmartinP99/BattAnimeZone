

namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
    public interface IAnimeUserModel
    {
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public int UserId { get; set; }
        public bool favorite { get; set; }
        public string Status { get; set; }
        public int Rating { get; set; }
        public string Date { get; set; }

    }
}
