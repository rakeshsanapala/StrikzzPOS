using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects.DataClasses;

namespace StrikzzPOS.Common
    {
    [Table("Orders")]
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public int FK_PaymentTypeId { get; set; }
        public int FK_CustomerId { get; set; }

        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public string CustomerPhone { get; set; }

        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? OrderCancelDate { get; set; }
        public double FinalTotal { get; set; }
        public string OrderStatus { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }

    }
}
