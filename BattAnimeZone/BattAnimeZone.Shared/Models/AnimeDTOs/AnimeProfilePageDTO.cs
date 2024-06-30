using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class AnimeProfilePageDTO
    {
        public int Mal_id { get; set; } = -1;
        public string Title { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public int Episodes { get; set; } = -1;
        public string Status { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
        public float Score { get; set; } = -1;
        public int Popularity { get; set; } = -1;
        public int Year { get; set; } = -1;
        public string ImageLargeWebpUrl { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;
        public int UserRating { get; set; } = -1;
        public bool UserFavorite { get; set; } = false;
        public string Date { get; set; } = string.Empty;
    }
}
