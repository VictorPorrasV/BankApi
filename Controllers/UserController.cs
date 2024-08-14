using BankApi.Data.DTOs;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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




        [HttpPost("Sign_up")]
        public async Task<IActionResult> Sign_up(UserDTOSignUp user)
        {
            // Verificar si las contraseñas coinciden
            if (!PasswordsMatch(user.Pwd, user.Confirm_Pwd))
            {
                return BadRequest(new { message = "Las contraseñas no coinciden." });
            }

            // Verificar si el usuario ya existe
            var existingUser = await _LoginService.Check_existence(user);
            if (existingUser != null)
            {
                return BadRequest(new { message = "El usuario con el correo indicado ya existe." });
            }

            // Hashear la contraseña del nuevo usuario
            var hashedPassword = HashPassword(user.Pwd);
            user.Pwd = hashedPassword;

            // Crear el nuevo usuario
            var newUser = await _LoginService.SignUpAsync(user); // Aquí debes pasar el objeto 'user', no 'existingUser'

            return Ok(newUser);
        }


        [HttpPost("LogIn")]
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
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTOOut>> GetById(int id)
        {
            var user = await _LoginService.GetById(id);

            if (user is null) return NotFound(new { Mensaje = $"El usuario con ID {id} no encontrado" });


            return user;

        }
        private string GenerateToken(User user)
        {
            var claims = new[]
            {

                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("AdminType",user.AdminType)
            };


            //JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
             );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        private string HashPassword(string password)
        {
            // Implementar un método seguro de hashing
            // Este es solo un ejemplo simple
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        private bool PasswordsMatch(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }
    }
}


