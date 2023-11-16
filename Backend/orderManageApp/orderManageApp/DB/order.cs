using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.DB
{
    public class order
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        [Required]
        public string CustomName { get; set; }
        [Required]
        public string CustomSurname { get; set; }
        public string CustomPatronymic { get; set; }
        [Required]
        public decimal SumMoney { get; set; }
        public virtual List<order_position> Positions { get; set; }
        public bool Removed { get; set; } = false;
    }
}
