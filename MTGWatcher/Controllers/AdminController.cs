using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MTGWatcher.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();



        }

        public ActionResult refreshDatabase()
        {
            return Json(new { message = "Update successful" }, JsonRequestBehavior.AllowGet);

        }
    }
}