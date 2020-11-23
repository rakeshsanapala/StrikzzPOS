using System.ComponentModel.DataAnnotations;

namespace StrikzzPOS.Common
{
    public class PaymentTypes
    {
        [Key]
        public int pk_PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
    }
}