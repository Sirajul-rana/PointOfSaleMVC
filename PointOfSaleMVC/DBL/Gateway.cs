using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.DBL
{
    public class Gateway
    {
        public SqlConnection Connection { get; set; }
        public SqlCommand SqlCommand { get; set; }

        public Gateway(string query)
        {
            Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PointOfSale_DB"].ConnectionString);

            Connection.Open();
            SqlCommand = new SqlCommand(query, Connection);
        }
    }
}