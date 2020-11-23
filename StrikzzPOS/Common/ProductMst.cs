using System.ComponentModel.DataAnnotations;

namespace StrikzzPOS.Common
{
    public class ProductMst
    {
         [Key]
        public int pk_ProductId { get; set; }
        public string username { get; set; }

        [Display(Name = "Product Type")] public int fk_prodtypeid { get; set; }

        [Display(Name = "Product Name")] public string ProductName { get; set; }

        public double oriPrice { get; set; }

        [Display(Name = "Price")] public double sellingUpToPrice { get; set; }

        public double productQuantity { get; set; }

    }
}