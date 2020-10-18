using Advantage.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Advantage.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly APIContext _apiContext;
        public CustomerController(APIContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _apiContext.Customers.OrderBy(c => c.Id);
            return Ok(data);
        }

        //GET api/customer/id
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            var customer = _apiContext.Customers.Find(id);
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest();
            _apiContext.Customers.Add(customer);
            _apiContext.SaveChanges();
            return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
        }
    }
}
