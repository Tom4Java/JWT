using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Jwt.DTOModels;
using Jwt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Jwt.Controllers
{
    public class AuthenticationController : Controller
    {
        private tokenParameter _tokenParameter = new tokenParameter();
        public AuthenticationController()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _tokenParameter = config.GetSection("tokenParameter").Get<tokenParameter>();
        }


        [HttpPost, Route("requesttoken")]
        public ActionResult RequestToken([FromBody] LoginRequestDTO request)
        {
            //这儿待完善
            if (request.username == null && request.password == null)
            {
                return BadRequest("Invalid Request");
            }

            //生成Token和RefreshToken
            string token = GenUserToken(request.username, "testUser");
            var refreshToken = "123456";

            return Ok(new[] { token, refreshToken });
        }
        private string GenUserToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenParameter.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenParameter.Issuer, null, claims, expires: DateTime.UtcNow.AddMinutes(_tokenParameter.AccessExpiration), signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}