using System;
using System.Text.RegularExpressions;

namespace BankIdentityScanner.Infrastructure.Services
{
    public class MrzValidator
    {
        // Karakterlerin ICAO 9303'e göre sayısal karşılıklarını (Ağırlıklarını) hesaplar
        private int GetCharacterValue(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'Z') return c - 'A' + 10;
            if (c == '<') return 0;
            throw new ArgumentException($"Geçersiz MRZ karakteri: {c}");
        }

        // 7-3-1 algoritması ile Mod 10 hesaplamasını yapar
        public bool VerifyCheckDigit(string data, char expectedCheckDigit)
        {
            int[] weights = { 7, 3, 1 };
            int sum = 0;

            for (int i = 0; i < data.Length; i++)
            {
                int value = GetCharacterValue(data[i]);
                sum += value * weights[i % 3]; // 7, 3, 1 sırasıyla çarpıp topluyoruz
            }

            int calculatedDigit = sum % 10; // Çıkan sonucun 10'a bölümünden kalanı alıyoruz
            int expectedDigit = GetCharacterValue(expectedCheckDigit);

            return calculatedDigit == expectedDigit;
        }

        // Metni parçalayıp doğrulamayı çalıştıran ana fonksiyon
        public bool ValidateMrzLine(string mrzLine)
        {
            // Doğum tarihi (6 hane) + Kontrol (1 hane) ve Bitiş tarihi (6 hane) + Kontrol (1 hane) yakalayan Regex
            var regex = new Regex(@"([0-9]{6})([0-9])([M|F|<])([0-9]{6})([0-9])");
            var match = regex.Match(mrzLine);

            if (!match.Success) return false;

            string birthDate = match.Groups[1].Value;
            char birthDateCheck = match.Groups[2].Value[0];

            string expiryDate = match.Groups[4].Value;
            char expiryDateCheck = match.Groups[5].Value[0];

            // Yukarıdaki Mod 10 fonksiyonuna gönderip test ediyoruz
            bool isBirthDateValid = VerifyCheckDigit(birthDate, birthDateCheck);
            bool isExpiryDateValid = VerifyCheckDigit(expiryDate, expiryDateCheck);

            // İkisi de doğruysa true (başarılı) döner
            return isBirthDateValid && isExpiryDateValid;
        }
    }
}