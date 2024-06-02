using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone.Services
{
    public partial class AnimeService
    {
        public async Task<List<AnimeStudioPageDTO>> GetProducedAnimes(int mal_id)
        {

            HashSet<int> produced_animes = await GetAnimesOfProducer(mal_id);

            List<Anime> animelist = new List<Anime>();
            foreach (int id in produced_animes)
            {
                animelist.Add(await GetAnimeByID(id));
            }
            return animeMapper.Map<List<AnimeStudioPageDTO>>(animelist);
        }

        public async Task<List<AnimeStudioPageDTO>> GetLicensedAnimes(int mal_id)
        {

            HashSet<int> produced_animes = await GetAnimesOfLicensor(mal_id);

            List<Anime> animelist = new List<Anime>();
            foreach (int id in produced_animes)
            {
                animelist.Add(await GetAnimeByID(id));
            }
            return animeMapper.Map<List<AnimeStudioPageDTO>>(animelist);
        }

        public async Task<List<AnimeStudioPageDTO>> GetStudioAnimes(int mal_id)
        {

            HashSet<int> produced_animes = await GetAnimesOfStudio(mal_id);

            List<Anime> animelist = new List<Anime>();
            foreach (int id in produced_animes)
            {
                animelist.Add(await GetAnimeByID(id));
            }
            return animeMapper.Map<List<AnimeStudioPageDTO>>(animelist);
        }



    }
}
