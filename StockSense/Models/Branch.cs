using System.ComponentModel.DataAnnotations;

namespace StockSense.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public int BranchType { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
