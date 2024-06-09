
using System.Security.Cryptography;

namespace BattAnimeZone.Authentication.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 32;
        private const int Keysize = 64;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';
        string IPasswordHasher.Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, Keysize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        bool IPasswordHasher.Verify(string passwordHash, string inputPassword)
        {
            var elements = passwordHash.Split(Delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);
            var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, Keysize);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }

    }
}
