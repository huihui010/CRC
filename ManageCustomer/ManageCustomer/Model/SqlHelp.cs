using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ManageCustomer.Model
{
    public class SqlHelp
    {
        public string ConnString { get; set; } = "server=127.0.0.1;port=3306;user=root;password=123456;database=Customer;Charset=utf8;";
        public DataTable ExecuteTable(string cmdText,params MySqlParameter[] mySqlParameters)
        {
            using MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.Parameters.AddRange(mySqlParameters);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds.Tables[0];
        }
        public int ExecuteNonQuery(string cmdText, params MySqlParameter[] mySqlParameters)
        {
            using MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.Parameters.AddRange(mySqlParameters);
            return cmd.ExecuteNonQuery();

        }
    }
}
