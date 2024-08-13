using BankApi.Services;
using BankApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly ClientService _service;

        public ClientController(ClientService service)
        {
            _service = service;
        }



        [HttpGet]
        public async Task<IEnumerable<Client>> GetClients()
        {
            return await  _service.GetClients(); 
        }


        [HttpGet ("{id}")]
        public async Task< ActionResult<Client>> GetById(int id)
        {
            var client = await _service.GetById(id);

            if(client is null) return NotFound(new { Mensaje = $"Cliente con ID {id} no encontrado" });


            return client;

        }

        [HttpPost]
        public async Task< IActionResult> Create(Client client) {
        
              var newClient= await _service.Create(client);
        
            return CreatedAtAction(nameof(GetById), new {id = newClient.Id}, newClient);  
            
        }


        [HttpPut("{id}")]
        public async Task <IActionResult> Update(int id)
        {
            
            var existingClient = await _service.GetById(id);
            if (existingClient is not null)
            {
                await   _service.Update(existingClient);
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
            var ClientToDelete =await _service.GetById(id);   
            if (ClientToDelete is not  null)
            {
                var clientName = ClientToDelete.Name;
                await   _service.Delete(id);
                return Ok($"El usuario '{clientName}' ha sido eliminado correctamnte");
            }
            else
            {
                return BadRequest("El cliente no ha sido eliminado favor revisar la informacion");
            }

           
        }

    }
}
