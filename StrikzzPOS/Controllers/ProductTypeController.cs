
using StrikzzPOS.Common;
using StrikzzPOS.Models;
using System.Linq;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : Controller
    {
        ApplicationDbContext _db;
        public ProductTypeController()
        {
            _db = new ApplicationDbContext();
        }

       [HttpGet]
        public ActionResult Create()
        {
             
            return View("CreateUpdateForm");
        }

        [HttpPost]
        public ActionResult CreateUpdateForm(ProductTypeMst PT)
        {
            if(PT.pk_prodtypeid==0)
            {
                _db.ProductTypeMsts.Add(PT);
                _db.SaveChanges();
            }
            else
            {
                var productTypeInDB = _db.ProductTypeMsts.FirstOrDefault(a => a.pk_prodtypeid == PT.pk_prodtypeid);
                productTypeInDB.Description = PT.Description;
                _db.SaveChanges();
            }
            
            return RedirectToAction("ProductTypeList");
        }

        public ActionResult ProductTypeList()
        {
            var prodList = _db.ProductTypeMsts.ToList();
            return View(prodList);
        }

        public ActionResult Edit(int id)
        {
            var productType = _db.ProductTypeMsts.FirstOrDefault(a => a.pk_prodtypeid == id);
            return View("CreateUpdateForm",productType);
        }

        public ActionResult Delete(int id)
        {
            var dataForDelete = _db.ProductTypeMsts.FirstOrDefault(a => a.pk_prodtypeid == id);
            _db.ProductTypeMsts.Remove(dataForDelete);
            _db.SaveChanges();
            return RedirectToAction("ProductTypeList");

        }
    }
}