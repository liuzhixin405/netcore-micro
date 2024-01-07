using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Enums;

namespace Ordering.Domain.Orders
{
    [Table("MOrder")]
    public class Order
    {
        public static Order CreateNew(long id, long userId, long pid, int quantity, decimal totalAmount,OrderStatus orderStatus= OrderStatus.Pending)
        {
            //var catalog = new Catalog(id,name,price,stock,maxStock,desc,imgPath)
            var catalog = new Order()
            {
                Id = id,
                UserId = userId,
                ProductId = pid,
                Quantity = quantity,
                TotalAmount = totalAmount,
                OrderStatus = orderStatus,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
            return catalog;
        }
        protected Order() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // 指定非自增
        public long Id { get; private set; }

        public long UserId { get; private set; }
        public long ProductId { get; private set; }
  
        public int Quantity { get; private set; }
        public decimal TotalAmount { get; private set; }

        public OrderStatus OrderStatus { get; private set; }
        public long CreateTime { get; private set; }
        [Timestamp]
        public byte[] Version { get; set; }

    }
}
