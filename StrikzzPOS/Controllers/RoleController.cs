using Microsoft.AspNet.Identity.EntityFramework;
using StrikzzPOS.Models;
using System.Linq;
using System.Web.Mvc;

namespace StrikzzPOS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {

        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Role

        public ActionResult RoleList()
        {
            var roleList = _db.Roles.ToList();
            return View(roleList);
        }
        [HttpGet]
        public ActionResult CreateUpdateRole()
        {
            var role = new IdentityRole();
            return View(role);
        }
        //[HttpPost]
        //public ActionResult CreateRole(IdentityRole identity)
        //{
        //    _db.Roles.Add(identity);
        //    _db.SaveChanges();
        //    return RedirectToAction("RoleList");
        //}

        [HttpPost]
        public ActionResult CreateUpdateRole(IdentityRole role)
        {
            var roleInDB = _db.Roles.FirstOrDefault(a => a.Id == role.Id);
            if (roleInDB==null)
            {
                _db.Roles.Add(role);
                _db.SaveChanges();
            }
            else
            {
                roleInDB.Name = role.Name;
                _db.SaveChanges();
            }

            return RedirectToAction("RoleList");
        }

        public ActionResult Edit(string id)
        {
            var role = _db.Roles.FirstOrDefault(a => a.Id==id);
            return View("CreateUpdateRole", role);

        }

        public ActionResult Delete(string id)
        {
            var dataForDelete = _db.Roles.FirstOrDefault(a => a.Id == id);
            _db.Roles.Remove(dataForDelete);
            _db.SaveChanges();
            return RedirectToAction("RoleList");

        }
    }
}