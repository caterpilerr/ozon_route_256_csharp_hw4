using System;
using System.Collections.Generic;

namespace Homework4.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public long ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveredAt { get; set; }
        public OrderStatus Status { get; set; }
        public List<Good> Goods { get; set; }
        public long StorehouseId { get; set; }

    }
}