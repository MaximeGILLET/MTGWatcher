using MTGWatcher.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTGWatcher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult RefreshMkmProducts()
        {
            var rawString64 = JsonConvert.DeserializeObject<ProductList>(RequestHelper.mkmRequest("https://www.mkmapi.eu/ws/v2.0/output.json/productlist")).productsfile;
            var gzip = Convert.FromBase64String(rawString64);
            System.IO.File.WriteAllBytes(HttpContext.Server.MapPath("~/MkmFiles/mkmProduct.gzip"), gzip);

            return View("Index");
        }             
    }
}