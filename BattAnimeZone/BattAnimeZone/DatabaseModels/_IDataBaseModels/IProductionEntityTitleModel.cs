
namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
 
    public interface IProductionEntityTitleModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int ParentId { get; set; }
    }
}
