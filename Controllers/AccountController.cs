using BankApi.Data.DTOs;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BankApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _AccountService;
        private readonly AccountTypeService _AccountTypeService;
        private readonly ClientService _ClientService;
        public AccountController(AccountService Accountservice, AccountTypeService accountTypeService, ClientService ClientService)
        {
            _AccountService = Accountservice;
            _AccountTypeService = accountTypeService;
            _ClientService = ClientService;
        }



        [HttpGet]
        public async Task<IEnumerable<AccountDTOout>> GetAccounts()
        {
            return await _AccountService.GetAccounts();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTOout>> GetById(int id)
        {
            var account = await _AccountService.GetDTObyID(id);

            if (account is null) return NotFound(new { Mensaje = $"La Cuenta con ID {id} no encontrado" });


            return account;

        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountDTOin account)
        {
            string validationResult = await validateAccount(account);

            if (!validationResult.Equals("Valido"))
                return BadRequest(new { message = validationResult });

            var newAccount = await _AccountService.Create(account);

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, newAccount);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AccountDTOin account)
        {

            var ExistingAccount = await _AccountService.GetById(id);
            if (ExistingAccount is not null)
            {
                string validationResult = await validateAccount(account);

                if (!validationResult.Equals("Valido"))
                    return BadRequest(new { message = validationResult });
                await _AccountService.Update(account);
                return Ok("Cliente actualizado correctamente.");

            }
            else
            {
                return BadRequest("Cliente no actualizado favor revisar informacion");
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var AccountToDelete = await _AccountService.GetById(id);
            if (AccountToDelete is not null)
            {
                var clientName = AccountToDelete.Id;
                await _AccountService.Delete(id);
                return Ok($"El usuario '{clientName}' ha sido eliminado correctamnte");
            }
            else
            {
                return BadRequest("El cliente no ha sido eliminado favor revisar la informacion");
            }


        }
        private async Task<string> validateAccount(AccountDTOin account)
        {
            string result = "Valido";
            var accountType = await _AccountTypeService.GetById(account.AccountType);
            if (accountType is null)
                result = $"El tipo de cuenta {account.AccountType} no existe.";

            var clietID = account.ClientId.GetValueOrDefault();
            var cleint = await _ClientService.GetById(clietID);


            if (cleint is null)
                result = $"El cliente {clietID} no existe.";

            return result;
        }
    }
}