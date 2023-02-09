using Microsoft.AspNet.Identity;
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
    public class ComBillController : Controller
    {
        ApplicationDbContext _db;

        public ComBillController()
        {
            _db = new ApplicationDbContext();
        }

        [ChildActionOnly]
        //  [OutputCache(CacheProfile = "Cache10Min")]
        public Com_Bill_DTO GetPageData()
        {
            Com_Bill_DTO comBill = new Com_Bill_DTO();

            IEnumerable<ProductListDTO> ProductList;
            var products = Session["PRODUCTMSTS"];
            //var paymentTypesList = new List<PaymentTypes>();
            if (products != null)
            {
                ProductList = (IEnumerable<ProductListDTO>)products;
                if (!ProductList.Any())
                {
                    ProductList = GetUserProducts();
                }
            }
            else
            {
                ProductList = GetUserProducts();
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

        private IEnumerable<ProductListDTO> GetUserProducts()
        {
            // TODO: Need to add these to Session
            IEnumerable<ProductListDTO> ProductList;
            var userid = User.Identity.GetUserId().ToString();
            ProductList = from a in _db.ProductMsts
                          join b in _db.ProductTypeMsts on a.fk_prodtypeid equals b.pk_prodtypeid
                          join c in _db.UserProducts on a.pk_ProductId equals c.fk_productId
                          where c.fk_UserId == userid
                          select new ProductListDTO
                          {
                              pk_ProductId = a.pk_ProductId,
                              ProductName = a.ProductName,
                              productTypeId = a.fk_prodtypeid
                          };
            return ProductList;
        }


        [HttpGet]
        public JsonResult GetCustomerByPhone(string phoneNumber)
            {
            CustomerMst customer = _db.CustomerMsts.Where(a => a.MobNo == phoneNumber).OrderByDescending(a => a.LastVisit).FirstOrDefault();

            if (customer != null)
            {
                ViewBag.CustomerId = customer.pk_Custid;
            }
                
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getItemUnitPrice(int itemId)
        {
            //TODO: Retried this on Application start
            var products = Session["PRODUCTS"];
            ProductMst product;
            if (products!=null)
            {
                var productList = (IEnumerable<ProductMst>)products;
                product =productList.FirstOrDefault(a => a.pk_ProductId == itemId);
            }
            else
                product = _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == itemId);
            double UnitePrice = product.oriPrice;

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveUpdateBill()
        {
            Com_Bill_DTO item = new Com_Bill_DTO();

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
        public JsonResult Order(Orders objOrder)
        {
            Orders order = new Orders();
            order.FK_CustomerId = objOrder.FK_CustomerId == 0 ? AddCustomer(objOrder) : objOrder.FK_CustomerId;
            order.FinalTotal = objOrder.FinalTotal;
            order.OrderDate = DateTime.Now;
            order.OrderNumber = GetOrderNumber();
            order.FK_PaymentTypeId = objOrder.FK_PaymentTypeId;
            order.OrderStatus = "A";
            _db.Orders.Add(order);
            _db.SaveChanges();
            int OrderId = order.OrderId;

            foreach (var objeOrderDetail in objOrder.OrderDetails)
            {
                OrderDetails orderDetail = new OrderDetails();
                orderDetail.FK_OrderId = OrderId;
                orderDetail.Discount = objeOrderDetail.Discount;
                orderDetail.FK_ProductId = objeOrderDetail.FK_ProductId;
                orderDetail.Total = objeOrderDetail.Total;
                orderDetail.UnitPrice = objeOrderDetail.UnitPrice;
                orderDetail.Quantity = objeOrderDetail.Quantity;
                _db.OrderDetails.Add(orderDetail);
                _db.SaveChanges();
            }

            UpdateCustomer(order.FK_CustomerId,order);
            //return Json(new { response = "Redirect", url = Url.Action("OrderList", "Order") });
            return Json(new { response = "Redirect", url = Url.Action("PrintBill", "Bill", new { id = order.OrderId }) });
            // return Json(new { response = "Redirect", url = Url.Action("PrintBill", "Bill", new { orderId = order.OrderId, customerId = order.FK_CustomerId }) });

        }
        protected string GetOrderNumber()
        {
            string numbers = "1234567890";
            string characters = numbers;
            int length = 6;
            string id = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (id.IndexOf(character) != -1);
                id += character;
            }
            return "G" + id;
        }

        private int AddCustomer(Orders order)
        {
            var customerMst = new CustomerMst();
            customerMst.Name = order.CustomerName;
            customerMst.MobNo = order.CustomerPhone;
            customerMst.Username = User.Identity.Name;
            customerMst.FirstVisit = DateTime.Now;
            customerMst.LastVisit = DateTime.Now;
            customerMst.TotalVisits = 0;
            customerMst.TotalSpent = 0;
            customerMst.PointBalance = 0;

            _db.CustomerMsts.Add(customerMst);
            _db.SaveChanges();

            return customerMst.pk_Custid;
        }

        private void UpdateCustomer(int customerId, Orders order)
        {
            CustomerMst customerMst = _db.CustomerMsts.Where(a => a.pk_Custid == customerId).FirstOrDefault();
            customerMst.LastVisit = DateTime.Now;
            customerMst.TotalVisits = customerMst.TotalVisits+ 1;
            customerMst.TotalSpent = customerMst.TotalSpent+ Convert.ToDecimal(order.FinalTotal);
            customerMst.PointBalance = customerMst.PointBalance + Convert.ToInt64(order.FinalTotal);

            _db.SaveChanges();
        }

    }

}