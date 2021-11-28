using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace ManageCustomer.Model
{
    public class GetToken
    {
        public string gettoken(string user, string password,string rolePermission)
        {

            DateTime utcNow = DateTime.UtcNow;
            string key = "f47b558d-7654-458c-99f2-13b190ef0199";
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            var claims = new List<Claim>() {
                new Claim("User",user),
                new Claim("Password",password),
                new Claim("rolePermission",rolePermission)
            };
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: "fan",
                audience: "audi~~!",
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddYears(1),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            //生成token
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            
            return token;
            
        }
    }
}
