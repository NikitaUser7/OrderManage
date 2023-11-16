using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.Models
{
    public class ResponseOrderModel
    {
        public long ID { get; set; }
        public string CreationDate { get; set; }
        public string OrderNumber { get; set; }
        public string CustomName { get; set; }
        public string CustomSurname { get; set; }
        public string CustomNSP 
        {
            get
            {
                string val = CustomName + " " + CustomSurname + " " + CustomPatronymic;
                return val;
            }
        }
        public string CustomPatronymic { get; set; }
        public string SumMoney { get; set; }
    }
}
