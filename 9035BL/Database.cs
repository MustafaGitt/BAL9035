using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace _9035BL
{
    /*
     * Contains Database Class and Methods
     */
    public class Database
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IMS"].ConnectionString);
        public SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        // Generates a Datable from SQL (Parameter : SQL Query Required)
        public DataTable SqlSelect(String Sql)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                DataTable dt = new DataTable();
                cmd.Connection = con;
                cmd.CommandText = Sql;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

    }
}