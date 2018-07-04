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
            string query = "Select O.OrganizationName, B.BranchName, B.BranchCode, B.BranchContactNo, B.BranchAddress from Branch B " +
                           "Inner join Organization O on O.OrganizationId = B.OrganizationId";
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
        /*
         * Outlet/ Branch setup code ends here
         */

        /*
         * Organization setup code starts here
         */

        /*
         * Organization setup code ends here
         */

        /*
         * Organization setup code starts here
         */

        /*
         * Organization setup code ends here
         */

    }
}