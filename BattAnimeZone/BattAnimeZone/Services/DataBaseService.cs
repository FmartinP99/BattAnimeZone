using BattAnimeZone.DbContexts;
using BattAnimeZone.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BattAnimeZone.Utilities;


namespace BattAnimeZone.Services
{
	public partial class DataBaseService
	{

		private IDbContextFactory<AnimeDbContext> _dbContextFactory;
		private static IMapper dataBaseMapper;

		public DataBaseService(IDbContextFactory<AnimeDbContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;

			MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<MappringProfileDataBase>());
			dataBaseMapper = config.CreateMapper();
		}

	}
}
