using BankApi.Data.DTOs;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly LoginService _LoginService;
        private IConfiguration _config;
        public UserController(LoginService service, IConfiguration config)
        {
            _LoginService = service;
            _config = config;
        }



        [HttpPost("authenticate")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            var admin = await _LoginService.GetAdmin(user);
            if (admin is null)
            {
                return BadRequest(new { message = "Credenciales invalidas favor revisar." });
            }
            string JWTtoken = GenerateToken(admin);
            //generar token
            return Ok(new { token = JWTtoken });

        }

        private string GenerateToken(User user) {


            var claims = new[]
            {

                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("AdminType",user.AdminType)
            };


            //JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);


            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddHours(1),
                signingCredentials:creds
             );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
       
    }
}
