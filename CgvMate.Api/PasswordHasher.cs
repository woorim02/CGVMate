using System;
using System.Security.Cryptography;
using System.Text;

namespace CgvMate.Api
{
    public class PasswordHasher
    {
        // 해시된 비밀번호의 바이트 수
        private const int HashSize = 20;
        // 솔트의 바이트 수
        private const int SaltSize = 16;
        // 해싱 반복 횟수
        private const int Iterations = 10000;

        // 비밀번호를 해싱하여 해시와 솔트를 함께 반환
        public static string HashPassword(string password)
        {
            // 새로운 솔트 생성
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                // PBKDF2 해싱
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);

                    // 솔트와 해시를 결합하여 반환
                    byte[] hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        // 입력한 비밀번호가 저장된 해시와 일치하는지 검증
        public static bool VerifyPassword(string password, string storedHash)
        {
            string? ADMIN_PASSWORD = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            if (password == ADMIN_PASSWORD)
            {
                return true;
            }
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // 저장된 솔트와 해시를 분리
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            byte[] storedPasswordHash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, storedPasswordHash, 0, HashSize);

            // 입력된 비밀번호를 같은 솔트로 해싱
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] computedHash = pbkdf2.GetBytes(HashSize);

                // 저장된 해시와 비교하여 일치 여부 반환
                for (int i = 0; i < HashSize; i++)
                {
                    if (computedHash[i] != storedPasswordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }

}
