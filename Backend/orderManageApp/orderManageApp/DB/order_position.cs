using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.DB
{
    public class order_position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        public int ProductSku { get; set; }
        [Required]
        public decimal SumMoney { get; set; }
        public long OrderID { get; set; }
        public virtual order Order { get; set; }
        public bool Removed { get; set; } = false;
    }
}
