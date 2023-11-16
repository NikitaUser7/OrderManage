using orderManageApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.Models
{
    public class SetOrderModel
    {
        public string OrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string CustomName { get; set; }
        public string CustomSurname { get; set; }
        public string CustomPatronymic { get; set; }
        public SetOrder_PositionModel[] Positions { get; set; }
    }
}
