using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.DBL
{
    public class PurchaseGateway
    {
        public List<Employee> GetEmployees(int branchId)
        {
            List<Employee> employees = new List<Employee>();
            string query = "SELECT E.EmployeeId, E.EmployeeName FROM Employee E WHERE E.BranchId = @branchId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@branchId", branchId);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Employee employee = new Employee();
                employee.EmployeeId = (int) reader["EmployeeId"];
                employee.EmployeeName = reader["EmployeeName"].ToString();
                employees.Add(employee);
            }
            reader.Close();
            gateway.Connection.Close();
            return employees;
        }

        public int SaveStockIn(StockIn stockIn)
        {
            string query = "INSERT INTO StockIn (BranchId, PartyId, EmployeeId, Date, Quantity, ItemId, PurchaseTransactionId) " +
                           "VALUES(@branchId, @partyId, @employeeId, @puchaseDateTime, @quantity, @itemId, @purchaseTransactionId)";

            var purchaseTransactionId = GetPurchaseTransactionId(stockIn.PurchaseTransaction.Total,stockIn.PurchaseTransaction.Paid, stockIn.PurchaseTransaction.Return);
            Gateway gateway = new Gateway(query);
            int rowAffected = -1;
            foreach (Item anItem in stockIn.Items)
            {
                gateway.SqlCommand.Parameters.Clear();
                gateway.SqlCommand.Parameters.AddWithValue("@branchId", stockIn.BranchId);
                gateway.SqlCommand.Parameters.AddWithValue("@partyId",stockIn.PartyId);
                gateway.SqlCommand.Parameters.AddWithValue("@employeeId",stockIn.EmployeeId);
                gateway.SqlCommand.Parameters.AddWithValue("@puchaseDateTime", stockIn.PurchaseDateTime);
                gateway.SqlCommand.Parameters.AddWithValue("@quantity", anItem.Quantity);
                gateway.SqlCommand.Parameters.AddWithValue("@itemId",anItem.ItemId);
                gateway.SqlCommand.Parameters.AddWithValue("@purchaseTransactionId",purchaseTransactionId);

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

        public int GetPurchaseTransactionId(decimal total, decimal paid, decimal returnAmount)
        {
            string query = "INSERT INTO PurchaseTransaction (Total, PaidAmount, ReturnAmount) OUTPUT Inserted.PurchaseTransactionId " +
                           "VALUES (@total, @paidAmount, @returnAmount)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@total", total);
            gateway.SqlCommand.Parameters.AddWithValue("@paidAmount", paid);
            gateway.SqlCommand.Parameters.AddWithValue("@returnAmount", returnAmount);

            int id = (int)gateway.SqlCommand.ExecuteScalar();
            gateway.Connection.Close();
            return id;
        }
    }
}