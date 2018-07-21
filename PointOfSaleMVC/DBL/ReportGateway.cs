using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.DBL
{
    public class ReportGateway
    {
        public List<StockIn> GetStocksByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = new List<StockIn>();
            string query =
                "SELECT Sum(Si.Quantity) TotalQuantity, I.ItemName,I.SalePrice, C.CategoryName,Rc.CategoryName RootCategory" +
                " FROM StockIn Si " +
                "INNER JOIN Item I ON I.ItemId = Si.ItemId " +
                "INNER JOIN Category C ON I.CategoryId = C.CategoryId " +
                "LEFT JOIN Category Rc ON C.RootCategoryId = Rc.CategoryId " +
                "WHERE Si.BranchId = @branchId " +
                "GROUP BY I.ItemName,I.SalePrice, C.CategoryName,Rc.CategoryName";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@branchId", stockIn.BranchId);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                StockIn newStockIn = new StockIn();
                Item item = new Item();
                item.ItemName = reader["ItemName"].ToString();
                item.SalePrice = (decimal)reader["SalePrice"];
                item.Quantity = (int)reader["TotalQuantity"];
                Category category = new Category();
                category.CategoryName = reader["CategoryName"].ToString();
                Category rootCatetpry = new Category();
                rootCatetpry.CategoryName = reader["RootCategory"].ToString();
                category.RootCategory = rootCatetpry;
                item.Category = category;
                newStockIn.Item = item;
                stockIns.Add(newStockIn);
            }
            reader.Close();
            gateway.Connection.Close();
            return stockIns;
        }
    }
}