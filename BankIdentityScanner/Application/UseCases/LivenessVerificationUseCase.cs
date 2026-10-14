using System;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using BankIdentityScanner.Infrastructure.Services;

namespace BankIdentityScanner.Application.UseCases
{
    [SupportedOSPlatform("windows")]
    public class LivenessVerificationUseCase
    {
        private readonly TtsService _ttsService;
        private readonly LivenessAnalyzer _livenessAnalyzer;
        private readonly Random _random;

        public LivenessVerificationUseCase()
        {
            _ttsService = new TtsService();
            _livenessAnalyzer = new LivenessAnalyzer();
            _random = new Random();
        }

        // Kullanıcıdan rastgele bir hareket ister ve bunu sesli söyler
        public async Task StartLivenessCheckAsync()
        {
            string[] actions = { "Lütfen sağa dönün.", "Lütfen sola dönün.", "Lütfen kameraya bakarak göz kırpın." };

            // Rastgele bir eylem seç (0, 1 veya 2)
            int selectedActionIndex = _random.Next(actions.Length);
            string requestedAction = actions[selectedActionIndex];

            // 1. Kullanıcıyı sesli olarak yönlendir (Asenkron)
            Console.WriteLine($"[SİSTEM]: {requestedAction}");
            await _ttsService.SpeakAsync(requestedAction);

            // 2. Kamera akışını dinleme simülasyonu (Burada normalde kameradan sürekli frame gelir)
            bool isVerified = false;

            if (selectedActionIndex == 0) // Sağa Dönme Bekleniyor
            {
                // Örnek: Kameradan EulerY = 40.5 derece geldiğini varsayalım
                string direction = _livenessAnalyzer.GetHeadTurnDirection(40.5f);
                if (direction == "RIGHT") isVerified = true;
            }
            else if (selectedActionIndex == 1) // Sola Dönme Bekleniyor
            {
                // Örnek: Kameradan EulerY = -40.5 derece geldiğini varsayalım
                string direction = _livenessAnalyzer.GetHeadTurnDirection(-40.5f);
                if (direction == "LEFT") isVerified = true;
            }
            else if (selectedActionIndex == 2) // Göz Kırpma Bekleniyor
            {
                // Örnek: Kameradan hesaplanan EAR değerinin 0.15 geldiğini varsayalım
                bool isBlink = _livenessAnalyzer.IsBlinking(0.15);
                if (isBlink) isVerified = true;
            }

            // 3. Sonucu bildir
            if (isVerified)
            {
                Console.WriteLine("[SİSTEM]: Doğrulama Başarılı!");
                await _ttsService.SpeakAsync("Canlılık doğrulaması başarıyla tamamlandı.");
            }
            else
            {
                Console.WriteLine("[SİSTEM]: Doğrulama Başarısız!");
                await _ttsService.SpeakAsync("Doğrulama başarısız, lütfen tekrar deneyin.");
            }
        }
    }
}