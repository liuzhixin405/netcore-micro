using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Enums;
using Ordering.Domain.Events;

namespace Ordering.Domain.Orders
{
    [Table("MOrder")]
    public class Order : IEntity
    {
        public static Order CreateNew(long id, long userId, long pid, int quantity, decimal totalAmount)
        {
            var catalog = new Order(id,userId,pid,quantity,totalAmount)
            {
                Id = id,
                UserId = userId,
                ProductId = pid,
                Quantity = quantity,
                TotalAmount = totalAmount,
                OrderStatus =  OrderStatus.BeConfirm,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            return catalog;
        }

        private Order(long id, long userId, long pid, int quantity, decimal totalAmount)
        {
            Id = id;
                UserId = userId;
            ProductId = pid;
            Quantity = quantity;
            TotalAmount = totalAmount;
            CreateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            AddDomainEvent(new CreateOrderEvent(id,userId,pid,quantity));
        }

        protected Order() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // 指定非自增
        public override long Id { get; protected set; }

        public long UserId { get; private set; }
        public long ProductId { get; private set; }

        public int Quantity { get; private set; }
        public decimal TotalAmount { get; private set; }

        public OrderStatus OrderStatus { get; private set; }
        public long CreateTime { get; private set; }
        [Timestamp]
        public byte[] Version { get; set; }

        public void SetOrderStaus(OrderStatus status)
        {
            OrderStatus = status;
        }

    }
}
