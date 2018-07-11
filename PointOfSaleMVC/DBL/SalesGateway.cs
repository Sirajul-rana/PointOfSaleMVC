using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.DBL
{
    public class SalesGateway
    {
        public List<Item> GetItems(string itemName)
        {
            List<Item> items = new List<Item>();
            string query = "select * from Item where ItemName like '%'+@itemName+'%'";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@itemName", itemName);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            
            while (reader.Read())
            {
                Item item = new Item();
                item.ItemId = (int)reader["ItemId"];
                item.ItemName = reader["ItemName"].ToString();
                item.ItemCode = reader["ItemCode"].ToString();
                item.CostPrice = (decimal)reader["CostPrice"];
                item.SalePrice = (decimal)reader["SalePrice"];
                items.Add(item);
            }
            reader.Close();
            gateway.Connection.Close();
            return items;
        }
    }
}