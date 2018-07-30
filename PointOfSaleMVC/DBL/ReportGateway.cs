using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.DBL
{
    public class ReportGateway
    {
        /*
         * Stock in report code starts here
         */
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
        /*
         * Stock in report code ends here
         */

        /*
         *  Purchase report code starts here 
         */
        public List<StockIn> GetPurchaseByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = new List<StockIn>();

            string query =
                "select CONVERT(VARCHAR(10),Si.Date,111) PurchaseDate,I.ItemName,Si.Quantity,I.CostPrice,B.BranchName,P.PartyName,(Si.Quantity * I.CostPrice) PurchaseTotal" +
                " from StockIn Si " +
                "Inner Join Branch B On B.BranchId = Si.BranchId " +
                "Inner Join Item I On I.ItemId = Si.ItemId " +
                "Inner Join Party P On P.PartyId = Si.PartyId " +
                "where Si.BranchId = @branchId";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@branchId", stockIn.BranchId);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                StockIn newStockIn = new StockIn();
                string date = reader["PurchaseDate"].ToString();
                newStockIn.PurchaseDateTime = DateTime.ParseExact(date,"yyyy/mm/dd",CultureInfo.InvariantCulture);
                newStockIn.PurchaseTotal = (decimal) reader["PurchaseTotal"];

                Branch branch = new Branch();
                branch.BranchName = reader["BranchName"].ToString();
                newStockIn.Branch = branch;

                Party party = new Party();
                party.PartyName = reader["PartyName"].ToString();
                newStockIn.Party = party;

                Item item = new Item();
                item.ItemName = reader["ItemName"].ToString();
                item.CostPrice = (decimal)reader["CostPrice"];
                item.Quantity = (int)reader["Quantity"];
                newStockIn.Item = item;

                stockIns.Add(newStockIn);
            }
            reader.Close();
            gateway.Connection.Close();
            return stockIns;
        }
        /*
         *  Purchase report code ends here 
         */
    }
}