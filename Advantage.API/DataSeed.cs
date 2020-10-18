using Advantage.API.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advantage.API
{
    public class DataSeed
    {
        private readonly APIContext _apiContext;
        public DataSeed(APIContext apiContext)
        {
            _apiContext = apiContext;
        }

        public void SeedData(int nCustomers, int nOrders)
        {
            //if there is no Customers
            if (!_apiContext.Customers.Any())
            {
                SeedCustomers(nCustomers);
                _apiContext.SaveChanges();
            }

            //if there is no Orders
            if (!_apiContext.Orders.Any())
            {
                SeedOrders(nOrders);
                _apiContext.SaveChanges();
            }

            //if there is no Servers
            if (!_apiContext.Servers.Any())
            {
                SeedServers();
                _apiContext.SaveChanges();
            }
        }

        private void SeedServers()
        {
            List<Server> servers = BuildServerList();
            foreach (var server in servers)
                _apiContext.Servers.Add(server);
        }

        private List<Server> BuildServerList()
        {
            return new List<Server>()
            {
                new Server {
                    Id=1,
                    Name="Dev-Web",
                    IsOnline = true
                },
                new Server {
                    Id=2,
                    Name="Dev-Mail",
                    IsOnline = false
                },
                new Server {
                    Id=3,
                    Name="Dev-Services",
                    IsOnline = true
                },
                new Server {
                    Id=4,
                    Name="QA-Web",
                    IsOnline = true
                },
                new Server {
                    Id=5,
                    Name="QA-Mail",
                    IsOnline = false
                },
                new Server {
                    Id=6,
                    Name="QA-Services",
                    IsOnline = true
                },
                new Server {
                    Id=7,
                    Name="Prod-Web",
                    IsOnline = false
                },
                new Server {
                    Id=8,
                    Name="Prod-Mail",
                    IsOnline = false
                },
                new Server {
                    Id=9,
                    Name="Prod-Services",
                    IsOnline = false
                }
            };
        }

        private void SeedOrders(int nOrders)
        {
            List<Order> orders = BuildOrdersList(nOrders);
            foreach (var order in orders)
                _apiContext.Orders.Add(order);

        }

        private List<Order> BuildOrdersList(int nOrders)
        {
            var orders = new List<Order>();
            for (int i = 1; i <= nOrders; i++)
            {
                var placed = Helpers.GetRandomOrderPlaced();
                var completed = Helpers.GetRandomOrderCompleted(placed);

                orders.Add(new Order
                {
                    Id = i,
                    Customer = Helpers.GetRandomCustomer(_apiContext),
                    Total = Helpers.GetRandomTotalAmount(),
                    Placed = placed,
                    Completed = completed
                });
            }
            return orders;
        }

        private void SeedCustomers(int nCustomers)
        {
            List<Customer> customers = BuildCustomerList(nCustomers);
            foreach (var customer in customers)
                _apiContext.Customers.Add(customer);
        }

        private List<Customer> BuildCustomerList(int nCustomers)
        {
            var customers = new List<Customer>();
            var names = new List<string>();

            for (int i = 1; i <= nCustomers; i++)
            {
                var name = Helpers.MakeCustomerName(names);
                names.Add(name);
                customers.Add(new Customer
                {
                    Id = i,
                    Name = name,
                    Email = Helpers.MakeCustomerEmail(name),
                    State = Helpers.GetRandomState()
                });
            }
            return customers;
        }
    }
}
