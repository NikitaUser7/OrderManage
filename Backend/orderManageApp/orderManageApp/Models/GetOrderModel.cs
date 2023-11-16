using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.Models
{
    public class GetOrderModel
    {
        public string sortByColumn { get; set; }

        public bool descending { get; set; } = false;

        public string searchByColumn { get; set; }

        public SearchOrderModel searchAction{ get; set; }

        public int? pageShow { get; set; }

        public int numOfDispLines { get; set; }
    }
}
