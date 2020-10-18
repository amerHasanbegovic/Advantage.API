using Advantage.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advantage.API
{
    public class Helpers
    {
        private static readonly Random rand = new Random();

        private static readonly List<string> businessPrefix = new List<string>()
        {
            "ABC", "XYZ", "Sales", "Ready", "Enterprise", "Magic", "Budget", "Family", "Creative", "Earth", "Comfort", "Fashion"
        };
        private static readonly List<string> businessSuffix = new List<string>()
        {
            "Corporation", "Co", "Logistics", "Goods", "Hotels", "Automotive", "Sport", "Transit", "Books"
        };
        private static readonly List<string> states = new List<string>()
        {
            "USA", "Germany", "France", "Canada", "India", "China", "Japan", "Poland", "Spain", "Italy", "Switzerland", "Estonia",
            "UK", "South Korea", "Indonesia"
        };
        internal static string MakeCustomerName(List<string> names)
        {
            var maxNames = businessPrefix.Count() * businessSuffix.Count();
            if (names.Count() >= maxNames)
                throw new System.InvalidOperationException("Maximum number of unique names exceeded.");

            var prefix = GetRandom(businessPrefix);
            var suffix = GetRandom(businessSuffix);
            var businessName = prefix + " " + suffix;

            if (names.Contains(businessName))
                MakeCustomerName(names);

            return businessName;
        }
        internal static DateTime GetRandomOrderPlaced()
        {
            var end = DateTime.Now;
            var start = end.AddDays(-90);
            TimeSpan possibleSpan = end - start;
            TimeSpan newSpan = new TimeSpan(0, rand.Next(0, (int)possibleSpan.TotalMinutes), 0);
            return start + newSpan;
        }

        public static DateTime? GetRandomOrderCompleted(DateTime placed)
        {
            var now = DateTime.Now;
            var minLeadTime = TimeSpan.FromDays(7);
            var timePassed = now - placed;

            if (timePassed < minLeadTime)
                return null;

            return placed.AddHours(rand.Next(10, 90));
        }

        internal static decimal GetRandomTotalAmount() => rand.Next(100, 5000);

        private static string GetRandom(IList<string> items) => items[rand.Next(items.Count())];

        internal static string MakeCustomerEmail(string name) => $"contact@{name.ToLower()}.com";

        internal static string GetRandomState() => GetRandom(states);

        internal static Customer GetRandomCustomer(APIContext apiContext)
        {
            var randomId = rand.Next(1, apiContext.Customers.Count());
            return apiContext.Customers.First(c => c.Id == randomId);
        }
    }
}