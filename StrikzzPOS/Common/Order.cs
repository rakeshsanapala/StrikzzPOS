using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

    namespace StrikzzPOS.Common
    {
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int FK_PaymentTypeId { get; set; }
        public int FK_CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? OrderCancelDate { get; set; }
        public double FinalTotal { get; set; }
        public string OrderStatus { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

    }
}
