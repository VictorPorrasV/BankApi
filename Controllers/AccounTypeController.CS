using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{

    [Authorize]
    [Route("[controller]")]
        [ApiController]
        public class AccounTypeController : ControllerBase
        {


           private readonly AccountTypeService _service;

            public AccounTypeController(AccountTypeService service)
                {
                _service = service;
               }

            [HttpGet]
            public async Task<IEnumerable<AccountType>> GetAccountsTypes()
            {
                return await _service.GetAccountTypes();
            }
           
            [HttpGet("{id}")]
            public async Task<ActionResult<AccountType>> GetById(int id)
            {
                var accountType = await _service.GetById(id);

                if (accountType is null) return NotFound(new { Mensaje = $"El tipo de cuenta con ID {id} no ha sido encontrado" });


                return accountType;

            }


            [HttpPost]
            public async Task<IActionResult> Create(AccountType accountType)
            {
                // Verificar si el modelo es nulo
                if (accountType == null)
                {
                    return BadRequest("El tipo de cuenta no puede ser nulo.");
                }

                // Crear el nuevo tipo de cuenta
                var newAccountType = await _service.Create(accountType);

                // Verificar si la creación fue exitosa
                if (newAccountType == null)
                {
                    return StatusCode(500, "Hubo un problema al crear el tipo de cuenta.");
                }

                // Devolver la respuesta 201 Created con la URL para acceder al recurso creado
                return CreatedAtAction(nameof(GetById), new { id = newAccountType.Id }, newAccountType);
            }



        }
    }


