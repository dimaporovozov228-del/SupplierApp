using System;
using System.Security.Cryptography;

namespace SupplierApp
{
    public static class PasswordHasher
    {
        public const int DefaultIterations = 600000;

        public static void Create(string password, out string salt, out string hash, out int iterations)
        {
            if (password == null) throw new ArgumentNullException("password");
            byte[] saltBytes = new byte[16];
            using (var random = RandomNumberGenerator.Create()) random.GetBytes(saltBytes);
            iterations = DefaultIterations;
            salt = Convert.ToBase64String(saltBytes);
            using (var derive = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256))
                hash = Convert.ToBase64String(derive.GetBytes(32));
        }

        public static bool Verify(string password, string salt, string hash, int iterations)
        {
            if (password == null || iterations < 10000 || iterations > 2000000) return false;
            try
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                byte[] expected = Convert.FromBase64String(hash);
                if (saltBytes.Length != 16 || expected.Length != 32) return false;
                byte[] actual;
                using (var derive = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256))
                    actual = derive.GetBytes(32);
                int difference = 0;
                for (int index = 0; index < actual.Length; index++) difference |= actual[index] ^ expected[index];
                return difference == 0;
            }
            catch (FormatException) { return false; }
            catch (ArgumentException) { return false; }
        }
    }
}
