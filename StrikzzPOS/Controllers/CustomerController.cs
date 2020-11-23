using StrikzzPOS.Common;
using StrikzzPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    public class CustomerController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Customer
        [Authorize]
        public ActionResult CustomerList()
        {
            IEnumerable<CustomerMst> customerList;
            customerList = _db.CustomerMsts.ToList();

            //IEnumerable<CustomerMst> customerList;
            //if (User.IsInRole("Admin"))
            //{
            //    customerList = _db.CustomerMsts.ToList();
            //}
            //else
            //{
            //    customerList = _db.CustomerMsts.Where(a => a.Username == User.Identity.Name);
            //}

            return View(customerList);
        }

        public ActionResult SaveUpdateCustomer()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var EditData = new CustomerMst();

            EditData = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == id);
            EditData.Username = User.Identity.Name;

            _db.SaveChanges();

            return View("SaveUpdateCustomer", EditData);

        }
        [HttpPost]
        public ActionResult SaveUpdateCustomer(CustomerMst customerMst)
        {

            if(customerMst.pk_Custid == 0 )
            {
                customerMst.Username = User.Identity.Name;
                customerMst.FirstVisit = DateTime.Now;
                customerMst.LastVisit = DateTime.Now;
                customerMst.TotalVisits = 1;
                customerMst.TotalSpent = 0;
                customerMst.PointBalance = 0;

                _db.CustomerMsts.Add(customerMst);
                _db.SaveChanges();
            }
            else
            {
                var customerInDB = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == customerMst.pk_Custid);
                customerInDB.Name = customerMst.Name;
                customerInDB.MobNo = customerMst.MobNo;
                customerInDB.Email = customerMst.Email;
                customerInDB.Username = User.Identity.Name;
                _db.SaveChanges();
            }
            

            return RedirectToAction("CustomerList");
        }
    }
}