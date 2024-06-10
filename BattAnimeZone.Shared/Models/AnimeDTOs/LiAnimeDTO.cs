namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class LiAnimeDTO : IAnimeDTO
	{
		public int Mal_id { get; set; } = -1;
		public string TitleEnglish { get; set; } = string.Empty;
		public string TitleJapanese { get; set; } = string.Empty;
		public string MediaType { get; set; } = string.Empty;
        public int Episodes { get; set; } = -1;
        public string Status { get; set; } = string.Empty;
		public string Rating { get; set; } = string.Empty;
		public float Score { get; set; } = -1;
		public int Popularity { get; set; } = -1;
		public int Year { get; set; } = -1;
        public string ImageLargeWebpUrl { get; set; } = string.Empty;
    }
}
