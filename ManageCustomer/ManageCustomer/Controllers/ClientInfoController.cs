﻿using ManageCustomer.Model;
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
    public class ClientInfoController : ControllerBase
    {
        //客户信息管理  查 增 改 删

        //客户信息管理  查
        [HttpGet]
        public ActionResult GetClientData()
        {
            //连接数据库，打开数据库连接
            string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();
            string token = this.Request.Headers["token"];
            if (token == "")
            {
                JObject result = new JObject();
                result.Add("code", 400);
                result.Add("msg", "fail");
                return BadRequest(result);
            }
            else
            {
                CheckPower cp = new CheckPower();
                string power = cp.check(token);
                if (power == 9.ToString())
                {
                    //sql语句参数化，防止sql注入问题
                    MySqlCommand cmd1 = new MySqlCommand("select * from clientinfo", conn);
                    //执行sql命令 结果填充DataSet
                    MySqlDataAdapter msda1 = new MySqlDataAdapter(cmd1);
                    DataSet ds1 = new DataSet();
                    msda1.Fill(ds1);

                    //取到所需的数据
                    DataTable dt1 = ds1.Tables[0];

                    //DataTable转化Json格式字符串
                    StringBuilder Json = new StringBuilder();
                    if (dt1.Rows.Count > 0)
                    {
                        Json.Append("[");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if ((bool)dt1.Rows[i]["isDeleted"])
                            {
                                continue;
                            }
                            else
                            {
                                Json.Append("{");
                                for (int j = 0; j < dt1.Columns.Count - 1; j++)
                                {
                                    Json.Append(dt1.Columns[j].ColumnName.ToString() + ":\"" + dt1.Rows[i][j].ToString() + "\"");
                                    if (j < dt1.Columns.Count - 2)
                                    {
                                        Json.Append(",");
                                    }
                                }
                                Json.Append("}");
                                if (i < dt1.Rows.Count - 1)
                                {
                                    Json.Append(",");
                                }
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
                        //创建Json对象
                        var result = new JObject();
                        //组合数据json
                        result.Add("code", 400);
                        result.Add("msg", "暂无数据！");
                        return BadRequest(result);
                    }
                }
                else
                {
                    CheckToken ct = new CheckToken();
                    string re = ct.check(token);

                    if (re != "outdate" && re != "error" && re != "null")
                    {
                        string[] strArray = re.Split(new char[5] { '{', '"', ':', ',', '}' });
                        string userName = strArray[5].ToString();
                        string userPassword = strArray[11].ToString();



                        //sql语句参数化，防止sql注入问题
                        MySqlCommand cmd = new MySqlCommand("select userIDCard from userinfo where userName=@userName and userPassword=@userPassword", conn);
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

                        //判断结果是否为空 
                        if (res.Rows.Count > 0)
                        {
                            //不为空时 取第一行数据  登录时账号一一对应  只有一行数据
                            DataRow dr = res.Rows[0];
                            string salesmanID = dr["userIDCard"].ToString();
                            //sql语句参数化，防止sql注入问题
                            MySqlCommand cmd1 = new MySqlCommand("select * from clientinfo where salesmanID=@salesmanID", conn);
                            MySqlParameter[] msp1 = new MySqlParameter[]
                            {
                            new MySqlParameter("@salesmanID",salesmanID),

                            };
                            cmd1.Parameters.AddRange(msp1);

                            //执行sql命令 结果填充DataSet
                            MySqlDataAdapter msda1 = new MySqlDataAdapter(cmd1);
                            DataSet ds1 = new DataSet();
                            msda1.Fill(ds1);

                            //取到所需的数据
                            DataTable dt1 = ds1.Tables[0];

                            //DataTable转化Json格式字符串
                            StringBuilder Json = new StringBuilder();

                            if (dt1.Rows.Count > 0)
                            {
                                Json.Append("[");
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    if ((bool)dt1.Rows[i]["isDeleted"])
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Json.Append("{");
                                        for (int j = 0; j < dt1.Columns.Count - 1; j++)
                                        {
                                            Json.Append(dt1.Columns[j].ColumnName.ToString() + ":\"" + dt1.Rows[i][j].ToString() + "\"");
                                            if (j < dt1.Columns.Count - 2)
                                            {
                                                Json.Append(",");
                                            }
                                        }
                                        Json.Append("}");
                                        if (i < dt1.Rows.Count - 1)
                                        {
                                            Json.Append(",");
                                        }
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
                                result.Add("msg", "暂无数据！2");
                                return BadRequest(result);
                            }
                        }
                        else
                        {
                            var result = new JObject();
                            //组合数据json
                            result.Add("code", 400);
                            result.Add("msg", "暂无数据！3");
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
        }


        //客户信息管理  增
        [HttpPost]
        public ActionResult InsertClientdata([FromBody] JObject value)
        {
            string salesmanID = value["salesmanID"].ToString();
            string clientName = value["clientName"].ToString();
            
            string clientAddress = value["clientAddress"].ToString();
            string clientCharge = value["clientCharge"].ToString();


            string clientProfession;
            if (value["clientProfession"] == null)
            {
                clientProfession = null;
            }
            else
            {
                clientProfession = value["clientProfession"].ToString();
            }


            string clientPicture;
            if (value["clientPicture"] == null)
            {
                clientPicture = null;
            }
            else
            {
                clientPicture = value["clientPicture"].ToString();
            }

            string clientPhone = value["clientPhone"].ToString();

            string clientFavorite;
            if (value["clientFavorite"] == null)
            {
                clientFavorite = null;
            }
            else
            {
                clientFavorite = value["clientFavorite"].ToString();
            }


            string clientHobby;
            if (value["clientHobby"] == null)
            {
                clientHobby = null;
            }
            else
            {
                clientHobby = value["clientHobby"].ToString();
            }

            string remark;
            if (value["remark"] == null)
            {
                remark = null;
            }
            else
            {
                remark = value["remark"].ToString();
            }

            JObject result = new JObject();

            //连接数据库，打开数据库连接
            string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();
            //sql语句参数化，防止sql注入问题
            MySqlCommand cmd = new MySqlCommand($"insert into clientinfo(salesmanID,clientName,clientAddress,clientCharge,clientProfession,clientPicture,clientPhone,clientFavorite,clientHobby,remark,isDeleted) " +
                $"values (@salesmanID,@clientName,@clientAddress,@clientCharge,@clientProfession,@clientPicture,@clientPhone,@clientFavorite,@clientHobby,@remark,'0')", conn);
            MySqlParameter[] msp = new MySqlParameter[]
            {
                new MySqlParameter("@salesmanID",salesmanID),
                new MySqlParameter("@clientName",clientName),
                new MySqlParameter("@clientAddress",clientAddress),
                new MySqlParameter("@clientCharge",clientCharge),
                new MySqlParameter("@clientProfession",clientProfession),
                new MySqlParameter("@clientPicture",clientPicture),
                new MySqlParameter("@clientPhone",clientPhone),
                new MySqlParameter("@clientFavorite",clientFavorite),
                new MySqlParameter("@clientHobby",clientHobby),
                new MySqlParameter("@remark",remark),

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

        [HttpPost]
        public ActionResult GetUpdateData([FromBody] JObject value)
        {
            string clientID = value["clientID"].ToString();
            JObject result = new JObject();
            //连接数据库，打开数据库连接
            string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();
            //sql语句参数化，防止sql注入问题
            MySqlCommand cmd = new MySqlCommand($"select * from clientinfo where clientID=@clientID", conn);
            MySqlParameter[] msp = new MySqlParameter[]
            {
                new MySqlParameter("@clientID",clientID),
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


        //客户信息管理  改
        [HttpPut]
        public ActionResult UpdateClientData([FromBody] JObject value)
        {
            string clientID = value["clientID"].ToString();
            string salesmanID = value["salesmanID"].ToString();
            string clientName = value["clientName"].ToString();
            string clientAddress = value["clientAddress"].ToString();
            string clientCharge = value["clientCharge"].ToString();
            string clientProfession;
            if (value["clientProfession"] == null)
            {
                clientProfession = null;
            }
            else
            {
                clientProfession = value["clientProfession"].ToString();
            }


            string clientPicture;
            if (value["clientPicture"] == null)
            {
                clientPicture = null;
            }
            else
            {
                clientPicture = value["clientPicture"].ToString();
            }

            string clientPhone = value["clientPhone"].ToString();

            string clientFavorite;
            if (value["clientFavorite"] == null)
            {
                clientFavorite = null;
            }
            else
            {
                clientFavorite = value["clientFavorite"].ToString();
            }


            string clientHobby;
            if (value["clientHobby"] == null)
            {
                clientHobby = null;
            }
            else
            {
                clientHobby = value["clientHobby"].ToString();
            }

            string remark;
            if (value["remark"] == null)
            {
                remark = null;
            }
            else
            {
                remark = value["remark"].ToString();
            }
            JObject result = new JObject();

            //连接数据库，打开数据库连接
            string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();
            //sql语句参数化，防止sql注入问题
            MySqlCommand cmd = new MySqlCommand($"update clientinfo set salesmanID=@salesmanID,clientName=@clientName,clientAddress=@clientAddress,clientCharge=@clientCharge,clientProfession=@clientProfession,clientPicture=@clientPicture,clientPhone=@clientPhone,clientFavorite=@clientFavorite,clientHobby=@clientHobby,remark=@remark where clientID=@clientID", conn);
            MySqlParameter[] msp = new MySqlParameter[]
            {
                new MySqlParameter("@salesmanID",salesmanID),
                new MySqlParameter("@clientName",clientName),
                new MySqlParameter("@clientAddress",clientAddress),
                new MySqlParameter("@clientCharge",clientCharge),
                new MySqlParameter("@clientProfession",clientProfession),
                new MySqlParameter("@clientPicture",clientPicture),
                new MySqlParameter("@clientPhone",clientPhone),
                new MySqlParameter("@clientFavorite",clientFavorite),
                new MySqlParameter("@clientHobby",clientHobby),
                new MySqlParameter("@remark",remark),
                new MySqlParameter("@clientID",clientID)
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


        //客户信息管理  删
        [HttpDelete]
        public ActionResult RemoveClientData([FromBody] JObject value)
        {
            string clientID = value["clientID"].ToString();

            string connstr = "server=127.0.0.1;port=3306;user=root;password=123456;database=crm;Charset=utf8;";
            using MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();

            MySqlCommand cmd = new MySqlCommand($"update clientinfo set isDeleted='1' where clientID=@clientID", conn);
            MySqlParameter[] msp = new MySqlParameter[]
            {
                new MySqlParameter("@clientID",clientID),
            };
            cmd.Parameters.AddRange(msp);

            JObject result = new JObject();

            if (cmd.ExecuteNonQuery() > 0)
            {
                result.Add("code", 200);
                result.Add("msg", "删除成功！");
                return Ok(result);
            }
            else
            {
                result.Add("code", 400);
                result.Add("msg", "删除失败！");
                return BadRequest(result);
            }
        }
    }

}
