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
        CategoryManager manager = new CategoryManager();
        ItemManager itemManager = new ItemManager();
        // GET: Setup
        /*
         * Category Setup code starts here
         */
        public ActionResult ItemCategorySetup()
        {
            ViewBag.Categories = manager.GetAllCategories();
            return View();
        }

        [HttpPost]
        public ActionResult SaveCategory(Category category)
        {
            string msg = manager.SaveCategory(category);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParentCategories()
        {
            SelectList categories = new SelectList(manager.GetParentCategories(), "CategoryId", "CategoryName", 0);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCategories()
        {
            List<Category> categories = manager.GetAllCategories();
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
            ViewBag.AllCategories = new SelectList(manager.GetChildCategories(), "CategoryId", "CategoryName", 0);
            ViewBag.Items = itemManager.GetAllItems();
            return View();
        }

        [HttpPost]
        public ActionResult ItemSetup(Item item)
        {
            string msg = itemManager.SaveItem(item);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllItems()
        {
            List<Item> items= itemManager.GetAllItems();
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
    }
}