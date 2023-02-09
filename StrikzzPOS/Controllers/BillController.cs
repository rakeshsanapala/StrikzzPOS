using StrikzzPOS.Common;
using StrikzzPOS.DTO;
using StrikzzPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    [Authorize]
    public class BillController : Controller
    {
        ApplicationDbContext _db;

        public BillController()
        {
            _db = new ApplicationDbContext();
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet]
        public ActionResult PrintBill(int id)
        {
            var ord = from o in _db.Orders
                      join c in _db.CustomerMsts on o.FK_CustomerId equals c.pk_Custid
                      join p in _db.PaymentTypes on o.FK_PaymentTypeId equals p.pk_PaymentTypeId
                      where o.OrderId == id
                      select new
                      {
                          OrderId = o.OrderId,
                          OrderNumber = o.OrderNumber,
                          OrderDate = o.OrderDate,
                          OrderStatus = o.OrderStatus,
                          CustomerName = c.Name,
                          FinalTotal = o.FinalTotal,
                          PaymentType = p.PaymentType,
                          OrderCancelDate = o.OrderCancelDate

                      };

            var dbOrder = ord.FirstOrDefault();
            var order = new OrderDTO
            {
                OrderId = dbOrder.OrderId,
                OrderNumber = dbOrder.OrderNumber,
                OrderDate = dbOrder.OrderDate,
                OrderStatus = dbOrder.OrderStatus,
                OrderCancelDate = dbOrder.OrderCancelDate,
                CustomerName = dbOrder.CustomerName,
                FinalTotal = dbOrder.FinalTotal
            };

            var orderDetails = from od in _db.OrderDetails.Where(a => a.FK_OrderId == order.OrderId)
                               join pr in _db.ProductMsts on od.FK_ProductId equals pr.pk_ProductId
                               select new
                               {
                                   OrderDetailId = od.OrderDetailId,
                                   FK_OrderId = od.FK_OrderId,
                                   ProductName = pr.ProductName,
                                   FK_ProductId = od.FK_ProductId,
                                   UnitPrice = od.UnitPrice,
                                   Quantity = od.Quantity,
                                   Discount = od.Discount,
                                   Total = od.Total
                               };
            var orderDetailDTOs = new List<OrderDetailDTO>();
            foreach (var o in orderDetails)
            {
                var orderDetailDTO = new OrderDetailDTO
                {
                    OrderDetailId = o.OrderDetailId,
                    FK_OrderId = o.FK_OrderId,
                    ProductName = o.ProductName,
                    FK_ProductId = o.FK_ProductId,
                    UnitPrice = o.UnitPrice,
                    Quantity = o.Quantity,
                    Discount = o.Discount,
                    Total = o.Total
                };
                orderDetailDTOs.Add(orderDetailDTO);
            }

            ViewBag.OrderPaymentType = dbOrder.PaymentType;
            order.OrderDetails = orderDetailDTOs;
            return View("PrintBill", order);
        }

        public ActionResult PrintBill_old(int orderId, int customerId)
        {
            var customer = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == customerId);

            ViewBag.CurrentDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            ViewBag.CustomerData = customer;
            // ViewBag.Setting = _settingService.List().FirstOrDefault();
            var order = _db.Orders.FirstOrDefault(a => a.OrderId == orderId);
            ViewBag.BillNo = orderId;
            ViewBag.Order = order;
            IEnumerable<OrderDetails> orderDetails = _db.OrderDetails.Where(a => a.FK_OrderId == orderId).ToList();

            return View(orderDetails);
        }
    }

}