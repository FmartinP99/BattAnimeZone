using DotNetEnv;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;

namespace BattAnimeZone.Services
{
	public class SupabaseService
	{
		private Supabase.Client _client;

        public SupabaseService(Supabase.Client client)
        {
			_client = client;
		}

    }
}
