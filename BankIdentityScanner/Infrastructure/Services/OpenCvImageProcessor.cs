using OpenCvSharp;
using BankIdentityScanner.Domain.Interfaces;

namespace BankIdentityScanner.Infrastructure.Services
{
    public class OpenCvImageProcessor : IImageProcessor
    {
        public byte[] ApplyAdaptiveThresholding(byte[] imageBytes)
        {
            // Byte dizisini OpenCV Mat nesnesine çevir
            using var src = Cv2.ImDecode(imageBytes, ImreadModes.Color);
            using var gray = new Mat();
            using var threshed = new Mat();

            // 1. Görüntüyü Gri Tonlamaya (Grayscale) Çevir
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 2. Adaptive Thresholding Uygula (Gölge ve parlamaları yok etmek için)
            Cv2.AdaptiveThreshold(gray, threshed, 255,
                AdaptiveThresholdTypes.GaussianC,
                ThresholdTypes.Binary, 11, 2);

            // İşlenmiş görüntüyü tekrar byte dizisine çevir
            return threshed.ToBytes();
        }
    }
}