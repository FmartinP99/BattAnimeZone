using AutoMapper;
using BattAnimeZone.DatabaseModels;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;

namespace BattAnimeZone.Utilities
{
	public class MappringProfileDataBase : Profile
	{
		public MappringProfileDataBase()
		{
			CreateMap<AnimeModel, AnimeHomePageDTO>();
		}
	}
}
