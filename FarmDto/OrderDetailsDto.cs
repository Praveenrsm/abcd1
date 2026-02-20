using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDto
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public AddressDto Address { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
