namespace BankIdentityScanner.Domain.Models
{
    public class MrzResult
    {
        public string? RawText { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}