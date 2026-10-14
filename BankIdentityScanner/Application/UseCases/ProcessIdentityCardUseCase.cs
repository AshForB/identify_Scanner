using System;
using BankIdentityScanner.Domain.Interfaces;

namespace BankIdentityScanner.Application.UseCases
{
    public class ProcessIdentityCardUseCase
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IOcrScanner _ocrScanner;

        public ProcessIdentityCardUseCase(IImageProcessor imageProcessor, IOcrScanner ocrScanner)
        {
            _imageProcessor = imageProcessor;
            _ocrScanner = ocrScanner;
        }

        public string Execute(byte[] rawCameraImage)
        {
            // 1. OCR ile Metni Oku
            var processedImage = _imageProcessor.ApplyAdaptiveThresholding(rawCameraImage);
            var result = _ocrScanner.ScanMrzZone(processedImage);

            if (result.IsSuccessful && !string.IsNullOrEmpty(result.RawText))
            {
                // 2. YENİ EKLENEN KISIM: Okunan metni doğrula
                var validator = new BankIdentityScanner.Infrastructure.Services.MrzValidator();
                bool isRealId = validator.ValidateMrzLine(result.RawText);

                if (isRealId)
                {
                    return "KİMLİK DOĞRULANDI! Okunan Metin: " + result.RawText;
                }
                else
                {
                    throw new Exception("UYARI: Kimlik numaraları matematiksel doğrulamadan geçemedi! Sahte veya yanlış okuma.");
                }
            }

            throw new Exception("Kimlik okuma başarısız: " + result.ErrorMessage);
        }
    }
}