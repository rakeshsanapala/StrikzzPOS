using StrikzzPOS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StrikzzPOS.DTO
{
    public class Com_Bill_DTO
    {
        public Com_Bill_DTO()
        {
            ProductList = new List<ProductDDD_DTO>();
            PaymentTypes = new List<PaymentTypes>();
        }
        public int pk_tempbillid { get; set; }

        
        [Display(Name = "Product")]
        public int Fk_ProductId { get; set; }

        [Required(ErrorMessage = "Payment type required.")]
        [Display(Name = "Payment Type")]
        public int Fk_PaymentTypeId { get; set; }

        [Display(Name = "Quantity")]
        public double prodQuantity { get; set; }

        [Display(Name = "Price")] public double price { get; set; }

        public CustomerMst CustomerMst { get; set; }

        public IEnumerable<ProductDDD_DTO> ProductList { get; set; }

        
        public IEnumerable<PaymentTypes> PaymentTypes { get; set; }

    }
}