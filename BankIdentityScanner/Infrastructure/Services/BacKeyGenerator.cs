using System;
using System.Security.Cryptography;
using System.Text;

namespace BankIdentityScanner.Infrastructure.Services
{
    public class BacKeyGenerator
    {
        // NFC Çipine girmek için gereken şifreyi türetir
        public byte[] DeriveSessionKey(string documentNumber, string birthDate, string expiryDate)
        {
            // 1. Kimlik numarası, doğum tarihi ve bitiş tarihini yan yana birleştiriyoruz
            string mrzInfo = $"{documentNumber}{birthDate}{expiryDate}";

            // 2. Bu birleşimi SHA-1 algoritması ile geri döndürülemez şekilde (Hash) şifreliyoruz
            using var sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(mrzInfo));

            // 3. Çıkan şifrenin sadece ilk 16 Byte'lık kısmını kesip alıyoruz. Çipin anahtarı budur!
            byte[] keySeed = new byte[16];
            Array.Copy(hash, 0, keySeed, 0, 16);

            return keySeed;
        }
    }
}