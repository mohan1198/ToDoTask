using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TokenController : Controller
    {

        public static string GenerateToken(string Username)
        {
            Encrypt encrypt = new Encrypt();
            var claimsdata = new[] { new Claim(ClaimTypes.Name, Username) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopasdfghklljukuu"));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha384Signature);
            var token = new JwtSecurityToken(
                issuer: "https://localhost:44318",
                audience: "https://localhost:44318",
                expires: DateTime.Now.AddMinutes(60),
                claims: claimsdata,
                signingCredentials: signInCred
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
