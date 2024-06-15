namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
	public class AnimeHomePageDTO
	{
		public int Mal_id { get; set; } = -1;
		public string Title { get; set; } = string.Empty;
		public string TitleEnglish { get; set; } = string.Empty;
		public string TitleJapanese { get; set; } = string.Empty;
		public string MediaType { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public float Score { get; set; } = -1;
		public string Season { get; set; } = string.Empty;
		public string ImageLargeWebpUrl { get; set; } = string.Empty;
	
	}
}
