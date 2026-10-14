using System;
using Moq;
using Xunit;
using BankIdentityScanner.Application.UseCases;
using BankIdentityScanner.Domain.Interfaces;
using BankIdentityScanner.Domain.Models;

namespace BankIdentityScanner.Tests
{
    public class ProcessIdentityCardUseCaseTests
    {
        [Fact] // Bu etiket, bunun bir test metodu olduðunu belirtir
        public void GecerliKimlik_Okundugunda_BasariliSonuc_Donmeli()
        {
            // 1. HAZIRLIK (Arrange): Sahte (Mock) nesneleri üretiyoruz. Gerçek kameraya ihtiyacýmýz yok.
            var mockImageProcessor = new Mock<IImageProcessor>();
            var mockOcrScanner = new Mock<IOcrScanner>();

            // Sahte bir MRZ metni ve sonucu hazýrlýyoruz (Mod 10 algoritmasýndan geçebilen doðru bir metin)
            string gecerliMrz = "9001011M9001011";
            var sahteSonuc = new MrzResult { IsSuccessful = true, RawText = gecerliMrz };

            // Sistemi kandýrýyoruz: "Ne zaman ScanMrzZone çaðrýlýrsa, benim hazýrladýðým sahteSonuc'u ver" diyoruz.
            mockOcrScanner.Setup(x => x.ScanMrzZone(It.IsAny<byte[]>())).Returns(sahteSonuc);

            // Test edilecek asýl iþ sýnýfýmýzý, bu sahte nesnelerle ayaða kaldýrýyoruz
            var useCase = new ProcessIdentityCardUseCase(mockImageProcessor.Object, mockOcrScanner.Object);

            // 2. EYLEM (Act): Sýnýfý çalýþtýrýyoruz
            byte[] sahteKameraGoruntusu = new byte[0];
            var sonuc = useCase.Execute(sahteKameraGoruntusu);

            // 3. DOÐRULAMA (Assert): Sonuç beklediðimiz gibi mi?
            Assert.Contains("KÝMLÝK DOÐRULANDI", sonuc);
        }
    }
}