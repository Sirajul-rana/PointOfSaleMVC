using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.DBL
{
    public class CategoryGateway
    {
        public List<Category> GetChildCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "Select C.CategoryId,C.CategoryName from Category C where C.RootCategoryId <>''";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Category category = new Category();
                category.CategoryId = (int)reader["CategoryId"];
                category.CategoryName = reader["CategoryName"].ToString();
                categories.Add(category);
            }


            reader.Close();
            gateway.Connection.Close();
            return categories;
        }


        public List<Category> GetParentCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "Select C.CategoryId,C.CategoryName from Category C where C.RootCategoryId is null";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Category category = new Category();
                category.CategoryId = (int)reader["CategoryId"];
                category.CategoryName = reader["CategoryName"].ToString();
                categories.Add(category);
            }


            reader.Close();
            gateway.Connection.Close();
            return categories;
        }

        public string SaveCategory(Category category)
        {
            string query = "INSERT INTO Category (CategoryName, CategoryCode,CategoryDescription, CategoryType,RootCategoryId)" +
                           " VALUES (@categoryName, @categoryCode,@categoryDescription, @categoryType,@rootCategoryId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@categoryName",category.CategoryName);
            gateway.SqlCommand.Parameters.AddWithValue("@categoryCode",category.CategoryCode);
            gateway.SqlCommand.Parameters.AddWithValue("@categoryDescription",category.CategoryDescription);
            gateway.SqlCommand.Parameters.AddWithValue("@categoryType", category.CategoryType);
            SqlParameter rootCategoryParam = gateway.SqlCommand.Parameters.AddWithValue("@rootCategoryId",category.RootCategoryId);
            if (category.RootCategoryId == null)
            {
                rootCategoryParam.Value = DBNull.Value;
            }
            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            if (rowAffected >= 0)
            {
                gateway.Connection.Close();
                return "Category has been added successfully";
            }
            else
            {
                gateway.Connection.Close();
                return "Adding failed";
            }
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT  B.CategoryType, B.CategoryName , B.CategoryCode, B.CategoryDescription ,C.CategoryName Category from Category B " +
                           "Left join Category C on B.RootCategoryId = C.CategoryId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Category category = new Category();
                category.CategoryType = reader["CategoryType"].ToString();
                category.CategoryName = reader["CategoryName"].ToString();
                category.CategoryCode = reader["CategoryCode"].ToString();
                category.CategoryDescription = reader["CategoryDescription"].ToString();
                Category newCategory = new Category();
                newCategory.CategoryName = reader["Category"].ToString();
                category.RootCategory = newCategory;
                categories.Add(category);
            }


            reader.Close();
            gateway.Connection.Close();
            return categories;
        }
    }
}