using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;

namespace BattAnimeZone.Services
{
    public partial class AnimeService
    {
        public Task<Dictionary<int, LiProductionEntityDTO>> GetProductionEntitiesDTO()
        {
            Dictionary<int, ProductionEntity> prodents = this.productionEntities;
            return Task.FromResult(productionEntityMapper.Map<Dictionary<int, LiProductionEntityDTO>>(prodents));
        }
    }
}
