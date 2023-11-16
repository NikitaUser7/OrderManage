using System;
using orderManageApp.Models;

namespace orderManageApp.Models
{
    public class EditOrderModel
    {
        public long OrderID { get; set; }
        public string OrderNumber { get; set; }
        public string CreationDate { get; set; }
        public string CustomName { get; set; }
        public string CustomSurname { get; set; }
        public string CustomPatronymic { get; set; }
        public string SumMoney { get; set; }
        public EditOrder_PositionModel[] Positions { get; set; }
    }
}
