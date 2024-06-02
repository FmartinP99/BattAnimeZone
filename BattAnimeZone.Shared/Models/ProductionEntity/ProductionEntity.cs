namespace BattAnimeZone.Shared.Models.ProductionEntity
{
	public class ProductionEntity
	{
		public int Mal_id { get; set; } = -1;
		public string Url { get; set; } = string.Empty;
		public List<ProductionEntityTitle> Titles { get; set; } = new List<ProductionEntityTitle>();
		public int Favorites { get; set; } = -1;
		public string Established { get; set; } = string.Empty;
		public string About { get; set; } = string.Empty;
		public int Count { get; set; } = -1;
		public string Image_url { get; set; } = string.Empty;
	}
}
