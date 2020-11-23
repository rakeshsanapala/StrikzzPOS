using StrikzzPOS.Common;
using StrikzzPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    public class ReceiptsController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Customer
        public ActionResult ReceiptsList()
        {
            IEnumerable<CustomerMst> customerList;
            //if (User.IsInRole("Admin"))
            //{
                customerList = _db.CustomerMsts.ToList();
            //}
            //else
            //{
            //    customerList = _db.CustomerMsts.Where(a => a.Username == User.Identity.Name);
            //}

            return View(customerList);
        }

        //public ActionResult ReceiptsList()
        //{
        //    var prodList = _db.ProductTypeMsts.ToList();
        //    return View(prodList);
        //}
    }
}