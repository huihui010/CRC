using ManageCustomer.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ManageCustomer.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //Get 数据获取
        //Post 数据插入
        //Put 数据更新
        //Delete 数据删除
        
        [HttpPost]
        public ActionResult CheckLog([FromBody] JObject value)
        {

            string userName = value["userName"].ToString();
            string userPassword = value["userPassword"].ToString();


            //连接数据库，打开数据库连接
            string connstr= "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();

            //sql语句参数化，防止sql注入问题
            MySqlCommand cmd = new MySqlCommand("select * from userinfo where userName=@userName", conn);
            MySqlParameter[] msp = new MySqlParameter[]
            {
                new MySqlParameter("@userName",userName),

            };
            cmd.Parameters.AddRange(msp);

            //执行sql命令 结果填充DataSet
            MySqlDataAdapter msda = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            msda.Fill(ds);

            //取到所需的数据
            DataTable res = ds.Tables[0];

            //创建Json对象
            var result = new JObject();
            
            //判断结果是否为空 不为空进入判断账号和密码的情况 为空返回账号不存在
            if (res.Rows.Count > 0)
            {
                //不为空时 取第一行数据  登录时账号一一对应  只有一行数据
                DataRow dr = res.Rows[0];
                var resUsers = dr["userName"].ToString();
                var resPass = dr["userPassword"].ToString();
                var rolePermission = dr["rolePermission"].ToString();
                //var IsDelete = (int)dr["isDeleted"];
                //判断软删除标记是否为1 为1表示删除 账号不存在
                if ((bool)dr["isDeleted"])
                {
                    result.Add("code",400);
                    result.Add("msg", "账号不存在！");
                    return BadRequest(result);
                }

                else if (resUsers == userName && resPass == userPassword)
                {
                    //生成token
                    GetToken gt = new GetToken();
                    string tokentext = gt.gettoken(userName, userPassword, rolePermission);

                    result.Add("code", 200);
                    result.Add("msg", "登陆成功！");
                    result.Add("token", tokentext);
                    
                    return Ok(result);
                }
                
                else
                {
                    result.Add("code", 400);
                    result.Add("msg", "密码错误！");
                    return BadRequest(result);
                }
            }
            else
            {
                result.Add("code", 400);
                result.Add("msg", "账号不存在！");
                return BadRequest(result);
            }
        }


        [HttpPost]
        public ActionResult ReturnData()
        {
            //string token = value["token"].ToString();
            string token = this.Request.Headers["token"];
            if (token=="")
            {
                JObject result = new JObject();
                result.Add("code", 400);
                result.Add("msg", "fail");
                return BadRequest(result);
            }
            else
            {
                CheckToken ct = new CheckToken();
                string re = ct.check(token);
                string[] strArray = re.Split(new char[5] { '{', '"', ':', ',', '}' });

                if (re != "outdate" && re != "error" && re!="null")
                {
                    string userName = strArray[5].ToString();
                    string userPassword = strArray[11].ToString();
                    string rolePermission = strArray[17].ToString();
                    JObject result = new JObject();

                    //连接数据库，打开数据库连接
                    string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
                    using MySqlConnection conn = new MySqlConnection(connstr);
                    conn.Open();
                    //sql语句参数化，防止sql注入问题
                    MySqlCommand cmd = new MySqlCommand("select * from userinfo where userName=@userName and userPassword=@userPassword", conn);
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
                        var userIDCard = dr["userIDCard"].ToString();
                        result.Add("code", 200);
                        result.Add("msg", "success");
                        result.Add("userIDCard", userIDCard);
                        //string[] role = new string[1] { rolePermission };
                        StringBuilder role = new StringBuilder();
                        role.Append("["+rolePermission+"]");
                        var jArray = JArray.Parse(role.ToString());
                        result.Add("rolePermission",jArray);
                        return Ok(result);
                    }
                    else
                    {
                        result.Add("code", 400);
                        result.Add("msg", "fail");
                        return BadRequest(result);
                    }
                }
                else
                {
                    JObject result = new JObject();
                    result.Add("code", 400);
                    result.Add("msg", re);
                    return BadRequest(result);
                }
            }
        }
        /*[HttpPost]
        public string test()
        {
            string token = this.Request.Headers["token"];
            CheckToken ct = new CheckToken();
            string re = ct.check(token);
            string[] strArray = re.Split(new char[5] { '{', '"', ':', ',', '}' });
            for(int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] == "1")
                {
                    return i.ToString();
                }
                else continue;
            }
            return "fail";
        }*/

    }
}
