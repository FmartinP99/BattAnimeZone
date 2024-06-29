using BattAnimeZone.Authentication;
using BattAnimeZone.Authentication.PasswordHasher;
using BattAnimeZone.DatabaseModels.SQliteDatabaseModels;
using BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;
using LoginRequest = BattAnimeZone.Shared.Models.User.LoginRequest;
using RegisterRequest = BattAnimeZone.Shared.Models.User.RegisterRequest;

namespace BattAnimeZone.Services.SupaBase
{
    public partial class SupaBaseUserAccountService
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
            string emaillowercalse = user.Email.ToLower();
            string usernamelowercase = user.UserName.ToLower();

            var result = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == usernamelowercase || x.Email == emaillowercalse).Single();
            if (result != null) return false;
            string? passwordHash = _passwordHasher.Hash(user.Password);
            var response = await _client.From<UserAccountSupabaseModel>().Insert(new UserAccountSupabaseModel { UserName = user.UserName, Password = passwordHash, Email = user.Email.ToLower(), Role = "User", RegisteredAt = DateTime.Now.ToUniversalTime().ToString() });
            if (response == null || !response.ResponseMessage.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<UserSession?> Login(LoginRequest loginRequest)
        {

            string usernamelowercase = loginRequest.UserName.ToLower();

            UserAccountSupabaseModel? userAccount = null;
            userAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == usernamelowercase).Single();
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

        public async Task<bool> Logout(UserSession uSession)
        {
            string usernamelowercase = uSession.UserName.ToLower();

            UserAccountSupabaseModel? userAccount = null;
            userAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == usernamelowercase).Single();
            if (userAccount == null) return false;
            userAccount.RefreshToken = null;
            userAccount.Token = null;
            userAccount.RefreshTokenExpiryTime = null;
            _tokenBlacklistingService.BlacklistToken(uSession.Token);
            await _client.From<UserAccountSupabaseModel>().Update(userAccount);      
            return true;

        }


        public async Task<RefreshTokenDTO?> Refresh(RefreshTokenDTO refreshTokenDTO)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager();
            var principal = jwtAuthenticationManager.GetPrincipalFromExpiredToken(refreshTokenDTO.Token);
            var username = principal.Identity.Name;
            


            UserAccountSupabaseModel? userAccount = null;
            userAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == username).Single();
            if (userAccount == null || userAccount.RefreshToken == null) return null;

            DateTime refreshTokenExpiryDate = DateTime.Parse(userAccount.RefreshTokenExpiryTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            if (userAccount.RefreshToken != refreshTokenDTO.RefreshToken || refreshTokenExpiryDate < DateTime.Now.ToUniversalTime()) return null;

            var signingCredentials = jwtAuthenticationManager.GetSigningCredentials();
            var claims = await jwtAuthenticationManager.GetClaims(userAccount);

            var tokenOptions = jwtAuthenticationManager.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = jwtAuthenticationManager.GenerateRefreshToken();
            userAccount.RefreshToken = refreshToken;
            userAccount.Token = token;

            await _client.From<UserAccountSupabaseModel>().Update(userAccount);

            return new RefreshTokenDTO
            {
                Token = token,
                TokenExpiryTimeStamp = DateTime.Now.ToUniversalTime().AddMinutes(jwtAuthenticationManager.JWT_TOKEN_VALIDITY_MINS),
                RefreshToken = refreshToken,
                RefreshTokenExpiryTimeStamp = refreshTokenExpiryDate
            };
           

        }

        public async Task<ChangeDetailsResponse> ChangeDetails(ChangeDetailsRequest newUser, string token)
        {

            ChangeDetailsResponse result = new ChangeDetailsResponse();

          
            string usernamelowercase = newUser.UserName.ToLower();

            /*CHECKING FOR EXISTING USERS*/
            UserAccountSupabaseModel? userAccount = null;
            userAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == usernamelowercase && x.Token == token).Single();
            if (userAccount == null)
                {
                    result.result = false;
                    result.Message = "User was not found in the database!";
                    return result;
                }

                bool isMatching = _passwordHasher.Verify(userAccount.Password, newUser.Password);
                if (!isMatching)
                {
                    result.result = false;
                    result.Message = "The current password is incorrect!";
                    return result;
                }


                if (newUser.ChangeUserName)
                {

                string newusernamelowercase = newUser.NewUsername.ToLower();

                if (userAccount?.UserName == newusernamelowercase)
                    {
                        result.result = false;
                        result.Message = "The New username can't be the same as the old username!";
                        return result;
                    }
                UserAccountSupabaseModel? newUserNameAccount = null;
                newUserNameAccount =  await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == newusernamelowercase).Single();
                if (newUserNameAccount != null)
                    {
                        result.result = false;
                        result.Message = "The new Username is already taken!";
                        return result;
                    }

                }

                if (newUser.ChangeEmail)
                {
                string emaillowercase = newUser.NewEmail.ToLower();

                if (!Regex.IsMatch(newUser.NewEmail, @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$"))
                    {
                        result.result = false;
                        result.Message = "The new email's format is invalid!";
                        return result;
                    }

                    if (userAccount?.Email == emaillowercase)
                    {
                        result.result = false;
                        result.Message = "The New email can't be the same as the old email!";
                        return result;
                    }

                UserAccountSupabaseModel? newUserNameAccount = null;
                newUserNameAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.Email == emaillowercase).Single();
                if (newUserNameAccount != null)
                    {
                        result.result = false;
                        result.Message = "The new Email is already taken!";
                        return result;
                    }

                }


                /*IF EVERYTHING WENT WELL, CHANGING THE NEW ATTRIBUTES, THE DATABASE IS CONVERTING THE USERNAMES/EMAILS TO LOWERCASE WITH A BEFORE TRIGGER SO NO NEED TO CHECK*/

                if (newUser.ChangePassword)
                {
                    string? passwordHash = _passwordHasher.Hash(newUser.NewPassword);
                    userAccount.Password = passwordHash;
                }

                if (newUser.ChangeEmail)
                {
                    userAccount.Email = newUser.NewEmail;
                }

                if (newUser.ChangeUserName)
                {
                    userAccount.UserName = newUser.NewUsername;
                }


               
                _tokenBlacklistingService.BlacklistToken(userAccount.Token);


                var jwtAuthenticationManager = new JwtAuthenticationManager();
                UserSession userSession = jwtAuthenticationManager.GenerateJwtToken(userAccount);

                userAccount.Token = userSession.Token;
                userAccount.RefreshToken = userSession.RefreshToken;
                userAccount.RefreshTokenExpiryTime = DateTime.Now.ToUniversalTime().AddMinutes(jwtAuthenticationManager.JWT_REFRESH_TOKEN_VALIDITY_MINS).ToString("o");

                await _client.From<UserAccountSupabaseModel>().Update(userAccount);

                result.result = true;
                result.Message = "Credentials changed successfully!";
                result.UserSession = userSession;

                return result;


        }


        public async Task<DeleteAccountResponse> DeleteAccount(DeleteAccountRequest der, string token)
        {

            DeleteAccountResponse deleteAccountResponse = new DeleteAccountResponse();
            UserAccountSupabaseModel? userAccount = null;
            userAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == der.UserName && x.Token == token).Single();
            if (userAccount == null)
            {
                deleteAccountResponse.Message = "Either the account was not found, or the token was expired! You will be logged out! Try again";
                deleteAccountResponse.result = false;
                return deleteAccountResponse;
            }


            bool isMatching = _passwordHasher.Verify(userAccount.Password, der.Password);
            if (!isMatching)
            {
                deleteAccountResponse.Message = "The password is incorrect!";
                deleteAccountResponse.result = false;
                return deleteAccountResponse;
            }

            try
            {
                await _client.From<UserAccountSupabaseModel>().Delete(userAccount);


                deleteAccountResponse.Message = "Account has been deleted :(. You will be logged out now. Goodbye!";
                deleteAccountResponse.result = true;
                return deleteAccountResponse;
            }
            catch (Exception ex)
            {
                deleteAccountResponse.Message = "Something went wrong with the account deletion. Try again later!";
                deleteAccountResponse.result = false;
                return deleteAccountResponse;
            }
        }



        public async Task<bool> RateAnime(AnimeActionTransfer aat, string? token)
        {
            string usernamelowercase = aat.UserName.ToLower();

            UserAccountSupabaseModel? userAccount = null;
            userAccount = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == usernamelowercase && x.Token == token).Single();
            if (userAccount == null) return false;

            AnimeUserSupabaseModel? db_animeUser = await _client.From<AnimeUserSupabaseModel>().Where(x => x.AnimeId == aat.AnimeId && x.UserId == userAccount.Id).Single();
            
            if (db_animeUser != null && aat.Status == null)
            {
                await _client.From<AnimeUserSupabaseModel>().Delete(db_animeUser);
                return true;
            }
            else if (db_animeUser == null && aat.Status == null)
            {
                return true;
            }

            AnimeUserSupabaseModel updated_entry;

            if (db_animeUser == null)
            {
                updated_entry = new AnimeUserSupabaseModel
                {
                    AnimeId = aat.AnimeId,
                    UserId = userAccount.Id,
                    favorite = aat.Favorite,
                    Status = aat.Status,
                    Rating = aat.Rating,
                    Date = DateTime.Now.ToUniversalTime().ToString(),
                };
                await _client.From<AnimeUserSupabaseModel>().Insert(updated_entry);
            }
            else
            {
                if (aat.Status != db_animeUser.Status)
                {
                    db_animeUser.Date = DateTime.Now.ToUniversalTime().ToString();
                }

                db_animeUser.favorite = aat.Favorite;
                db_animeUser.Status = aat.Status;
                db_animeUser.Rating = aat.Rating;

                await _client.From<AnimeUserSupabaseModel>().Update(db_animeUser);
            }
            return true;
           
        }


        public async Task<Dictionary<int, InteractedAnime>?> GetInteractedAnimes(string UserName, string token)
        {
            string usernamelowercase = UserName.ToLower();

            UserAccountSupabaseModel? db_UserExists = await _client.From<UserAccountSupabaseModel>().Where(x => x.UserName == usernamelowercase && x.Token == token).Single();
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
