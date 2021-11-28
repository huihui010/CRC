using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ManageCustomer.Model
{
    public class CheckPower
    {
        public string check(string token)
        {
            string power;
            CheckToken ct = new CheckToken();
            string re = ct.check(token);
            if (re != "outdate" && re != "error" && re != "null")
            {
                string[] strArray = re.Split(new char[5] { '{', '"', ':', ',', '}' });
                string userName = strArray[5].ToString();
                string userPassword = strArray[11].ToString();
                //连接数据库，打开数据库连接
                string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
                using MySqlConnection conn = new MySqlConnection(connstr);
                conn.Open();
                //sql语句参数化，防止sql注入问题
                MySqlCommand cmd = new MySqlCommand("select rolePermission from userinfo where userName=@userName and userPassword=@userPassword", conn);
                MySqlParameter[] msp = new MySqlParameter[]
                {
                        new MySqlParameter("@userName",userName),
                        new MySqlParameter("@userPassword",userPassword )
                };
                cmd.Parameters.AddRange(msp);
                //执行sql命令 结果填充DataSet
                MySqlDataAdapter msda = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                msda.Fill(ds);

                //取到所需的数据
                DataTable res = ds.Tables[0];

                //判断结果是否为空 不为空进入判断账号和密码的情况 为空返回账号不存在
                if (res.Rows.Count > 0)
                {
                    //不为空时 取第一行数据  登录时账号一一对应  只有一行数据
                    DataRow dr = res.Rows[0];
                    power = dr["rolePermission"].ToString();
                    return power;
                }
                else
                {
                    power = null;
                    return power;
                }
            }
            else
            {
                power = null;
                return power;
            }
        }
    }
}
