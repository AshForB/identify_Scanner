using System;
using Tesseract;
using BankIdentityScanner.Domain.Interfaces;
using BankIdentityScanner.Domain.Models;

namespace BankIdentityScanner.Infrastructure.Services
{
    // 1. Sınıfımızın yanına IDisposable ekliyoruz
    public class TesseractOcrScanner : IOcrScanner, IDisposable
    {
        private readonly TesseractEngine _engine;
        private bool _disposed = false;

        public TesseractOcrScanner()
        {
            _engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
            _engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789<");
        }

        public MrzResult ScanMrzZone(byte[] processedImageBytes)
        {
            // 2. 'using' bloğu çok kritiktir! İşlem bittiği an img ve page nesnelerini RAM'den siler.
            using (var img = Pix.LoadFromMemory(processedImageBytes))
            {
                using (var page = _engine.Process(img))
                {
                    return new MrzResult { RawText = page.GetText().Trim(), IsSuccessful = true };
                }
            }
        }

        // 3. --- DÜNYA STANDARDI DISPOSE (TEMİZLİK) KALIBI ---

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Çöp toplayıcıya "Ben temizledim, sen yorulma" diyoruz.
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Unmanaged (C++ tabanlı) kaynakları RAM'den kesin olarak siliyoruz
                if (_engine != null)
                {
                    _engine.Dispose();
                }

                _disposed = true;
            }
        }

        // Yıkıcı metot: Eğer programcı Dispose metodunu çağırmayı unutursa sistem son çare olarak bunu tetikler
        ~TesseractOcrScanner()
        {
            Dispose(false);
        }
    }
}