using BankApi.Data.DTOs;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly LoginService _LoginService;

        public UserController(LoginService service)
        {
            _LoginService = service;
        }



        [HttpPost("authenticate")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            var admin = await _LoginService.GetAdmin(user);
            if (admin is null)
            {
                return BadRequest(new { message = "Credenciales invalidas favor revisar." });
            }
            //generar token
            return Ok(new { token = "Some value" });

        }
       
    }
}
