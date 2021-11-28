using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace ManageCustomer.Model
{
    public class CheckToken
    {
        public string check(string token)
        {

            
            string key = "f47b558d-7654-458c-99f2-13b190ef0199";
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            
            //校验token
            var validateParameter = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "fan",
                ValidAudience = "audi~~!",
                IssuerSigningKey = securityKey,
            };
            //不校验，直接解析token
            //jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token1);
            try
            {
                //校验并解析token
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validateParameter, out SecurityToken validatedToken);//validatedToken:解密后的对象
                var jwtPayload = ((JwtSecurityToken)validatedToken).Payload.SerializeToJson(); //获取payload中的数据
                return jwtPayload;
                

            }
            catch (ArgumentNullException)
            {
                //表示过期
                //return "outdate";

                return "null";
            }
            catch (SecurityTokenExpiredException)
            {
                //表示过期
                //return "outdate";
                
                return "outdate";
            }
            catch (SecurityTokenException)
            {
                //表示token错误
                //return "error";
                
                return "error";
            }
        }

    }
}
