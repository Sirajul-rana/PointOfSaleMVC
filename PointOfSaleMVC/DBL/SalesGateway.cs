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
                item.Quantity = GetStockQuantity(item.ItemId);
                items.Add(item);
            }
            reader.Close();
            gateway.Connection.Close();
            return items;
        }

        public int SaveStockOut(StockOut stockOut)
        {
            string query = "INSERT INTO StockOut (BranchId, EmployeeId, SaleDate, StockQuantity, ItemId, SalesTransactionId) " +
                           "VALUES(@branchId, @employeeId, @saleDate, @stockQuantity, @itemId, @salesTransactionId)";

            var salesTransactionId = GetSalesTransactionId(stockOut.SalesTransaction);
            Gateway gateway = new Gateway(query);
            int rowAffected = -1;
            foreach (Item anItem in stockOut.Items)
            {
                gateway.SqlCommand.Parameters.Clear();
                gateway.SqlCommand.Parameters.AddWithValue("@branchId", stockOut.BranchId);
                gateway.SqlCommand.Parameters.AddWithValue("@employeeId", stockOut.EmployeeId);
                gateway.SqlCommand.Parameters.AddWithValue("@saleDate", stockOut.SaleDate);
                gateway.SqlCommand.Parameters.AddWithValue("@stockQuantity", anItem.Quantity);
                gateway.SqlCommand.Parameters.AddWithValue("@itemId", anItem.ItemId);
                gateway.SqlCommand.Parameters.AddWithValue("@salesTransactionId", salesTransactionId);

                rowAffected = gateway.SqlCommand.ExecuteNonQuery();
                if (rowAffected <= 0)
                {
                    gateway.Connection.Close();
                    return rowAffected;

                }
            }

            gateway.Connection.Close();
            return rowAffected;
        }

        private int GetSalesTransactionId(SalesTransaction salesTransaction)
        {
            string query = "INSERT INTO SalesTransaction (SubTotal, Vat, Discount, Total, PaidAmount, ReturnAmount)" +
                " OUTPUT Inserted.SalesTransactionId VALUES (@subTotal, @vat, @discount, @total, @paidAmount, @returnAmount)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@subTotal", salesTransaction.SubTotal);
            gateway.SqlCommand.Parameters.AddWithValue("@vat", salesTransaction.Vat);
            gateway.SqlCommand.Parameters.AddWithValue("@discount", salesTransaction.Discount);
            gateway.SqlCommand.Parameters.AddWithValue("@total", salesTransaction.Total);
            gateway.SqlCommand.Parameters.AddWithValue("@paidAmount", salesTransaction.PaidAmount);
            gateway.SqlCommand.Parameters.AddWithValue("@returnAmount", salesTransaction.ReturnAmount);

            int id = (int)gateway.SqlCommand.ExecuteScalar();
            gateway.Connection.Close();
            return id;
        }

        private int GetStockQuantity(int itemId)
        {
            int quantity = 0;
            string query = "select Sum(Quantity) StockQuantity From StockIn where ItemId = @itemId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@itemId", itemId);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                if (reader["StockQuantity"] == DBNull.Value)
                {
                    quantity = 0;
                }
                else
                {
                    quantity = (int)reader["StockQuantity"];
                }

            }
            reader.Close();
            gateway.Connection.Close();
            return quantity;
        }

        public int GetTotalSales()
        {
            string query = "SELECT SUM(So.StockQuantity) TotalSales FROM StockOut So";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            int totalSales = 0;
            if (reader.HasRows)
            {
                reader.Read();
                totalSales = (int) reader["TotalSales"];
            }

            reader.Close();
            gateway.Connection.Close();
            return totalSales;
        }
    }
}