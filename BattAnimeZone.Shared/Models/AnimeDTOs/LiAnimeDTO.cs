namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class LiAnimeDTO : IAnimeDTO
	{
		public int Mal_id { get; set; } = -1;
		public string Title_english { get; set; } = string.Empty;
		public string Title_japanese { get; set; } = string.Empty;
		public string Media_type { get; set; } = string.Empty;
        public float Episodes { get; set; } = -1;
        public string Status { get; set; } = string.Empty;
		public string Rating { get; set; } = string.Empty;
		public float Score { get; set; } = -1;
		public float Popularity { get; set; } = -1;
		public float Year { get; set; } = -1;
        public string Image_large_webp_url { get; set; } = string.Empty;
    }
}
