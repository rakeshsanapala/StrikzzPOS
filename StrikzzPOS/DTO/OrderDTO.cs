using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrikzzPOS.DTO
{
    public class OrderDTO
    {
        [Key]
        public int OrderId { get; set; }
        public int FK_PaymentTypeId { get; set; }
        public int FK_CustomerId { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Order#")]
        public string OrderNumber { get; set; }
        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }
        [Display(Name = "Order Cancel Date")]
        public DateTime? OrderCancelDate { get; set; }
        [Display(Name = "Order Total")]
        public double FinalTotal { get; set; }
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }

        public IEnumerable<OrderDetailDTO> OrderDetails { get; set; }

    }
}
