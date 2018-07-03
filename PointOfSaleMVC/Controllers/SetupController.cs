using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSaleMVC.Manager;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Controllers
{
    public class SetupController : Controller
    {
        SetupManager setupManager = new SetupManager();
        // GET: Setup
        /*
         * Category Setup code starts here
         */
        public ActionResult ItemCategorySetup()
        {
            ViewBag.Categories = setupManager.GetAllCategories();
            return View();
        }

        [HttpPost]
        public ActionResult SaveCategory(Category category)
        {
            string msg = setupManager.SaveCategory(category);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParentCategories()
        {
            SelectList categories = new SelectList(setupManager.GetParentCategories(), "CategoryId", "CategoryName", 0);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCategories()
        {
            List<Category> categories = setupManager.GetAllCategories();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        /*
         * Category Setup code ends here
         */

        /*
         * Item Setup code starts here
         */
        [HttpGet]
        public ActionResult ItemSetup()
        {
            ViewBag.AllCategories = new SelectList(setupManager.GetChildCategories(), "CategoryId", "CategoryName", 0);
            ViewBag.Items = setupManager.GetAllItems();
            return View();
        }

        [HttpPost]
        public ActionResult ItemSetup(Item item)
        {
            string msg = setupManager.SaveItem(item);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllItems()
        {
            List<Item> items= setupManager.GetAllItems();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        /*
         * Item setup code ends here
         */

        /*
         * Expense category setup code starts here
         */
        [HttpGet]
        public ActionResult ExpenseCategorySetup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveExpenseCategory(ExpenseCategory expenseCategory)
        {
            string msg = setupManager.SaveExpenseCategory(expenseCategory);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetParentExpenseCategories()
        {
            SelectList categories = new SelectList(setupManager.GetParentExpenseCategories(), "CategoryId", "CategoryName", 0);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        /*
         * Expense category setup code ends here
         */
        /*
         * Organization setup code starts here
         */
        [HttpGet]
        public ActionResult OrganizationSetup()
        {
            ViewBag.Organizations = setupManager.GetAllOrganizations();
            return View();
        }
        [HttpPost]
        public ActionResult SaveOrganization(Organization organization)
        {
            string msg = setupManager.SaveOrganization(organization);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrganizations()
        {
            List<Organization> organizations = setupManager.GetAllOrganizations();
            return Json(organizations, JsonRequestBehavior.AllowGet);
        }
        /*
         * Organization setup code ends here
         */

        /*
         * Outlet/ Branch setup code starts here
         */

        /*
         * Outlet/ Branch setup code ends here
         */
    }
}