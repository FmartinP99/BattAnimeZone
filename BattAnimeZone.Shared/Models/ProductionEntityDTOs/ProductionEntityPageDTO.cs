using BattAnimeZone.Shared.Models.AnimeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BattAnimeZone.Shared.Models.ProductionEntityDTOs
{
    public class ProductionEntityPageDTO
    {
        public ProductionEntity.ProductionEntity? ProdEnt { get; set; } = null;
        public List<AnimeStudioPageDTO>? ProducedAnimes {  get; set; } = null;
        public List<AnimeStudioPageDTO>? LicensedAnimes {  get; set; } = null;
        public List<AnimeStudioPageDTO>? StudioAnimes {  get; set; } = null;
    }
}
