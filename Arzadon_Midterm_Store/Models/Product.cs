using System.ComponentModel.DataAnnotations;

namespace Arzadon_Midterm_Store.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;
    }
}