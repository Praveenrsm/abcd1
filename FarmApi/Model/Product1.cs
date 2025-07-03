using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FarmApi.Model
{
    public class Product1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        //[AllowNull]
        //public byte[] productImage { get; set; }

        public decimal productPrice { get; set; }
        public string Description { get; set; }
        public int availableQuantity { get; set; }
    }
}
