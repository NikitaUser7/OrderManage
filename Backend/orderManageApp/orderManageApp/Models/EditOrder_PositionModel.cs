using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.Models
{
    public class EditOrder_PositionModel
    {
        public long PositionID { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public int ProductSku { get; set; }
        public string SumMoney { get; set; }
    }
}
