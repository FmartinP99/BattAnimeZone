using AutoMapper;
using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone._DEPRECATED.Utilities
{
    public class MappingProfileAnime : Profile
    {
        public MappingProfileAnime()
        {
            CreateMap<Anime, AnimeHomePageDTO>();
            CreateMap<Anime, AnimePageDTO>();
            CreateMap<Anime, LiAnimeDTO>();
            CreateMap<Anime, LiGenreAnimeDTO>();
            CreateMap<Anime, AnimeSearchResultDTO>();
            CreateMap<Anime, AnimeRelationsKeyDTO>();
            CreateMap<Anime, AnimeRelationDTO>();
            CreateMap<Anime, AnimeStudioPageDTO>();
        }

    }
}
