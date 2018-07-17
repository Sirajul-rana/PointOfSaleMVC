using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PointOfSaleMVC.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        [HttpGet]
        public ActionResult SalesReport()
        {
            return View();
        }
    }
}