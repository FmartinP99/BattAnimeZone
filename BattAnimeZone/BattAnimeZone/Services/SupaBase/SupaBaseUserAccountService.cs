using BattAnimeZone.Authentication;
using BattAnimeZone.Authentication.PasswordHasher;
using BattAnimeZone.DatabaseModels.SQliteDatabaseModels;
using BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Supabase.Gotrue;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
    public class SupaBaseUserAccountService
    {
        private Supabase.Client? _client;
        private readonly ITokenBlacklistingService _tokenBlacklistingService;
        private JsonSerializerOptions jsonOptions;
        private readonly IPasswordHasher _passwordHasher;

        private struct InteractedAnimeEntry
        {
            public int Mal_id { get; set; }
            public int Rating { get; set; }
            public string Status { get; set; }
            public bool Favorite { get; set; }
        }

        public SupaBaseUserAccountService(Supabase.Client? client, ITokenBlacklistingService tokenBlacklistingService)
        {
            _client = client;
            _tokenBlacklistingService = tokenBlacklistingService;
            _passwordHasher = new PasswordHasher();

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        }


        public async Task<bool> RegisterUser(RegisterRequest user)
        {
            var result = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == user.UserName || x.Email == user.Email).Single();
            if (result != null) return false;
            string? passwordHash = _passwordHasher.Hash(user.Password);
            var response = await _client.From<UserAccountSupabaseModel>().Insert(new UserAccountSupabaseModel { UserName = user.UserName, Password = passwordHash, Email = user.Email.ToLower(), Role = "User", RegisteredAt = DateTime.Now.ToUniversalTime().ToString() });
            if (response == null || !response.ResponseMessage.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<UserSession?> Login(LoginRequest loginRequest)
        {
            UserAccountSupabaseModel? userAccount = null;
            userAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == loginRequest.UserName).Single();
            if (userAccount == null) return null;


            var jwtAuthenticationManager = new JwtAuthenticationManager();
            UserSession? userSession;
            bool isMatching = _passwordHasher.Verify(userAccount.Password, loginRequest.Password);
            if (!isMatching) return null;
            userSession = jwtAuthenticationManager.GenerateJwtToken(userAccount);

            userAccount.Token = userSession.Token;
            userAccount.RefreshToken = userSession.RefreshToken;
            userAccount.RefreshTokenExpiryTime = DateTime.Now.ToUniversalTime().AddMinutes(jwtAuthenticationManager.JWT_REFRESH_TOKEN_VALIDITY_MINS).ToString("o");

            await _client.From<UserAccountSupabaseModel>().Update(userAccount);

            return userSession;
        }


        public async Task<Dictionary<int, InteractedAnime>?> GetInteractedAnimes(string UserName, string token)
        {

            UserAccountSupabaseModel? db_UserExists = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == UserName && x.Token == token).Single();
            if (db_UserExists == null) return null;

            var response = await _client.Rpc("get_interacted_anime_by_user", new { _username = UserName });

            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var returnDto = new Dictionary<int, InteractedAnime>();
                if (response.Content.Trim() == "[]" ) return returnDto; 

                var response_content = JsonSerializer.Deserialize<List<InteractedAnimeEntry>>(response.Content, jsonOptions);
                response_content.ForEach(rc => returnDto[rc.Mal_id] = new InteractedAnime { Favorite = rc.Favorite, Rating = rc.Rating, Status = rc.Status });
                
                return returnDto;
            }
            return null;
           
           

        }

    }
}
