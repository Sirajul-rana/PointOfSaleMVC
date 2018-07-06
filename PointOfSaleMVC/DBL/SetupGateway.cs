using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.DBL
{
    public class SetupGateway
    {
        /*
         * Category setup code starts here
         */
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

        public int SaveCategory(Category category)
        {
            string query = "INSERT INTO Category (CategoryName, CategoryCode,CategoryDescription, CategoryType,RootCategoryId)" +
                           " VALUES (@categoryName, @categoryCode,@categoryDescription, @categoryType,@rootCategoryId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@categoryName", category.CategoryName);
            gateway.SqlCommand.Parameters.AddWithValue("@categoryCode", category.CategoryCode);
            gateway.SqlCommand.Parameters.AddWithValue("@categoryDescription", category.CategoryDescription);
            gateway.SqlCommand.Parameters.AddWithValue("@categoryType", category.CategoryType);
            SqlParameter rootCategoryParam = gateway.SqlCommand.Parameters.AddWithValue("@rootCategoryId", category.RootCategoryId);

            if (category.RootCategoryId == null)
            {
                rootCategoryParam.Value = DBNull.Value;
            }

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;
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
        /*
         * Category setup code ends here
         */

        /*
         * Item setup code starts here
         */
        public int SaveItem(Item item)
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

            gateway.Connection.Close();
            return rowAffected;
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
        /*
         * Item setup code ends here
         */

        /*
         * Expense category setup code starts here
         */
        public List<ExpenseCategory> GetParentExpenseCategories()
        {
            List<ExpenseCategory> expenseCategories = new List<ExpenseCategory>();
            string query = "Select Ex.ExpenseCategoryId,Ex.ExpenseName from ExpenseCategory Ex where Ex.RootExpenseCategoryId is null";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                ExpenseCategory expenseCategory = new ExpenseCategory();
                expenseCategory.ExpenseCategoryId = (int)reader["ExpenseCategoryId"];
                expenseCategory.ExpenseName = reader["ExpenseName"].ToString();
                expenseCategories.Add(expenseCategory);
            }

            reader.Close();
            gateway.Connection.Close();
            return expenseCategories;
        }

        public int SaveExpenseCategory(ExpenseCategory expenseCategory)
        {
            string query = "INSERT INTO Category (ExpenseName, ExpenseCode,ExpenseDescription, RootExpenseCategoryId)" +
                           " VALUES (@expenseName, @expenseCode, @expenseDescription, @rootExpenseCategoryId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@expenseName", expenseCategory.ExpenseName);
            gateway.SqlCommand.Parameters.AddWithValue("@expenseCode", expenseCategory.ExpenseCode);
            gateway.SqlCommand.Parameters.AddWithValue("@expenseDescription", expenseCategory.ExpenseDescription);
            SqlParameter rootCategoryParam = gateway.SqlCommand.Parameters.AddWithValue("@rootExpenseCategoryId", expenseCategory.RootExpenseCategoryId);
            if (expenseCategory.RootExpenseCategoryId == null)
            {
                rootCategoryParam.Value = DBNull.Value;
            }

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();

            gateway.Connection.Close();
            return rowAffected;
        }
        /*
         * Expense category setup code ends here
         */

        /*
         * Organization setup code starts here
         */
        public int SaveOrganization(Organization organization)
        {
            string query = "INSERT INTO Organization (OrganizationName, OrganizationCode,OrganizationContactNo, OrganizationAddress)" +
                           " VALUES (@organizationName, @organizationCode, @organizationContactNo, @organizationAddress)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@organizationName", organization.OrganizationName);
            gateway.SqlCommand.Parameters.AddWithValue("@organizationCode", organization.OrganizationCode);
            gateway.SqlCommand.Parameters.AddWithValue("@organizationContactNo", organization.OrganizationContactNo);
            gateway.SqlCommand.Parameters.AddWithValue("@organizationAddress", organization.OrganizationAddress);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();

            gateway.Connection.Close();
            return rowAffected;
        }

        public List<Organization> GetAllOrganizations()
        {
            List<Organization> organizations = new List<Organization>();
            string query = "SELECT O.OrganizationName, O.OrganizationCode, O.OrganizationContactNo, O.OrganizationAddress FROM Organization O";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Organization organization = new Organization();
                organization.OrganizationName = reader["OrganizationName"].ToString();
                organization.OrganizationCode = reader["OrganizationCode"].ToString();
                organization.OrganizationContactNo = reader["OrganizationContactNo"].ToString();
                organization.OrganizationAddress = reader["OrganizationAddress"].ToString();
                organizations.Add(organization);
            }

            reader.Close();
            gateway.Connection.Close();
            return organizations;
        }

        public List<Organization> GetOrganizationsForDropdown()
        {
            List<Organization> organizations = new List<Organization>();
            string query = "SELECT O.OrganizationId,O.OrganizationName FROM Organization O";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Organization organization = new Organization();
                organization.OrganizationId = (int)reader["OrganizationId"];
                organization.OrganizationName = reader["OrganizationName"].ToString();
                organizations.Add(organization);
            }

            reader.Close();
            gateway.Connection.Close();
            return organizations;
        }
        /*
         * Organization setup code ends here
         */

        /*
         * Outlet/ Branch setup code starts here
         */
        public List<Branch> GetAllBranches()
        {
            List<Branch> branches = new List<Branch>();
            string query = "SELECT O.OrganizationName, B.BranchName, B.BranchCode, B.BranchContactNo, B.BranchAddress FROM Branch B " +
                           "INNER JOIN Organization O ON O.OrganizationId = B.OrganizationId";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Branch branch = new Branch();
                branch.BranchName = reader["BranchName"].ToString();
                branch.BranchCode = reader["BranchCode"].ToString();
                branch.BranchContactNo = reader["BranchContactNo"].ToString();
                branch.BranchAddress = reader["BranchAddress"].ToString();
                Organization organization = new Organization();
                organization.OrganizationName = reader["OrganizationName"].ToString();
                branch.Organization = organization;
                branches.Add(branch);
            }

            reader.Close();
            gateway.Connection.Close();
            return branches;
        }

        public int SaveBranch(Branch branch)
        {
            string query = "INSERT INTO Branch (BranchName, BranchCode,BranchContactNo, BranchAddress, OrganizationId)" +
                           " VALUES (@branchName, @branchCode, @branchContactNo, @branchAddress, @organizationId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@branchName", branch.BranchName);
            gateway.SqlCommand.Parameters.AddWithValue("@branchCode", branch.BranchCode);
            gateway.SqlCommand.Parameters.AddWithValue("@branchContactNo", branch.BranchContactNo);
            gateway.SqlCommand.Parameters.AddWithValue("@branchAddress", branch.BranchAddress);
            gateway.SqlCommand.Parameters.AddWithValue("@organizationId", branch.OrganizationId);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();

            gateway.Connection.Close();
            return rowAffected;
        }
        public List<Branch> GetBranches()
        {
            List<Branch> branches = new List<Branch>();
            string query = "SELECT B.BranchId, B.BranchName FROM Branch B";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Branch branch = new Branch();
                branch.BranchId = (int)reader["BranchId"];
                branch.BranchName = reader["BranchName"].ToString();
                branches.Add(branch);
            }

            reader.Close();
            gateway.Connection.Close();
            return branches;
        }
        /*
         * Outlet/ Branch setup code ends here
         */

        /*
         * Party setup code starts here
         */
        public List<PartyType> GetAllPartyTypes()
        {
            List<PartyType> partyTypes = new List<PartyType>();
            string query = "SELECT Pt.PartyTypeId, Pt.Type FROM PartyType Pt";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PartyType partyType = new PartyType();
                partyType.PartyTypeId = (int)reader["PartyTypeId"];
                partyType.Type = reader["Type"].ToString();
                partyTypes.Add(partyType);
            }

            reader.Close();
            gateway.Connection.Close();
            return partyTypes;
        }
        public List<Party> GetAllParties()
        {
            List<Party> parties = new List<Party>();
            string query = "SELECT Pt.Type, P.PartyName, P.PartyCode, P.PartyContactNo, P.PartyEmail, P.PartyAddress FROM Party P " +
                           "INNER JOIN PartyType Pt ON P.PartyTypeId = Pt.PartyTypeId";

            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Party party = new Party();
                party.PartyName = reader["PartyName"].ToString();
                party.PartyCode = reader["PartyCode"].ToString();
                party.PartyContactNo = reader["PartyContactNo"].ToString();
                party.PartyEmail = reader["PartyEmail"].ToString();
                party.PartyAddress = reader["PartyAddress"].ToString();
                PartyType partyType = new PartyType();
                partyType.Type = reader["Type"].ToString();
                party.PartyType = partyType;
                parties.Add(party);
            }

            reader.Close();
            gateway.Connection.Close();
            return parties;
        }

        public int SaveParty(Party party)
        {
            string query = "INSERT INTO Party (PartyName, PartyCode, PartyContactNo, PartyEmail, PartyAddress, PartyTypeId)" +
                           " VALUES (@partyName, @partyCode, @partyContactNo, @partyEmail, @partyAddress, @partyTypeId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@partyName", party.PartyName);
            gateway.SqlCommand.Parameters.AddWithValue("@partyCode", party.PartyCode);
            gateway.SqlCommand.Parameters.AddWithValue("@partyContactNo", party.PartyContactNo);
            gateway.SqlCommand.Parameters.AddWithValue("@partyEmail", party.PartyEmail);
            gateway.SqlCommand.Parameters.AddWithValue("@partyAddress", party.PartyAddress);
            gateway.SqlCommand.Parameters.AddWithValue("@partyTypeId", party.PartyTypeId);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();

            gateway.Connection.Close();
            return rowAffected;
        }
        /*
         * Party setup code ends here
         */

        /*
         * Employee setup code starts here
         */
        public int SaveEmployee(Employee employee)
        {
            string query = "INSERT INTO Employee (EmployeeName, EmployeeFatherName, EmployeeMotherName, EmployeeCode," +
                           " EmployeeJoinDate, EmployeeContactNo, EmployeeEmergencyContactNo, EmployeeNId, EmployeeUsername," +
                           " EmployeePassword, EmployeeEmail, EmployeePresentAddress, EmployeePermanentAddress, BranchId)" +
                           " VALUES (@employeeName, @employeeFatherName, @employeeMotherName, @employeeCode, @employeeJoinDate," +
                           " @employeeContactNo, @employeeEmergencyContactNo, @employeeNId, @employeeUsername, @employeePassword," +
                           " @employeeEmail, @employeePresentAddress, @employeePermanentAddress, @branchId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@employeeName", employee.EmployeeName);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeFatherName", employee.EmployeeFatherName);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeMotherName", employee.EmployeeMotherName);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeCode", employee.EmployeeCode);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeJoinDate", employee.EmployeeJoinDate);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeContactNo", employee.EmployeeContactNo);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeEmergencyContactNo", employee.EmployeeEmergencyContactNo);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeNId", employee.EmployeeNId);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeUsername", employee.EmployeeUsername);
            gateway.SqlCommand.Parameters.AddWithValue("@employeePassword", employee.EmployeePassword);
            gateway.SqlCommand.Parameters.AddWithValue("@employeeEmail", employee.EmployeeEmail);
            gateway.SqlCommand.Parameters.AddWithValue("@employeePresentAddress", employee.EmployeePresentAddress);
            gateway.SqlCommand.Parameters.AddWithValue("@employeePermanentAddress", employee.EmployeePermanentAddress);
            gateway.SqlCommand.Parameters.AddWithValue("@branchId", employee.BranchId);
            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();

            gateway.Connection.Close();
            return rowAffected;
        }
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string query = "SELECT E.EmployeeId, E.EmployeeName, E.EmployeeFatherName, E.EmployeeMotherName, E.EmployeeCode," +
                           " E.EmployeeJoinDate, E.EmployeeContactNo, E.EmployeeEmergencyContactNo, E.EmployeeNId," +
                           " E.EmployeeEmail, E.EmployeePresentAddress, E.EmployeePermanentAddress, B.BranchName FROM Employee E" +
                           " INNER JOIN Branch B ON B.BranchId = E.BranchId";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Employee employee = new Employee();
                employee.EmployeeId = (int) reader["EmployeeId"];
                employee.EmployeeName = reader["EmployeeName"].ToString();
                employee.EmployeeFatherName = reader["EmployeeFatherName"].ToString();
                employee.EmployeeMotherName = reader["EmployeeMotherName"].ToString();
                employee.EmployeeCode = reader["EmployeeCode"].ToString();
                employee.EmployeeJoinDate = (DateTime)reader["EmployeeJoinDate"];
                employee.EmployeeContactNo = reader["EmployeeContactNo"].ToString();
                employee.EmployeeEmergencyContactNo = reader["EmployeeEmergencyContactNo"].ToString();
                employee.EmployeeNId = reader["EmployeeNId"].ToString();
                //employee.EmployeeUsername = reader["EmployeeUsername"].ToString();
                //employee.EmployeePassword = reader["EmployeePassword"].ToString();
                employee.EmployeeEmail = reader["EmployeeEmail"].ToString();
                employee.EmployeePresentAddress = reader["EmployeePresentAddress"].ToString();
                employee.EmployeePermanentAddress = reader["EmployeePermanentAddress"].ToString();
                Branch branch = new Branch();
                branch.BranchName = reader["BranchName"].ToString();
                employee.Branch = branch;
                employees.Add(employee);
            }

            reader.Close();
            gateway.Connection.Close();
            return employees;
        }
        /*
         * Employee setup code ends here
         */



    }
}