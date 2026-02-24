using System;
using System.Collections.Generic;
using lab20.Interfaces;
using lab20.Models;

namespace lab20.Services
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly Dictionary<int, Order> _orders = new();

        public void Save(Order order)
        {
            _orders[order.Id] = order;
            Console.WriteLine($"Order {order.Id} saved to in-memory database.");
        }

        public Order GetById(int id)
        {
            return _orders.ContainsKey(id) ? _orders[id] : null;
        }
    }
}