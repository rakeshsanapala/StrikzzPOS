using StrikzzPOS.DTO;
using StrikzzPOS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Product

        [HttpGet]
        public ActionResult AddUpdateProduct()
        {
            var pageLoadData = new ProductMstDTO
            {
                ProductTypeMstList = _db.ProductTypeMsts.ToList()
            };
            return View(pageLoadData);
        }


        public ActionResult ProductList()
        {
            IEnumerable<ProductListDTO> ProductList;

            var products = Session["PRODUCTS"];
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
                                  productType = b.Description,
                                  ProductName = a.ProductName,
                                  productQuantity = a.productQuantity,
                                  sellingUpToPrice = a.sellingUpToPrice
                              };
                Session["PRODUCTS"] = ProductList;
            }

            

            return View(ProductList);
        }

        public ActionResult Edit(int id)
        {
            var EditData = new ProductMstDTO
            {
                productMst = _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == id),
                ProductTypeMstList = _db.ProductTypeMsts.ToList()
               
            };
            Session.Remove("PRODUCTS");
            return View("AddUpdateProduct", EditData);
           
        }

        public ActionResult Delete(int id)
        {
         var dataForDelete= _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == id);
            _db.ProductMsts.Remove(dataForDelete);
            _db.SaveChanges();
            Session.Remove("PRODUCTS");
            return RedirectToAction("ProductList");

        }

        [HttpPost]
        public ActionResult AddUpdateProduct(ProductMstDTO product)
        {
            if(product.productMst.pk_ProductId==0)
            {
                product.productMst.username=  User.Identity.Name;
                _db.ProductMsts.Add(product.productMst);
                _db.SaveChanges();
            }
            else
            {
                var dataInDb= _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == product.productMst.pk_ProductId);

                dataInDb.fk_prodtypeid = product.productMst.fk_prodtypeid;
                dataInDb.ProductName = product.productMst.ProductName;
                dataInDb.productQuantity = product.productMst.productQuantity;
                dataInDb.sellingUpToPrice = product.productMst.sellingUpToPrice;
                _db.SaveChanges();


            }
            Session.Remove("PRODUCTS");
            return RedirectToAction("ProductList");
        }



    }
}