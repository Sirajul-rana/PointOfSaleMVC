using System;
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
            return setupGateway.SaveCategory(category);
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
            return setupGateway.SaveItem(item);
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

        /*
         * Expense category setup code ends here
         */
    }
}