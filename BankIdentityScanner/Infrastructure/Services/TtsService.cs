using System.Runtime.Versioning; // 1. YENİ EKLENEN KÜTÜPHANE
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace BankIdentityScanner.Infrastructure.Services
{
    [SupportedOSPlatform("windows")] // 2. YENİ EKLENEN ETİKET
    public class TtsService
    {
        // Metin okuma işlemini arka plan iş parçacığında (Background Thread) çalıştırır
        public async Task SpeakAsync(string text)
        {
            await Task.Run(() =>
            {
                using var synthesizer = new SpeechSynthesizer();
                // Windows'taki varsayılan kadın/erkek sesini kullanır
                synthesizer.SelectVoiceByHints(VoiceGender.Female);
                synthesizer.Rate = 0; // Okuma hızı (-10 ile 10 arası)
                synthesizer.Volume = 100; // Ses seviyesi

                synthesizer.Speak(text);
            });
        }
    }
}