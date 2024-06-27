using DotNetEnv;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;

namespace BattAnimeZone.Services
{
	public class SupabaseService
	{
		private IConfiguration _configuration;
		private Supabase.Client _client;

        public SupabaseService(IConfiguration configuration, Supabase.Client client)
        {
			_configuration = configuration;
			_client = client;
		}

        public async Task fillDatabase()
		{
			var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
			var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
         
            var options = new Supabase.SupabaseOptions
			{
				AutoConnectRealtime = true
			};

			var supabase = new Supabase.Client(url, key, options);
			await supabase.InitializeAsync();

			string sqlCommands = File.ReadAllText(_configuration.GetConnectionString("DatabaseInitFilePath"));

			using (HttpClient _httpClient = new HttpClient()) {
				try
				{
					var apiUrl = $"{url}/rpc/execute_sql";

					// Construct the JSON body
					var requestBody = new { sql = sqlCommands };
					var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					// Set the Supabase API key in the headers
					_httpClient.DefaultRequestHeaders.Clear();
					_httpClient.DefaultRequestHeaders.Add("apikey", key);

					// Send the HTTP POST request
					var response = await _httpClient.PostAsync(apiUrl, content);

					// Check response status
					response.EnsureSuccessStatusCode(); // Throws exception for non-success status codes

					// Read and handle the response
					string responseBody = await response.Content.ReadAsStringAsync();
					Console.WriteLine("SQL script executed successfully!");
					Console.WriteLine($"Response: {responseBody}");
				}
				catch (Exception ex)
				{
					// Handle any exceptions
					Console.WriteLine($"Error executing SQL script: {ex.Message}");
				}
			}

		}
    }
}
