using Advantage.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Advantage.API.Controllers
{
    [Route("api/[controller]")]
    public class OrderContoller : Controller
    {
        private readonly APIContext _apiContext;

        public OrderContoller(APIContext apiContext)
        {
            _apiContext = apiContext;
        }

        //GET api/order/pageNumber/pageSize
        [HttpGet("{pageNum:int}/{pageSize:int}")]
        public IActionResult Get(int pageNum, int pageSize)
        {
            var data = _apiContext.Orders.Include(order => order.Customer)
                .OrderByDescending(c => c.Placed);
            
            var p = new PaginatedResponse<Order>(data, pageNum, pageSize);

            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Page = p,
                TotalPages = totalPages
            };
            return Ok(response);
        }

        [HttpGet("ByState")]
        public IActionResult ByState()
        {
            var orders = _apiContext.Orders.Include(o => o.Customer).ToList();
            var grouped = orders.GroupBy(o => o.Customer.State)
                .ToList().Select(group => new
                {
                    State = group.Key,
                    Total = group.Sum(x => x.Total)
                }).OrderByDescending(res => res.Total).ToList();
            return Ok(grouped);
        }

        [HttpGet("ByCustomer/{n}")]
        public IActionResult ByCustomer(int n)
        {
            var orders = _apiContext.Orders.Include(o => o.Customer).ToList();
            var grouped = orders.GroupBy(o => o.Customer.Id)
                .ToList().Select(group => new
                {
                    Name = _apiContext.Customers.Find(group.Key).Name,
                    Total = group.Sum(x => x.Total)
                }).OrderByDescending(res => res.Total).Take(n).ToList();
            return Ok(grouped);
        }

        [HttpGet("GetOrder/{}", Name ="GetOrder")]
        public IActionResult GetOrder(int id)
        {
            var order = _apiContext.Orders.Include(o => o.Customer).First(o => o.Id == id);
            return Ok(order);
        }
    }
}
