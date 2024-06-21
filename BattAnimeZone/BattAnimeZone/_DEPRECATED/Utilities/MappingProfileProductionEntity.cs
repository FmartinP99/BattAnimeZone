using AutoMapper;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;

namespace BattAnimeZone._DEPRECATED.Utilities
{
    public class MappingProfileProductionEntity : Profile
    {
        public MappingProfileProductionEntity()
        {
            CreateMap<ProductionEntity, LiProductionEntityDTO>();
        }
    }
}
