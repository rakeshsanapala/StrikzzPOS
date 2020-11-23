using System.ComponentModel.DataAnnotations;

    namespace StrikzzPOS.Common
    {
    public class OrderDetail
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