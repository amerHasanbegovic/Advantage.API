using Advantage.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Advantage.API.Controllers
{
    [Route("api/[controller]")]
    public class ServerController : Controller    
    {
        private readonly APIContext _apiContext;
        public ServerController(APIContext apiContext)
        {
            _apiContext = apiContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var data = _apiContext.Servers.OrderBy(s => s.Id).ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetServer(int id)
        {
            var data = _apiContext.Servers.Find(id);
            return Ok(data);
        }

        [HttpPut("{id}")]
        public IActionResult Message(int id, [FromBody] ServerMessage message)
        {
            var server = _apiContext.Servers.Find(id);
            if (server == null)
                return NotFound();
            if(message.Payload == "activate")
                server.IsOnline = true;
            if(message.Payload == "deactivate")
                server.IsOnline = false;

            _apiContext.SaveChanges();
            return new NoContentResult();
        }
    }
}
