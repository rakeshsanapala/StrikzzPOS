
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
    public class OrderController : Controller
    {
        ApplicationDbContext _db;
        public OrderController()
        {
            _db = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult OrderDetails(int id)
        {
            var ord= from o in _db.Order
                       join c in _db.CustomerMsts on o.FK_CustomerId equals c.pk_Custid
                       join p in _db.PaymentTypes on o.FK_PaymentTypeId equals p.pk_PaymentTypeId where o.OrderId== id
                       select new { OrderId = o.OrderId,
                           OrderNumber = o.OrderNumber,
                           OrderDate = o.OrderDate,
                           OrderStatus = o.OrderStatus,
                           CustomerName = c.Name,
                           FinalTotal = o.FinalTotal,
                           PaymentType= p.PaymentType,
                           OrderCancelDate=o.OrderCancelDate

                       };

            var dbOrder = ord.FirstOrDefault();
            var order = new OrderDTO
            {
                OrderId = dbOrder.OrderId,
                OrderNumber = dbOrder.OrderNumber,
                OrderDate = dbOrder.OrderDate,
                OrderStatus = dbOrder.OrderStatus,
                OrderCancelDate=dbOrder.OrderCancelDate,
                CustomerName = dbOrder.CustomerName,
                FinalTotal = dbOrder.FinalTotal
            };

            var orderDetails = from od in _db.OrderDetail.Where(a => a.FK_OrderId == order.OrderId)
                               join pr in _db.ProductMsts on od.FK_ProductId equals pr.pk_ProductId
                               select new
                               {
                                   OrderDetailId = od.OrderDetailId,
                                   FK_OrderId = od.FK_OrderId,
                                   ProductName=pr.ProductName,
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
                    ProductName=o.ProductName,
                    FK_ProductId = o.FK_ProductId,
                    UnitPrice = o.UnitPrice,
                    Quantity = o.Quantity,
                    Discount = o.Discount,
                    Total=o.Total
                };
                orderDetailDTOs.Add(orderDetailDTO);
            }
            order.OrderDetails = orderDetailDTOs;
            return View("OrderDetail",order);
        }

        [HttpGet]
        public ActionResult CreateOrder()
        {

            return View("CreateUpdateForm");
        }

        [HttpPost]
        public ActionResult CreateOrder(Order objOrder)
        {
            Order order = new Order();
            order.FK_CustomerId = objOrder.FK_CustomerId;
            order.FinalTotal = objOrder.FinalTotal;
            order.OrderDate = DateTime.Now;
            order.OrderNumber = String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now);
            order.FK_PaymentTypeId = objOrder.FK_PaymentTypeId;
            order.OrderStatus = "A";
            _db.Order.Add(order);
            _db.SaveChanges();
            int OrderId = order.OrderId;

            foreach (var objeOrderDetail in objOrder.OrderDetails)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.FK_OrderId = OrderId;
                orderDetail.Discount = objeOrderDetail.Discount;
                orderDetail.FK_ProductId = objeOrderDetail.FK_ProductId;
                orderDetail.Total = objeOrderDetail.Total;
                orderDetail.UnitPrice = objeOrderDetail.UnitPrice;
                orderDetail.Quantity = objeOrderDetail.Quantity;
                _db.OrderDetail.Add(orderDetail);
                _db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderList()
        {
            var orders = new List<OrderDTO>();
            var orderList = from o in _db.Order
                            join c in _db.CustomerMsts on o.FK_CustomerId equals c.pk_Custid
                            select new { OrderId=o.OrderId,OrderNumber = o.OrderNumber, OrderDate = o.OrderDate, OrderStatus = o.OrderStatus, CustomerName = c.Name, FinalTotal = o.FinalTotal };
            foreach (var o in orderList)
            {
                var order = new OrderDTO
                {
                    OrderId=o.OrderId,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    CustomerName = o.CustomerName,
                    FinalTotal = o.FinalTotal
                };
                orders.Add(order);
            }

            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CancelOrder(Order ord)
        {
            var order = _db.Order.FirstOrDefault(a => a.OrderId == ord.OrderId);
            order.OrderStatus = "C";
            order.OrderCancelDate = DateTime.Now;
            _db.SaveChanges();

           // return Json(new
           // {
           //     newUrl = Url.Action("OrderDetails", new { id = ord.OrderId }) //Payment as Action; Process as Controller
           // }
           //);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("OrderDetails", "Order", new { id = ord.OrderId });

            return Json(new { Url = redirectUrl, status = "OK" });
            //  return RedirectToAction("OrderDetails",new {id=ord.OrderId});
        }

        //public ActionResult Delete(int id)
        //{
        //    var dataForDelete = _db.ProductTypeMsts.FirstOrDefault(a => a.pk_prodtypeid == id);
        //    _db.ProductTypeMsts.Remove(dataForDelete);
        //    _db.SaveChanges();
        //    return RedirectToAction("ProductTypeList");

        //}
    }
}