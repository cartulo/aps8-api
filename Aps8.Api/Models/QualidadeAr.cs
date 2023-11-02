namespace Aps8.Api.Models
{
    public class QualidadeAr
    {
        public required string Country { get; set; }
        public required string City { get; set; }
        public required decimal Count { get; set; }
        public required decimal Locations { get; set; }
        public required DateTime FirstUpdated { get; set; }
        public required DateTime LastUpdated { get; set; }
        public string[] Parameters { get; set; } = new string[0];
    }

    public class RetornoApi
    {
        public required object Meta { get; set; }
        public List<QualidadeAr> Results { get; set; } = new List<QualidadeAr>();
    }
}