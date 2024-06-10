namespace BattAnimeZone.Shared.Models.Anime
{
	public class Anime
	{
		public int Mal_id { get; set; } = -1;
		//public string Url { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string Title_english { get; set; } = string.Empty;
		public string Title_japanese { get; set; } = string.Empty;
		//public List<string> Ttile_synonyms { get; set; } = new List<string> {""};
		public string Media_type { get; set; } = string.Empty;
		public string Source { get; set; } = string.Empty;
		public int Episodes { get; set; } = -1;
		public string Status { get; set; } = string.Empty;
		public string Duration { get; set; } = string.Empty;
		public string Rating { get; set; } = string.Empty;
		public float Score { get; set; } = -1;
		public int Scored_by { get; set; } = -1;
		public int Rank {  get; set; } = -1;
		public int Popularity {  get; set; } = -1;
		public int Members {  get; set; } = -1;
		public int Favorites {  get; set; } = -1;
		public string Synopsis {  get; set; } = string.Empty;
		public string Background {  get; set; } = string.Empty;
		public string Season {  get; set; } = string.Empty;
		public int Year { get; set; } = -1;
		public List<Entry> Producers { get; set; }
		public List<Entry> Licensors { get; set; }
		public List<Entry> Studios { get; set; }
		public List<Entry> Genres { get; set; }
		public List<Entry> Themes { get; set; }
		public List<Relations> Relations { get; set; }
		//public List<External> Externals { get; set; }
		//public List<Streaming> Streamings { get; set; }
		//public string Image_jpg_url { get; set; } = string.Empty;
		//public string Image_small_jpg_url { get; set; } = string.Empty;
		//public string Image_large_jpg_url { get; set; } = string.Empty;
		//public string Image_webp_url { get; set; } = string.Empty;
		//public string Image_small_webp_url { get; set; } = string.Empty;
		public string Image_large_webp_url { get; set; } = string.Empty;
		//public string Trailer_url { get; set; } = string.Empty;
		//public string Trailer_embed_url { get; set; } = string.Empty;
		//public string Trailer_image_url { get; set; } = string.Empty;
		//public string Trailer_image_small_url { get; set; } = string.Empty;
		//public string Trailer_image_medium_url { get; set; } = string.Empty;
		//public string Trailer_image_large_url { get; set; } = string.Empty;
		//public string Trailer_image_maximum_url { get; set; } = string.Empty;
		public int Aired_from_day { get; set; } = -1;
		public int Aired_from_month { get; set; } = -1;
		public int Aired_from_year { get; set; } = -1;
		public int Aired_to_day { get; set; } = -1;
		public int Aired_to_month { get; set; } = -1;
		public int Aired_to_year { get; set; } = -1;
		public string Aired_string { get; set; } = string.Empty;




	}
}
