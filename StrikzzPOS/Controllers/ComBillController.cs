using StrikzzPOS.Common;
using StrikzzPOS.DTO;
using StrikzzPOS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    [Authorize]
    public class ComBillController : Controller
    {
        ApplicationDbContext _db;

        public ComBillController()
        {
            _db = new ApplicationDbContext();
        }

        [ChildActionOnly]
        [OutputCache(CacheProfile = "Cache10Min")]
        public Com_Bill_DTO GetPageData()
        {
            Com_Bill_DTO comBill = new Com_Bill_DTO();

            IEnumerable<ProductListDTO> ProductList;
            var products = Session["PRODUCTMSTS"];
            //var paymentTypesList = new List<PaymentTypes>();
            if (products != null)
                ProductList = (IEnumerable<ProductListDTO>)products;
            else
            {
                ProductList = from a in _db.ProductMsts
                              join b in _db.ProductTypeMsts on a.fk_prodtypeid equals b.pk_prodtypeid
                              select new ProductListDTO
                              {
                                  pk_ProductId = a.pk_ProductId,
                                  ProductName = a.ProductName,
                                  productTypeId = a.fk_prodtypeid,
                              };
                Session["PRODUCTMSTS"] = ProductList;
            }
            var prodList = from productList in ProductList
                           select new ProductDDD_DTO
                           {
                               productId = productList.pk_ProductId,
                               ProductName = productList.ProductName,
                               ProductTypeId = productList.productTypeId
                           };
            comBill.ProductList = prodList.ToList();

            var paymentTypes = _db.PaymentTypes;
            comBill.PaymentTypes = paymentTypes.ToList();


            return comBill;
        }



        [HttpGet]
        public JsonResult GetCustomerByPhone(string phoneNumber)
        {
            CustomerMst customer = _db.CustomerMsts.Where(a => a.MobNo == phoneNumber).OrderByDescending(a=>a.LastVisit).FirstOrDefault();

            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getItemUnitPrice(int itemId)
        {
            ProductMst product = _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == itemId);
            double UnitePrice = product.oriPrice;

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveUpdateBill()
        {
            Com_Bill_DTO item = new Com_Bill_DTO();

            //CustomerMst cust;
            //if (User.IsInRole("Admin"))
            //{
            //   cust = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == id);
            //}
            //else
            //{
            //    cust = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == id && a.Username == User.Identity.Name);
            //}
            item = GetPageData();
            item.CustomerMst = new CustomerMst();
            return View(item);
        }
        [HttpPost]
        public ActionResult SaveUpdateBill(Com_Bill_DTO billData)
        {
            Com_Bill_DTO item = new Com_Bill_DTO();

            CustomerMst cust;
            if (User.IsInRole("Admin"))
            {
                cust = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == billData.CustomerMst.pk_Custid);
            }
            else
            {
                cust = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == billData.CustomerMst.pk_Custid && a.Username == User.Identity.Name);
            }

            billData.CustomerMst = cust;

            ProductListForBill productDetail = new ProductListForBill();
            productDetail.username = User.Identity.Name;
            productDetail.fk_custId = billData.CustomerMst.pk_Custid;
            productDetail.Fk_ProductId = billData.Fk_ProductId;
            productDetail.productQuantity = billData.prodQuantity;
            productDetail.price = billData.price;
            _db.ProductListForBills.Add(productDetail);
            _db.SaveChanges();
            item = GetPageData();
            billData.ProductList = item.ProductList;
            return View(billData);

        }

        [HttpPost]
        public JsonResult Order(Order objOrder)
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
            return Json(new { response = "Redirect", url = Url.Action("OrderList", "Order") });
        }

     }

}