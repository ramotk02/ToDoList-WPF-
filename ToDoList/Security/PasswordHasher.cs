using System;
using System.Security.Cryptography;

namespace ToDoApp.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;        // 128-bit
        private const int KeySize = 32;         // 256-bit (32 bytes)
        private const int Iterations = 120_000; // OK pour desktop

        public static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(KeySize);

            return (hash, salt);
        }

        public static bool Verify(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (storedHash == null || storedSalt == null) return false;

            using var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA256);
            byte[] computed = pbkdf2.GetBytes(storedHash.Length);

            return CryptographicOperations.FixedTimeEquals(computed, storedHash);
        }
    }
}
