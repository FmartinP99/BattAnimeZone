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
			CreateMap<AnimeModel, AnimeHomePageDTO>()
			.ForMember(dest => dest.Synopsis, opt => opt.MapFrom<SynopsisTruncateResolver>());
		}
	}




	public class SynopsisTruncateResolver : IValueResolver<AnimeModel, AnimeHomePageDTO, string>
	{
		public string Resolve(AnimeModel source, AnimeHomePageDTO destination, string destMember, ResolutionContext context)
		{

			if (string.IsNullOrEmpty(source.Synopsis))
			{
				return string.Empty;
			}


			if (source.Synopsis.Length <= 500)
			{
				return source.Synopsis;
			}
			else
			{

				int lastIndex = source.Synopsis.LastIndexOf(' ', 500);


				if (lastIndex <= 0)
				{
					return source.Synopsis.Substring(0, 500) + "...[Click for more!]";
				}
				else
				{
					return source.Synopsis.Substring(0, lastIndex) + "...[Click for more!]";
				}
			}
		}
	}
}
