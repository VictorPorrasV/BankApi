using BankApi.Data;
using BankApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Services
{
    
    
    public class ClientService
    {
        private readonly BankContext _context;

        public ClientService(BankContext context)
        {
            _context = context;

        }


        public IEnumerable<Client> GetClients()
        {
            return _context.Clients.ToList();

        }

      
        public Client? GetById(int id)
        {
            return _context.Clients.Find(id);

           
        }

      
        public Client Create(Client client)
        {

            _context.Clients.Add(client);
            _context.SaveChanges();

            return client;
        }


       
        public void Update( Client client)
        {
           

            var existingClient = GetById(client.Id);

            if (existingClient is not null)
            {
                existingClient.Name = client.Name;
                existingClient.PhoneNumber = client.PhoneNumber;
                existingClient.Email = client.Email;

                _context.SaveChanges();
            }
             
        }


       
        public void  Delete(int id)
        {
            var existingClient = GetById(id);
            if (existingClient is not null)
            {

                _context.Clients.Remove(existingClient);
                _context.SaveChanges();

            }

        }
    }
}
