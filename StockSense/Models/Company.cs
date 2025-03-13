using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockSense.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? TagLine { get; set; }
        public string? VatRegistrationNo { get; set; }
        public string? TinNo { get; set; }
        public string? WebsiteLink { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? Remarks { get; set; }
        public string? LogoPath { get; set; }
        [NotMapped]
        public IFormFile? LogoImage { get; set; }
    }
}
