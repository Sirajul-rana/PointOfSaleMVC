using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointOfSaleMVC.DBL;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Manager
{
    public class SetupManager
    {
        /*
         * Category setup code starts here
         */
        SetupGateway setupGateway = new SetupGateway();
        public List<Category> GetParentCategories()
        {
            List<Category> categories = setupGateway.GetParentCategories();
            return categories;
        }

        public string SaveCategory(Category category)
        {
            if (setupGateway.SaveCategory(category) >= 0)
            {
                return "Category has been added successfully";
            }
            else
            {
                return "Adding failed";
            }
        }

        public List<Category> GetAllCategories()
        {
            return setupGateway.GetAllCategories();
        }

        public List<Category> GetChildCategories()
        {
            List<Category> categories = setupGateway.GetChildCategories();
            return categories;
        }
        /*
         * Category setup code ends here
         */

        /*
         * Item setup code starts here
         */

        public string SaveItem(Item item)
        {
            if (setupGateway.SaveItem(item) >= 0)
            {
                return "Item has been added successfully";
            }
            else
            {
                return "Adding failed";
            }
        }

        public List<Item> GetAllItems()
        {
            return setupGateway.GetAllItems();
        }
        /*
         * Item setup code ends here
         */

        /*
         * Expense category setup code starts here
         */
        public List<ExpenseCategory> GetParentExpenseCategories()
        {
            return setupGateway.GetParentExpenseCategories();
        }

        public string SaveExpenseCategory(ExpenseCategory expenseCategory)
        {
            if (setupGateway.SaveExpenseCategory(expenseCategory) >= 0)
            {
                return "Expense category has been added successfully";
            }
            else
            {
                return "Adding failed";
            }
        }
        /*
         * Expense category setup code ends here
         */


        /*
         * Organization setup code starts here
         */
        public string SaveOrganization(Organization organization)
        {
            if (setupGateway.SaveOrganization(organization) >= 0)
            {
                return "Organization has been added successfully";
            }
            else
            {
                return "Adding failed";
            }
        }

        public List<Organization> GetAllOrganizations()
        {
            return setupGateway.GetAllOrganizations();
        }

        public List<Organization> GetAllOrganization()
        {
            return setupGateway.GetOrganizationsForDropdown();
        }
        /*
         * Organization setup code ends here
         */


        /*
         * Outlet/ Branch setup code starts here
         */
        public string SaveBranch(Branch branch)
        {
            if (setupGateway.SaveBranch(branch) >= 0)
            {
                return "Organization has been added successfully";
            }
            else
            {
                return "Adding failed";
            }
        }
        public List<Branch> GetAllBranches()
        {
            return setupGateway.GetAllBranches();
        }
        /*
         * Outlet/ Branch setup code ends here
         */

        /*
         * Party setup code starts here
         */
        public List<PartyType> GetAllPartyTypes()
        {
            return setupGateway.GetAllPartyTypes();
        }
        public List<Party> GetAllParties()
        {
            return setupGateway.GetAllParties();
        }

        public string SaveParty(Party party)
        {
            if (setupGateway.SaveParty(party) >= 0)
            {
                return "Party has been added successfully";
            }
            else
            {
                return "Adding failed";
            }
        }
        /*
         * Party setup code ends here
         */

        /*
         * Organization setup code starts here
         */

        /*
         * Organization setup code ends here
         */



    }
}