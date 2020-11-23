using StrikzzPOS.Common;
using StrikzzPOS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentTypeController : Controller
    {
        ApplicationDbContext _db;
        public PaymentTypeController()
        {
            _db = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult AddUpdatePaymentType()
        {

            return View("AddUpdatePaymentType");
        }

        [HttpPost]
        public ActionResult AddUpdatePaymentType(PaymentTypes PT)
        {
            if (PT.pk_PaymentTypeId == 0)
            {
              //PT.pk_PaymentTypeId = DateTime.Now.Year + DateTime.Now.Millisecond;
                _db.PaymentTypes.Add(PT);
                _db.SaveChanges();
            }
            else
            {
                var paymentTypeInDB = _db.PaymentTypes.FirstOrDefault(a => a.pk_PaymentTypeId == PT.pk_PaymentTypeId);
                paymentTypeInDB.PaymentType = PT.PaymentType;
                _db.SaveChanges();
            }

            Session.Remove("PAYMENTTYPES"); //Remove so that List call will get them all again.

            return RedirectToAction("PaymentTypeList");
        }

        public ActionResult PaymentTypeList()
        {
            var paymentTypes = Session["PAYMENTTYPES"];
            var paymentTypesList = new List<PaymentTypes>();
            if (paymentTypes != null)
                paymentTypesList = (List<PaymentTypes>)paymentTypes;
            else
            {
                paymentTypesList = _db.PaymentTypes.ToList();
                Session["PAYMENTTYPES"] = paymentTypesList;
            }
                
            return View(paymentTypesList);
        }

        public ActionResult Edit(int id)
        {
            var paymentType = _db.PaymentTypes.FirstOrDefault(a => a.pk_PaymentTypeId == id);
            return View("AddUpdatePaymentType", paymentType);
        }


    }
}