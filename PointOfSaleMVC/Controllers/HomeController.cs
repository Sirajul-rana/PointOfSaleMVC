using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSaleMVC.Manager;

namespace PointOfSaleMVC.Controllers
{
    public class HomeController : Controller
    {
        SalesManager salesManager = new SalesManager();
        SetupManager setupManager = new SetupManager();
        public ActionResult Index()
        {
            ViewBag.TotalSales = salesManager.GetTotalSales();
            ViewBag.TotalItems = setupManager.GetTotalItems();
            ViewBag.TotalEmployees = setupManager.GetTotalEmployees();
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
    }
}