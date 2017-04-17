using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoRestaurant.Models;
using DemoRestaurant.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace DemoRestaurant.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")] 
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        // cần fix  bind check model state chứ ko check theo Id
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var model = db.Roles.AsEnumerable();
            return View(model);
        }
        [HttpGet]
        public ViewResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(IdentityRole Role)
        {
            // kiểm tra dữ liệu nếu đúng thì add còn ko thì báo lỗi
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(Role);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(Role);
        }
        [HttpGet]

        // Nguyên tắc bảo mật- ko làm delete theo link ip get 
        public ActionResult DeleteRole(string id) {
            var model = db.Roles.Find(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteRole")]
        public ActionResult DeleteConfirmed(string id)
        {
            IdentityRole model = null;
            try
            {
                model = db.Roles.Find(id);
                db.Roles.Remove(model);
                db.SaveChanges();
                return View("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult UserIndex()
        {
            
            var user = db.Users.AsEnumerable();
            
            return View(user);
        }


        // cần fix thêm UserIndexPostBack
        [HttpPost]
        [ActionName("UserIndex")]
        public ActionResult UserIndexPostBack ()
        {
            var user = db.Users.AsEnumerable();

            return View(user);
        }
        [HttpGet]
        public ActionResult EditUser (string Id)
        {
            
            var user = db.Users.Find(Id);
            return View(user);
        }

        [HttpPost]
        [ActionName("EditUser")]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserConfirmed(ApplicationUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("UserIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult EditUserRole (string Id)
        {
            // user role list lấy theo id, hiện từ id lấy sang list name tại view, cần fix lại
            // action này và các action liên quan vẫn cho view làm việc trực tiếp với db,
            //cần thêm viewmodel tương ứng và xử lý logic tại controller
            var user = db.Users.Find(Id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserRoleConfirm(string UserId, string RoleId)
        {
            // cần xem có nên sửa lại deleteuserroleconfirm cho hiệu quả không
           
            var user = db.Users.Find(UserId);
            user.Roles.Remove(user.Roles.Single(m => m.RoleId == RoleId));
            db.SaveChanges();
           
            return RedirectToAction("EditUserRole", new { Id = UserId });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserRoleConfirm(string Id, string RoleId)
        {
            ApplicationUser user = db.Users.Find(Id);
            if (RoleId != null && RoleId != "")
            {

                IdentityUserRole UserRole = new IdentityUserRole() { UserId = Id, RoleId = RoleId };
                try
                {
                    if (!user.Roles.Contains(UserRole))
                    {
                        user.Roles.Add(UserRole);
                        db.SaveChanges();
                    }


                    return RedirectToAction("EditUserRole", new { Id = Id });
                }
                catch (Exception ex)
                {
                    ViewBag.error = ex;
                    return RedirectToAction("EditUserRole", new { Id = Id });
                }
                
            }
            return RedirectToAction("EditUserRole", new { Id = Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditUserRole")]
        public ActionResult EditUserRoleConfirm()
        {
            return View();
        }


        [HttpGet]
        public ActionResult DeleteUser(string Id)
        {
            var user = db.Users.Find(Id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserConfimed(string Id)
        {
            ApplicationUser user = null;
            try
            {
                user = db.Users.Find(Id);
                db.Users.Remove(user);
                db.SaveChanges();
                return RedirectToAction("UserIndex");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }

    }
}