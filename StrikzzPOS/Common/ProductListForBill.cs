﻿
namespace StrikzzPOS.Common
{
    public class ProductListForBill
    {
        public int id { get; set; }
        public string username { get; set; }
        public int fk_custId { get; set; }
        public int Fk_ProductId { get; set; }
        public double productQuantity { get; set; }
        public double price { get; set; }

    }
}