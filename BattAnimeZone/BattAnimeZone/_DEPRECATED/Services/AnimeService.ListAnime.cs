using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.Genre;


#pragma warning disable CS1998
namespace BattAnimeZone.Services
{
    public partial class AnimeService
    {


        private async Task<HashSet<int>> FilterByGenres(List<int> genres)
        {
            HashSet<int> found_animes_by_genres;

            if (genres != null && genres.Count > 0)
            {
                found_animes_by_genres = new HashSet<int>(this.animesPerGenreIdsHash[genres[0]]);
                for (int i = 1; i < genres.Count; i++)
                {
                    HashSet<int> curr_animes = this.animesPerGenreIdsHash[genres[i]];
                    found_animes_by_genres.IntersectWith(curr_animes);
                }

                return found_animes_by_genres;
            }
            return new HashSet<int>();
        }


        private async Task<HashSet<int>> FilterByMediaTypes(List<string> mediaTypes)
        {
            HashSet<int> commonElements;

            if (mediaTypes != null && mediaTypes.Count > 0)
            {
                commonElements = new HashSet<int>(this.animesPerMediaTypesHash[mediaTypes[0]]);
                if (mediaTypes.Count > 1)
                {
                    foreach (string key in mediaTypes.Skip(1))
                    {
                        commonElements.UnionWith(this.animesPerMediaTypesHash[key]);
                    }
                }
                return commonElements;
            }
            return new HashSet<int>();
        }


        public async Task<List<LiAnimeDTO>> GetFilteredAnimes(List<int>? genres, List<string>? mediaTypes)
        {

            List<Task<HashSet<int>>> filteringTasks = new List<Task<HashSet<int>>>();

            if (genres != null && genres.Any()) filteringTasks.Add(FilterByGenres(genres));
            if (mediaTypes != null && mediaTypes.Any()) filteringTasks.Add(FilterByMediaTypes(mediaTypes));

            await Task.WhenAll(filteringTasks);

            List<HashSet<int>> found_anime_ids = filteringTasks.Select(t => t.Result).ToList();


            HashSet<int> found_animes = new HashSet<int>();
            if (found_anime_ids.Any())
            {
                found_animes = found_anime_ids[0];
                foreach (HashSet<int> faids in found_anime_ids.Skip(1))
                {
                    found_animes.IntersectWith(faids);
                }
            }

            

            List<Task<Anime>> animeTasks = found_animes.Select(anime_id => GetAnimeByID(anime_id)).ToList();
            await Task.WhenAll(animeTasks);

            List<Anime> animelist = animeTasks.Select(t => t.Result).ToList();

            return animeMapper.Map<List<LiAnimeDTO>>(animelist);
        }



        public async Task<List<LiAnimeDTO>> GetAnimesForListAnime(HashSet<int> ids)
        {
            List<Anime> animelist = new List<Anime>();
            foreach (int id in ids)
            {
                animelist.Add(await GetAnimeByID(id));
            }

            return animeMapper.Map<List<LiAnimeDTO>>(animelist);
        }
    }
}
