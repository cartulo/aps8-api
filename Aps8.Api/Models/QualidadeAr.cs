namespace Aps8.Api.Models
{
    public class QualidadeAr
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public decimal Count { get; set; }
        public decimal Locations { get; set; }
        public DateTime FirstUpdated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string[] Parameters { get; set; } = Array.Empty<string>();
    }

    public class RetornoApi
    {
        public object? Meta { get; set; }
        public List<QualidadeAr> Results { get; set; } = new List<QualidadeAr>();
    }
}
