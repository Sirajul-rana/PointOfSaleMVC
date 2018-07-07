using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSaleMVC.Manager;

namespace PointOfSaleMVC.Controllers
{
    public class OperationController : Controller
    {
        SetupManager setupManager = new SetupManager();
        // GET: Operation
        [HttpGet]
        public ActionResult SalesOperation()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            return View();
        }


        


    }
}