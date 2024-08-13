using BankApi.Data;
using BankApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BankApi.Services
{
    
    
    public class ClientService
    {
        private readonly BankContext _context;

        public ClientService(BankContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

      
        public async Task <Client?> GetById(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

      
        public async Task<Client> Create(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }


       
        public async Task  Update( Client client)
        {
           

            var existingClient = await GetById(client.Id);
            
            if (existingClient is not null)
            {
                existingClient.Name = client.Name;
                existingClient.PhoneNumber = client.PhoneNumber;
                existingClient.Email = client.Email;

                await   _context.SaveChangesAsync();
            }
             
        }


       
        public async Task  Delete(int id)
        {
            var existingClient = await GetById(id);

            if (existingClient is not null)
            {
                _context.Clients.Remove(existingClient);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
