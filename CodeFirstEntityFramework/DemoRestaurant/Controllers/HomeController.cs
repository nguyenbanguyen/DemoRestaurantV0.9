using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoRestaurant.DAL;
using DemoRestaurant.Models;

namespace DemoRestaurant.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        RestaurantDemoContext ResDb = new RestaurantDemoContext();
        public ActionResult Index()
        {
            ClientDetail Clientdetail = new ClientDetail();
            string ClientIpAndress = Request.UserHostAddress;
            string ClientTime = DateTime.Now.ToString();
            string DeviceName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;

            ViewBag.Testlogic = " IP : " + ClientIpAndress + "|| Time : " + ClientTime + "|| Name : " + DeviceName;
            
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [ChildActionOnly]
        public  ActionResult CategoriesMenu()
        {
            var categories = from s in ResDb.Category select s;
            return PartialView("CategoriesMenu", categories.ToList());
        }
    }
}