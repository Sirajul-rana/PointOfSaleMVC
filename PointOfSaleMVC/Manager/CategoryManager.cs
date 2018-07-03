using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointOfSaleMVC.DBL;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Manager
{
    public class CategoryManager
    {
        CategoryGateway gateway = new CategoryGateway();
        public List<Category> GetParentCategories()
        {
            List<Category> categories = gateway.GetParentCategories();
            return categories;
        }

        public string SaveCategory(Category category)
        {
            return gateway.SaveCategory(category);
        }

        public List<Category> GetAllCategories()
        {
            return gateway.GetAllCategories();
        }

        public List<Category> GetChildCategories()
        {
            List<Category> categories = gateway.GetChildCategories();
            return categories;
        }
    }
}