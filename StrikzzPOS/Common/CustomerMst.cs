using System;
using System.ComponentModel.DataAnnotations;

namespace StrikzzPOS.Common
{
    public class CustomerMst
    {
        [Key]
        public int pk_Custid { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone No.")]
        [Required]
        public string MobNo { get; set; }

        [Display(Name = "First Visit")]
        public DateTime FirstVisit { get; set; }

        [Display(Name = "Last Visit")]
        public DateTime LastVisit { get; set; }

        [Display(Name = "Total Visits")]
        public int TotalVisits { get; set; }

        [Display(Name = "Total Spent")]
        public decimal TotalSpent { get; set; }

        [Display(Name = "Point Balance")]
        public long PointBalance { get; set; }

    }
}