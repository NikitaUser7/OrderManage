using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.Models
{
    public class ResponseOreder_PositionModel
    {
        public long ID { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public int ProductSku { get; set; }
        public string SumMoney { get; set; }
        public long OrderID { get; set; }

    }
}
