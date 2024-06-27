
namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
    public interface IAnimeGenreModel
    {
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public int GenreId { get; set; }
        public bool IsTheme { get; set; } 
    }
}
