using BattAnimeZone.DbContexts;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BattAnimeZone.Utilities;
using System.ComponentModel;


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



        //THIS FUNCTION IS FOR TESTING PURPOSES
        private async Task PrintProperties(object obj, int depth = 0)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                if (value == null) continue;

                if (value is IList list)
                {
                    await Console.Out.WriteLineAsync(name);
                    foreach (var ev in list)
                    {
                        int newdepth = depth + 1;

                        await PrintProperties(ev, newdepth);
                    }
                }
                else
                {
                    string baseString = String.Concat(Enumerable.Repeat("\t", depth));
                    await Console.Out.WriteLineAsync($"{baseString}{name}  --  {value}");
                }

            }

        }

    }
}
