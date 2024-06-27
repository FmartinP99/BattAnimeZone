
namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
	public interface IAnimeModel
	{
	
		public int Mal_id { get; set; }
		public string Title { get; set; }
		public string TitleEnglish { get; set; } 
		public string TitleJapanese { get; set; }
		public string TitleSynonyms { get; set; } 
		public string MediaType { get; set; } 
		public string Source { get; set; } 
		public int Episodes { get; set; } 
		public string Status { get; set; } 
		public string Duration { get; set; } 
		public string Rating { get; set; } 
		public float Score { get; set; } 
		public int ScoredBy { get; set; } 
		public int Rank { get; set; } 
		public int Popularity { get; set; }
		public int Members { get; set; } 
		public int Favorites { get; set; }
		public string Synopsis { get; set; } 
		public string Background { get; set; } 
		public string Season { get; set; } 
		public int Year { get; set; } 
		public string ImageJpgUrl { get; set; } 
		public string ImageSmallJpgUrl { get; set; } 
		public string ImageLargeJpgUrl { get; set; } 
		public string ImageWebpUrl { get; set; }
		public string ImageSmallWebpUrl { get; set; }
		public string ImageLargeWebpUrl { get; set; } 
		public string TrailerUrl { get; set; } 
		public string TrailerEmbedUrl { get; set; } 
		public string TrailerImageUrl { get; set; } 
		public string TrailerImageSmallUrl { get; set; } 
		public string TrailerImageMediumUrl { get; set; }
		public string TrailerImageLargeUrl { get; set; }
		public string TrailerImageMaximumUrl { get; set; }
		public int AiredFromDay { get; set; }
		public int AiredFromMonth { get; set; }
		public int AiredFromYear { get; set; }
		public int AiredToDay { get; set; }
		public int AiredToMonth { get; set; }
		public int AiredToYear { get; set; }
		public string AiredString { get; set; }
	}
}
