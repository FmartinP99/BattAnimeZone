namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
  
    public interface IProductionEntityModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Favorites { get; set; } 
        public string Established { get; set; }
        public string About { get; set; }
        public int Count { get; set; } 
        public string ImageUrl { get; set; } 
    }
}
