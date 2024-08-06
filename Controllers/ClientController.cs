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
        public IEnumerable<Client> GetClients()
        {
            return _service.GetClients(); 
        }


        [HttpGet ("{id}")]
        public ActionResult<Client> GetById(int id)
        {
            var client = _service.GetById(id);

            if(client is null) return NotFound(new { Mensaje = $"Cliente con ID {id} no encontrado" });


            return client;

        }

        [HttpPost]
        public IActionResult Create(Client client) {
        
              var newClient= _service.Create(client);
        
            return CreatedAtAction(nameof(GetById), new {id = newClient.Id}, newClient);  
            
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id)
        {
            
            var existingClient = _service.GetById(id);
            if (existingClient is not null)
            {
                _service.Update(existingClient);
                return Ok("Cliente actualizado correctamente.");

            }
            else
            {
                return BadRequest("Cliente no actualizado favor revisar informacion");
            }

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ClientToDelete =_service.GetById(id);   
            if (ClientToDelete is not  null)
            {
                _service.Delete(id);
                return Ok("Usuario eliminado correctamnte");
            }
            else
            {
                return BadRequest("El cliente no ha sido eliminado favor revisar la informacion");
            }

           
        }

    }
}
