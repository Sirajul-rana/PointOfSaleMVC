using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.DBL
{
    public class ItemGateway
    {
        public string SaveItem(Item item)
        {
            string query = "INSERT INTO Item (ItemName, ItemCode,ItemDescription, CostPrice, SalePrice, CategoryId)" +
                           " VALUES (@itemName, @itemCode,@itemDescription, @costPrice,@salePrice,@categoryId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@itemName", item.ItemName);
            gateway.SqlCommand.Parameters.AddWithValue("@itemCode", item.ItemCode);
            gateway.SqlCommand.Parameters.AddWithValue("@itemDescription", item.ItemDescription);
            gateway.SqlCommand.Parameters.AddWithValue("@costPrice", item.CostPrice);
            gateway.SqlCommand.Parameters.AddWithValue("@salePrice", item.SalePrice);
            gateway.SqlCommand.Parameters.AddWithValue("@categoryId", item.CategoryId);
            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            if (rowAffected >= 0)
            {
                gateway.Connection.Close();
                return "Item has been added successfully";
            }
            else
            {
                gateway.Connection.Close();
                return "Adding failed";
            }
        }

        public List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>();
            string query =
                "SELECT C.CategoryName, I.ItemName, I.ItemCode, I.ItemDescription, I.CostPrice, I.SalePrice FROM Item I " +
                "INNER JOIN Category C ON C.CategoryId = I.CategoryId";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                Item item = new Item();
                item.ItemName = reader["ItemName"].ToString();
                item.ItemCode = reader["ItemCode"].ToString();
                item.ItemDescription = reader["ItemDescription"].ToString();
                item.CostPrice = (decimal)reader["CostPrice"];
                item.SalePrice = (decimal)reader["SalePrice"];
                Category category = new Category();
                category.CategoryName = reader["CategoryName"].ToString();
                item.Category = category;
                items.Add(item);
            }
            reader.Close();
            gateway.Connection.Close();
            return items;
        }
    }
}