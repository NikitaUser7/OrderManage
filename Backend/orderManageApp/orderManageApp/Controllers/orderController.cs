using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using orderManageApp.DB;
using orderManageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace orderManageApp.Controllers
{
    public class orderController
    {
        private readonly ILogger<orderController> _logger;

        private readonly orderAppContext _orderAppContext;

        public orderController(ILogger<orderController> logger, orderAppContext orderAppcontext)
        {
            _orderAppContext = orderAppcontext;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<long> GetOrdersCount([FromBody] GetOrderModel data)
        {
            var query = _orderAppContext.Orders
                .Include(d => d.Positions)
                .Where(d => d.Removed == false);
            var t = typeof(SearchOrderModel);
            var props = t.GetProperties();
            bool isSearch = false;
            foreach (var a in props)
            {
                if (a.GetValue(data.searchAction) == null || a.GetValue(data.searchAction) == "")
                {
                    isSearch = false;
                }
                else { isSearch = true; break; }
            }
            if (isSearch)
            {
                query = data.searchByColumn switch
                {
                    "creationDate" => query.Where(d =>
                    d.CreationDate == data.searchAction.Date),
                    "orderNumber" => query.Where(d => d.OrderNumber == data.searchAction.String),
                    "customSurname" => query.Where(d => d.CustomSurname.Contains(data.searchAction.String)),
                    "positionName" => query.Where(d => d.Positions.Where(b => b.Removed == false && b.OrderID
                    == d.ID).Any(g => g.ProductName.Contains(data.searchAction.String))),
                    "dateRange" => query.Where(d => d.CreationDate > data.searchAction.DateA && d.CreationDate < data.searchAction.DateZ)
                };

            }
            return query.Count();
        }

        [HttpPost]
        public ActionResult<ResponseOrderModel[]> GetOrders([FromBody] GetOrderModel data)
        {
            var query = _orderAppContext.Orders
                .Include(d => d.Positions.Where(f => f.Removed == false))
                .Where(d => d.Removed == false);
            var t = typeof(SearchOrderModel);
            var props = t.GetProperties();
            bool isSearch = false;
            foreach (var a in props)
            {
                if (a.GetValue(data.searchAction) == null || a.GetValue(data.searchAction) == "")
                {
                    isSearch = false;
                }
                else { isSearch = true; break; }
            }

            query = data.sortByColumn switch
            {
                "creationDate" => data.descending
                    ? query.OrderByDescending(d => d.CreationDate)
                    : query.OrderBy(d => d.CreationDate),
                "customNSP" => data.descending
                    ? query.OrderByDescending(d => d.CustomSurname)
                    : query.OrderBy(d => d.CustomSurname),
                "sumMoney" => data.descending
                    ? query.OrderByDescending(d => d.SumMoney)
                    : query.OrderBy(d => d.SumMoney),
                    null => data.descending
                    ? query.OrderByDescending(d => d.CreationDate)
                    : query.OrderBy(d => d.CreationDate)
            };

            if (isSearch)
            {
                query = data.searchByColumn switch
                {
                    "creationDate" => query.Where(d =>
                    d.CreationDate == data.searchAction.Date),
                    "orderNumber" => query.Where(d => d.OrderNumber == data.searchAction.String),
                    "customSurname" => query.Where(d => d.CustomSurname.Contains(data.searchAction.String)),
                    "positionName" => query.Where(d => d.Positions.Where(b => b.Removed == false && b.OrderID == d.ID).Any(g => g.ProductName.Contains(data.searchAction.String))),
                    "dateRange" => query.Where(d => d.CreationDate > data.searchAction.DateA && d.CreationDate < data.searchAction.DateZ)
                };

            }

            if (data.numOfDispLines != 0 && data.numOfDispLines != null)
            {
                int page = data.pageShow.HasValue ? data.pageShow.Value : 0;

                query = query.Skip(page * data.numOfDispLines);

                var q = query
                    .Take(data.numOfDispLines)
                    .Select(d => new ResponseOrderModel()
                    {
                        ID = d.ID,
                        CreationDate = d.CreationDate.ToString("yyyy-MM-dd"),
                        OrderNumber = d.OrderNumber,
                        CustomName = d.CustomName,
                        CustomSurname = d.CustomSurname,
                        CustomPatronymic = d.CustomPatronymic,
                        SumMoney = d.SumMoney.ToString()

                    })
                    .ToArray();
                return q;
            }

            else
            {
                var q = query
                    .Select(d => new ResponseOrderModel()
                    {
                        ID = d.ID,
                        CreationDate = d.CreationDate.ToString("yyyy-MM-dd"),
                        OrderNumber = d.OrderNumber,
                        CustomName = d.CustomName,
                        CustomSurname = d.CustomSurname,
                        CustomPatronymic = d.CustomPatronymic,
                        SumMoney = d.SumMoney.ToString()

                    })
                    .ToArray();
                return q;
            }
        }
        [HttpPost]
        public ActionResult<ResponseOreder_PositionModel[]> GetOrderPositions([FromBody] long OrderID)
        {
                var query = _orderAppContext.Order_positions.Where(d => d.Removed == false && d.OrderID == OrderID);
                var q = query.Select(d => new ResponseOreder_PositionModel()
                {
                    ID = d.ID,
                    ProductName = d.ProductName,
                    ProductPrice = d.ProductPrice.ToString(),
                    ProductSku = d.ProductSku,
                    SumMoney = d.SumMoney.ToString(),
                    OrderID = OrderID
                })
                    .ToArray();
                return q;

        }
        [HttpPost]
        public ActionResult SetOrder([FromBody] SetOrderModel data)
        {
            decimal SumMoney = 0;
            foreach (var a in data.Positions)
            {
                SumMoney += Convert.ToDecimal(a.ProductPrice) * a.ProductSku;
            }
            var order = new order()
            {
                CreationDate = Convert.ToDateTime(data.CreationDate.ToShortDateString()),
                OrderNumber = data.OrderNumber,
                CustomName = data.CustomName,
                CustomSurname = data.CustomSurname,
                CustomPatronymic = data.CustomPatronymic,
                SumMoney = SumMoney
            };

            _orderAppContext.Orders.Add(order);
            _orderAppContext.SaveChanges();
            _orderAppContext.Order_positions.AddRange(data.Positions.Select(d => new order_position() { OrderID = order.ID, ProductName = d.ProductName, ProductPrice = Convert.ToDecimal(d.ProductPrice), ProductSku = d.ProductSku, SumMoney = Convert.ToDecimal(d.ProductPrice) * d.ProductSku }));

            _orderAppContext.SaveChanges();

            return new OkResult();
        }
        [HttpPost]
        public ActionResult SetOrderPosition([FromBody] SetOrder_PositionModel data)
        {
            var position = new order_position()
            {
                ProductName = data.ProductName,
                ProductPrice = Convert.ToDecimal(data.ProductPrice),
                ProductSku = data.ProductSku,
                SumMoney = Convert.ToDecimal(data.ProductPrice) * data.ProductSku,
                OrderID = data.OrderID
            };
            _orderAppContext.Order_positions.Add(position);
            _orderAppContext.SaveChanges();

            var order = _orderAppContext.Orders
                           .Include(d => d.Positions.Where(f => f.Removed == false))
                           .FirstOrDefault(d => d.ID == data.OrderID && d.Removed == false);
            order.SumMoney = order.Positions.Where(d => d.Removed == false).Select(g => g.SumMoney).Sum();
            _orderAppContext.SaveChanges();

            return new OkResult();
        }
        [HttpPost]
        public ActionResult EditOrder([FromBody] EditOrderModel data)
        {
            var order = _orderAppContext.Orders
                           .Include(d => d.Positions.Where(f=>f.Removed==false))
                           .FirstOrDefault(d => d.ID == data.OrderID && d.Removed == false);
            foreach (var u in order.Positions)
                u.Removed = true;

            order.Positions.AddRange(data.Positions.Select(d => new order_position() { OrderID = order.ID, ProductName = d.ProductName, ProductPrice = Convert.ToDecimal(d.ProductPrice), ProductSku = d.ProductSku, SumMoney = Convert.ToDecimal(d.ProductPrice) * d.ProductSku }));

            decimal SumMoney = 0;
            foreach (var a in data.Positions)
            {
                SumMoney += Convert.ToDecimal(a.ProductPrice) * a.ProductSku;
            }

            order.CreationDate = Convert.ToDateTime(data.CreationDate);
            order.OrderNumber = data.OrderNumber;
            order.CustomName = data.CustomName;
            order.CustomSurname = data.CustomSurname;
            order.CustomPatronymic = data.CustomPatronymic;
            order.SumMoney = SumMoney;

             _orderAppContext.SaveChanges();

            return new OkResult();
        }
        [HttpPost]
        public ActionResult EditOrderPositions([FromBody] EditOrder_PositionModel data)
        {
            var position = _orderAppContext.Order_positions
                           .FirstOrDefault(d => d.ID == data.PositionID && d.Removed == false);

            position.ProductName = data.ProductName;
            position.ProductPrice = Convert.ToDecimal(data.ProductPrice);
            position.ProductSku = data.ProductSku;
            position.SumMoney = Convert.ToDecimal(data.ProductPrice) * data.ProductSku;

            _orderAppContext.SaveChanges();
            var order = _orderAppContext.Orders
                           .Include(d => d.Positions.Where(f => f.Removed == false))
                           .FirstOrDefault(d => d.ID == position.OrderID && d.Removed == false);
            order.SumMoney = order.Positions.Where(d => d.Removed == false).Select(g => g.SumMoney).Sum();
            _orderAppContext.SaveChanges();

            return new OkResult();
        }
        [HttpPost]
        public ActionResult<EditOrderModel> editOrderInfo([FromBody] long OrderID)
        {
            var query = _orderAppContext.Orders.Include(d => d.Positions).FirstOrDefault(d => d.ID == OrderID);
            EditOrderModel order = new EditOrderModel();
            List<EditOrder_PositionModel> positions = new List<EditOrder_PositionModel>();
            order.OrderID = query.ID;
            order.CreationDate = query.CreationDate.ToString("yyyy-MM-dd");
            order.OrderNumber = query.OrderNumber;
            order.CustomName = query.CustomName;
            order.CustomSurname = query.CustomSurname;
            order.CustomPatronymic = query.CustomPatronymic;
            order.SumMoney = query.SumMoney.ToString();
            positions.AddRange(query.Positions.Where(b=>b.Removed==false).Select(d => new EditOrder_PositionModel() { PositionID = d.ID, ProductName = d.ProductName, ProductPrice = d.ProductPrice.ToString(), ProductSku = d.ProductSku, SumMoney = (d.ProductPrice * d.ProductSku).ToString() }));
            order.Positions = positions.ToArray();
            return order;
        }
        [HttpPost]
        public ActionResult<EditOrder_PositionModel> editOrderPositionInfo([FromBody] long PositionID)
        {
            var query = _orderAppContext.Order_positions.FirstOrDefault(d => d.ID == PositionID);
            EditOrder_PositionModel position = new EditOrder_PositionModel();

            position.ProductName = query.ProductName;
            position.ProductPrice = query.ProductPrice.ToString();
            position.ProductSku = query.ProductSku;
            position.SumMoney = query.SumMoney.ToString();
            return position;
        }
        [HttpPost]
        public ActionResult RemoveOrder([FromBody] long[] OrderIDs)
        {

            foreach (var u in OrderIDs)
            {
                _orderAppContext.Orders.FirstOrDefault(d => d.ID == u).Removed = true;
            }

            _orderAppContext.SaveChanges();

            return new OkResult();
        }
        [HttpPost]
        public ActionResult RemoveOrderPositions([FromBody] long[] PositionIDs)
        {
            foreach (var u in PositionIDs)
            {
                _orderAppContext.Order_positions.FirstOrDefault(d => d.ID == u).Removed = true;
            }
            _orderAppContext.SaveChanges();
            long orderID = _orderAppContext.Orders.Include(d => d.Positions).FirstOrDefault(b => b.Positions.Any(f => f.ID == PositionIDs[0])).ID;
            var order = _orderAppContext.Orders
                           .Include(d => d.Positions.Where(f => f.Removed == false))
                           .FirstOrDefault(d => d.ID == orderID && d.Removed == false);
            order.SumMoney = order.Positions.Where(d => d.Removed == false).Select(g => g.SumMoney).Sum();

            _orderAppContext.SaveChanges();

            return new OkResult();
        }

    }
}
