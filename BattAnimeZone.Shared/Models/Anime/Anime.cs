namespace BattAnimeZone.Shared.Models.Anime
{
	public class Anime
	{
		public int Mal_id { get; set; } = -1;
		public string Url { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string TitleEnglish { get; set; } = string.Empty;
		public string TitleJapanese { get; set; } = string.Empty;
		public List<string> TtileSynonyms { get; set; } = new List<string> {""};
		public string MediaType  { get; set; } = string.Empty;
		public string Source { get; set; } = string.Empty;
		public int Episodes { get; set; } = -1;
		public string Status { get; set; } = string.Empty;
		public string Duration { get; set; } = string.Empty;
		public string Rating { get; set; } = string.Empty;
		public float Score { get; set; } = -1;
		public int ScoredBy { get; set; } = -1;
		public int Rank {  get; set; } = -1;
		public int Popularity {  get; set; } = -1;
		public int Members {  get; set; } = -1;
		public int Favorites {  get; set; } = -1;
		public string Synopsis {  get; set; } = string.Empty;
		public string Background {  get; set; } = string.Empty;
		public string Season {  get; set; } = string.Empty;
		public int Year { get; set; } = -1;
		public List<Entry>? Producers { get; set; }
		public List<Entry>? Licensors { get; set; }
		public List<Entry>? Studios { get; set; }
		public List<Entry>? Genres { get; set; }
		public List<Entry>? Themes { get; set; }
		public List<Relations>? Relations { get; set; }

		public List<External>? Externals { get; set; }
		public List<Streaming>? Streamings { get; set; }
		public string ImageJpgUrl { get; set; } = string.Empty;
		public string ImageSmallJpgUrl { get; set; } = string.Empty;
		public string ImageLargeJpgUrl { get; set; } = string.Empty;
		public string ImageWebpUrl { get; set; } = string.Empty;
		public string ImageSmallWebpUrl { get; set; } = string.Empty;

		public string ImageLargeWebpUrl { get; set; } = string.Empty;

		public string TrailerUrl { get; set; } = string.Empty;
		public string TrailerEmbedUrl { get; set; } = string.Empty;
		public string TrailerImageUrl { get; set; } = string.Empty;
		public string TrailerImageSmallUrl { get; set; } = string.Empty;
		public string TrailerImageMediumUrl { get; set; } = string.Empty;
		public string TrailerImageLargeUrl { get; set; } = string.Empty;
		public string TrailerImageMaximumUrl { get; set; } = string.Empty;

		public int AiredFromDay { get; set; } = -1;
		public int AiredFromMonth { get; set; } = -1;
		public int AiredFromYear { get; set; } = -1;
		public int AiredToDay { get; set; } = -1;
		public int AiredToMonth { get; set; } = -1;
		public int AiredToYear { get; set; } = -1;
		public string AiredString { get; set; } = string.Empty;




	}
}
