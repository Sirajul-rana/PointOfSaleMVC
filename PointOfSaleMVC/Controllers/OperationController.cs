using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSaleMVC.Manager;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Controllers
{
    public class OperationController : Controller
    {
        SetupManager setupManager = new SetupManager();
        SalesManager salesManager = new SalesManager();
        PurchaseManager purchaseManager = new PurchaseManager();
        // GET: Operation
        /*
         * Purchase operation code starts here
         */
        [HttpGet]
        public ActionResult PurchaseOperation()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            ViewBag.DropdownSuppliers = new SelectList(setupManager.GetParties(), "PartyId", "PartyName", 0);
            return View();

        }
        [HttpPost]
        public ActionResult GetEmployees(int branchId)
        {
            List<Employee> employees = purchaseManager.GetEmployees(branchId);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
        /*
         * Purchase operation code ends here
         */

        /*
         * Sales operation code starts here
         */
        [HttpGet]
        public ActionResult SalesOperation()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            return View();
        }

        [HttpPost]
        public JsonResult GetItems(string itemName)
        {
            var items = (from item in salesManager.GetItems(itemName)
                         select new
                         {
                             label = item.ItemName,
                             value = item.ItemName,
                             sale = item.SalePrice.ToString(),
                             cost = item.CostPrice
                         }).ToList(); ;
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        /*
         * Sales operation code ends here
         */



    }
}