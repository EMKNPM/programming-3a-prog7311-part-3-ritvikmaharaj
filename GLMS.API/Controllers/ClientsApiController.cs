using Microsoft.AspNetCore.Mvc;
using GLMS.API.Data;
using GLMS.API.Models;

namespace GLMS.API.Controllers
{

    [ApiController]

    [Route("api/clients")]

    public class ClientsApiController : ControllerBase
    {
        private readonly LogisticsDbContext _context;

        public ClientsApiController(LogisticsDbContext context) // dependency injection
        {
            _context = context;
        }

        // method
        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = _context.Clients.ToList();
            return Ok(clients);
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            return Ok(client);
        }
    }
}