using System;

namespace BankIdentityScanner.Infrastructure.Services
{
    // Yüzdeki bir referans noktasını temsil eden basit struct
    public struct Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class LivenessAnalyzer
    {
        // İki nokta arasındaki Öklid mesafesini hesaplar
        private double CalculateDistance(Point2D p1, Point2D p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        // Göz En-Boy Oranını (EAR) Hesaplarken formülü uygular
        public double CalculateEAR(Point2D p1, Point2D p2, Point2D p3, Point2D p4, Point2D p5, Point2D p6)
        {
            double vertical1 = CalculateDistance(p2, p6);
            double vertical2 = CalculateDistance(p3, p5);
            double horizontal = CalculateDistance(p1, p4);

            if (horizontal == 0) return 0; // Sıfıra bölünme hatasını önle

            return (vertical1 + vertical2) / (2.0 * horizontal);
        }

        // EAR eşik değerine göre göz kırpma durumunu döner
        public bool IsBlinking(double earValue, double threshold = 0.20)
        {
            return earValue < threshold;
        }

        // Euler Y açısına göre baş hareketini tespit eder
        public string GetHeadTurnDirection(float eulerY)
        {
            if (eulerY > 35) return "RIGHT";
            if (eulerY < -35) return "LEFT";
            return "CENTER";
        }
    }
}