using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FarmTradeEntity;

namespace FarmApi.Model
{
    public class Product1
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public decimal ProductPrice { get; set; }

            public string Description { get; set; }

            public int AvailableQuantity { get; set; }
            //public ReviewsAndRatings review { get; set; }

    }
}
