using BankIdentityScanner.Domain.Models;

namespace BankIdentityScanner.Domain.Interfaces
{
    public interface IOcrScanner
    {
        MrzResult ScanMrzZone(byte[] processedImageBytes);
    }
}