using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homework4.Entities;

namespace Homework4.Contracts
{
    public interface IOrderService
    {
        public Task Save(Order[] orders);
        public IAsyncEnumerable<Order> Find(long storehouseId, OrderStatus status, DateTime createdFrom, DateTime createdTo);
    }
}