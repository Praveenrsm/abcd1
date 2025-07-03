using System.ComponentModel.DataAnnotations;

namespace razorpractice.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, 10000)]
        public decimal Price { get; set; }
    }
}
