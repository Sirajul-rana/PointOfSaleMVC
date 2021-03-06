﻿using System;
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
        [HttpPost]
        public ActionResult GetItemCode(Item item)
        {
            string itemCode = setupManager.GetItemCode(item);
            return Json(itemCode,JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        public ActionResult BranchSetup()
        {
            ViewBag.Branchs = setupManager.GetAllBranches();
            ViewBag.AllOrganization = new SelectList(setupManager.GetAllOrganization(), "OrganizationId", "OrganizationName", 0);
            return View();
        }
        [HttpPost]
        public ActionResult SaveBranch(Branch branch)
        {
            string msg = setupManager.SaveBranch(branch);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Getbranches()
        {
            List<Branch> branches = setupManager.GetAllBranches();
            return Json(branches, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetOrganizationCode(Organization organization)
        {
            string organizationCode = setupManager.GetOrganizationCode(organization);
            return Json(organizationCode, JsonRequestBehavior.AllowGet);
        }
        /*
         * Outlet/ Branch setup code ends here
         */

        /*
         * Organization setup code starts here
         */
        public ActionResult PartySetup()
        {
            ViewBag.AllParties = new SelectList(setupManager.GetAllPartyTypes(), "PartyTypeId", "Type", 0);
            ViewBag.Parties = setupManager.GetAllParties();
            return View();
        }
        [HttpPost]
        public ActionResult SaveParty(Party party)
        {
            string msg = setupManager.SaveParty(party);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParties()
        {
            List<Party> parties = setupManager.GetAllParties();
            return Json(parties, JsonRequestBehavior.AllowGet);
        }
        /*
         * Organization setup code ends here
         */

        /*
         * Employee setup code starts here
         */
        [HttpGet]
        public ActionResult EmployeeSetup()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            return View();
        }
        [HttpPost]
        public ActionResult SaveEmployee(Employee employee)
        {
            string msg = setupManager.SaveEmployee(employee);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewEmployees()
        {
            List<Employee> employees = setupManager.GetAllEmployees();
            return View(employees);
        }
        /*
         * Employee setup code ends here
         */
    }
}