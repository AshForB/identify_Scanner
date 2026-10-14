namespace BankIdentityScanner.Domain.Interfaces
{
    public interface IImageProcessor
    {
        byte[] ApplyAdaptiveThresholding(byte[] imageBytes);
    }
}