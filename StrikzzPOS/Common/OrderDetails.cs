using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace StrikzzPOS.Common
    {
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int FK_OrderId { get; set; }
        public int FK_ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }

    }
}