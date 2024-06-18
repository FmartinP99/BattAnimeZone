

namespace BattAnimeZone.Shared.Models.User.BrowserStorageModels
{
    public class InteractedAnime
    {
        public int MalId { get; set; } = -1;
        public string? Title { get; set; } = "";
        public int Rating { get; set; } = 0;
        public string? Status { get; set; } = null;
        public bool Favorite { get; set; } = false;
    }
}
