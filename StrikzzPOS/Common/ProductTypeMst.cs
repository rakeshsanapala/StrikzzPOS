using System.ComponentModel.DataAnnotations;

    namespace StrikzzPOS.Common
    {
        public class ProductTypeMst
        {
            [Key]
            public int pk_prodtypeid { get; set; }
            public string Description { get; set; }
        }
    }