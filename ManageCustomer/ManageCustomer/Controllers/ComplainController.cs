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

namespace ManageCustomer.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComplainController : ControllerBase
    {
        //投诉建议记录  查、增
        //投诉建议记录  查
        [HttpGet]
        public ActionResult GetComplainData()
        {
            //连接数据库，打开数据库连接
            string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();

            //sql语句参数化，防止sql注入问题
            MySqlCommand cmd = new MySqlCommand("select * from complains_records", conn);

            //执行sql命令 结果填充DataSet
            MySqlDataAdapter msda = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            msda.Fill(ds);

            //取到所需的数据
            DataTable dt = ds.Tables[0];

            //DataTable转化Json格式字符串
            StringBuilder Json = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                Json.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        Json.Append(dt.Columns[j].ColumnName.ToString() + ":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 2)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("]");

                if (Json.Length < 5)
                {
                    var result = new JObject();
                    //组合数据json
                    result.Add("code", 200);
                    result.Add("msg", "暂无数据！");
                    return Ok(result);
                }
                else
                {
                    //将json格式字符串转化为JSON类型数据
                    var jArray = JArray.Parse(Json.ToString());
                    //创建Json对象
                    var result = new JObject();
                    //组合数据json
                    result.Add("code", 200);
                    result.Add("msg", "success");
                    result.Add("data", jArray);
                    return Ok(result);
                }
            }
            else
            {
                var result = new JObject();
                //组合数据json
                result.Add("code", 400);
                result.Add("msg", "暂无数据");
                return BadRequest(result);
            }
        }


        //投诉建议记录  增
        [HttpPost]
        public ActionResult InsertComplainData([FromBody] JObject value)
        {
            string token = this.Request.Headers["token"];
            CheckPower cp = new CheckPower();
            string power = cp.check(token);
            if (power == 8.ToString() || power == 9.ToString())
            {
                string clientID = value["clientID"].ToString();
                string complainType = value["complainType"].ToString();
                string complainContent;
                if (value["complainContent"] == null)
                {
                    complainContent = null;
                }
                else
                {
                    complainContent = value["complainContent"].ToString();
                }
                string complainFollowUp = value["complainFollowUp"].ToString();

                JObject result = new JObject();

                //连接数据库，打开数据库连接
                string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
                using MySqlConnection conn = new MySqlConnection(connstr);
                conn.Open();
                //sql语句参数化，防止sql注入问题
                MySqlCommand cmd = new MySqlCommand($"insert into complains_records(clientID,complainType,complainContent,complainFollowUp,isDeleted) values (@clientID,@complainType,@complainContent,@complainFollowUp,'0')", conn);
                MySqlParameter[] msp = new MySqlParameter[]
                {
                new MySqlParameter("@clientID",clientID),
                new MySqlParameter("@complainType",complainType),
                new MySqlParameter("@complainContent",complainContent),
                new MySqlParameter("@complainFollowUp",complainFollowUp),

                };
                cmd.Parameters.AddRange(msp);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    result.Add("code", 200);
                    result.Add("msg", "添加成功！");
                    return Ok(result);
                }
                else
                {
                    result.Add("code", 400);
                    result.Add("msg", "添加失败！");
                    return BadRequest(result);
                }
            }
            else
            {
                JObject result = new JObject();
                result.Add("code", 400);
                result.Add("msg", "您没有权限！");
                return BadRequest(result);
            }

        }


        [HttpPost]
        public ActionResult GetUpdateData([FromBody] JObject value)
        {
            string complainID = value["complainID"].ToString();
            JObject result = new JObject();
            //连接数据库，打开数据库连接
            string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();
            //sql语句参数化，防止sql注入问题
            MySqlCommand cmd = new MySqlCommand($"select * from complains_records where complainID=@complainID", conn);
            MySqlParameter[] msp = new MySqlParameter[]
            {
                new MySqlParameter("@complainID",complainID),
            };
            cmd.Parameters.AddRange(msp);
            MySqlDataAdapter msda = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            msda.Fill(ds);

            //取到所需的数据
            DataTable dt = ds.Tables[0];
            //DataTable转化Json格式字符串
            StringBuilder Json = new StringBuilder();

            //判断结果是否为空 
            if (dt.Rows.Count > 0)
            {
                Json.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        Json.Append(dt.Columns[j].ColumnName.ToString() + ":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 2)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("]");
                //将json格式字符串转化为JSON类型数据
                var jArray = JArray.Parse(Json.ToString());
                //组合数据json
                result.Add("code", 200);
                result.Add("msg", "success");
                result.Add("data", jArray);
                return Ok(result);
            }
            else
            {
                result.Add("code", 400);
                result.Add("msg", "fail");
                return BadRequest(result);
            }
        }


        [HttpPut]
        public ActionResult UpdateComplainData([FromBody] JObject value)
        {
            string token = this.Request.Headers["token"];
            CheckPower cp = new CheckPower();
            string power = cp.check(token);
            if (power == 8.ToString()|| power == 9.ToString())
            {
                string complainID = value["complainID"].ToString();
                string clientID = value["clientID"].ToString();
                string complainType = value["complainType"].ToString();
                string complainContent;
                if (value["complainContent"] == null)
                {
                    complainContent = null;
                }
                else
                {
                    complainContent = value["complainContent"].ToString();
                }
                string complainFollowUp = value["complainFollowUp"].ToString();
                string handler;
                string handingResult;
                string handingExplain;
                if (value["handler"] == null)
                {
                    handler = null;
                }
                else
                {
                    handler = value["handler"].ToString();
                }

                if (value["handingResult"] == null)
                {
                    handingResult = null;
                }
                else
                {
                    handingResult = value["handingResult"].ToString();
                }

                if (value["handingExplain"] == null)
                {
                    handingExplain = null;
                }
                else
                {
                    handingExplain = value["handingExplain"].ToString();
                }
                JObject result = new JObject();

                //连接数据库，打开数据库连接
                string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
                using MySqlConnection conn = new MySqlConnection(connstr);
                conn.Open();
                //sql语句参数化，防止sql注入问题
                MySqlCommand cmd = new MySqlCommand($"update complains_records set clientID=@clientID,complainType=@complainType,complainContent=@complainContent,complainFollowUp=@complainFollowUp,handler=@handler,handingResult=@handingResult,handingExplain=@handingExplain where complainID=@complainID", conn);
                MySqlParameter[] msp = new MySqlParameter[]
                {
                new MySqlParameter("@clientID",clientID),
                new MySqlParameter("@complainType",complainType),
                new MySqlParameter("@complainContent",complainContent),
                new MySqlParameter("@complainFollowUp",complainFollowUp),
                new MySqlParameter("@handler",handler),
                new MySqlParameter("@handingResult",handingResult),
                new MySqlParameter("@handingExplain",handingExplain),
                new MySqlParameter("@complainID",complainID),

                };
                cmd.Parameters.AddRange(msp);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    result.Add("code", 200);
                    result.Add("msg", "更新成功！");
                    return Ok(result);
                }
                else
                {
                    result.Add("code", 400);
                    result.Add("msg", "更新失败！");
                    return BadRequest(result);
                }
            }
            else
            {
                JObject result = new JObject();
                result.Add("code", 400);
                result.Add("msg", "您没有权限！");
                return BadRequest(result);
            }
        }

    }
}
