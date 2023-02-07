using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrikzzPOS.Common
    {
    [Table("UserProduct")]
    public class UserProduct
    {
        [Key]
        [Required]
        public int UserProductId { get; set; }

        [Required]
        public int fk_productId { get; set; }

        [Required]
        public string fk_UserId { get; set; }

    }
}
