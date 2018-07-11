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
    }
}