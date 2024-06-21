using System.Collections.Concurrent;


namespace BattAnimeZone.Services
{

    public interface ITokenBlacklistingService
    {
        void BlacklistToken(string token);
        bool IsTokenBlacklisted(string token);
    }

    public class TokenBlacklistingService : ITokenBlacklistingService, IDisposable
    {
        private readonly ConcurrentDictionary<string, DateTime> _blacklist = new ConcurrentDictionary<string, DateTime>();
        private readonly Timer _cleanupTimer;
        private readonly TimeSpan _tokenLifetime = TimeSpan.FromMinutes(10);

        public TokenBlacklistingService()
        {
           
            _cleanupTimer = new Timer(CleanupExpiredTokens, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        }


        public void BlacklistToken(string token)
        {
            _blacklist[token] = DateTime.UtcNow.ToUniversalTime().Add(_tokenLifetime);

        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklist.ContainsKey(token);
        }

        private void CleanupExpiredTokens(object? state)
        {

            var now = DateTime.UtcNow.ToUniversalTime();
            var expiredTokens = _blacklist.Where(kvp => kvp.Value <= now).Select(kvp => kvp.Key).ToList();

            foreach (var token in expiredTokens)
            {
                _blacklist.TryRemove(token, out _);
            }

        }

        public void Dispose()
        {
            _cleanupTimer?.Dispose();
        }
    }
}
