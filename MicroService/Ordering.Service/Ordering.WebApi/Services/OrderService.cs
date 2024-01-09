﻿using Common.Redis.Extensions;
using DistributedId;
using Ordering.Domain.Dtos;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Repositories;
using Common.Util;
using Ordering.WebApi.Dtos;
using Namotion.Reflection;

namespace Ordering.WebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IWriteOrderRepository _writeRepo;
        private readonly IReadOrderRepository _readRepo;
        private readonly IDistributedId _distributedId;
        private readonly long _userId;
        private readonly IRedisCache _redisDb;
        public OrderService(IWriteOrderRepository writeOrderRepository,
            IReadOrderRepository readOrderRepository,
            IDistributedId distributedId,
            IHttpContextAccessor contextAccessor,
            IRedisCache redisDb)
        {
            _writeRepo = writeOrderRepository;
            _readRepo = readOrderRepository;
            _distributedId = distributedId;
            _userId = long.Parse(contextAccessor.HttpContext.Items["UserId"]?.ToString());
            _redisDb = redisDb;
        }
        public async Task<bool> Create(CreateOrderDto orderDto)
        {

            long userId = _userId;
            if (userId == 0)
            {
                return false;
            }
            //新增订单
            decimal totalAmount = orderDto.price * orderDto.quantity;

            await _writeRepo.AddAsync(Order.CreateNew(_distributedId.NewLongId(), userId, orderDto.pid, orderDto.quantity, totalAmount));
            var result = await _writeRepo.SaveChangeAsync();
            //扣减库存
            return result != -1;
        }

        public async Task<List<OrderDetailDto>> GetOrderDetails()
        {
            long userId = _userId;
            if (userId == 0)
            {
                throw new Exception("用户记录为空");
            }
            var orderList = await _readRepo.GetListAsync(x => x.UserId == userId);
            if (orderList == null)
            {
                return null;
            }
            var productsDic = await _redisDb.HGetAllAsync("products");
            List<CatalogCopy> catalogCopies = new();
            foreach ( var product in productsDic.Values )
            {
                catalogCopies.Add(System.Text.Json.JsonSerializer.Deserialize<CatalogCopy>(product));
            }
            
            var result = orderList.Select(x => new OrderDetailDto
            {
                Quantity = x.Quantity,
                TotalAmount = x.TotalAmount,
                ProductName = catalogCopies.Where(c=>c.Id.Equals(x.ProductId)).Select(c=>c.Name).FirstOrDefault(),
                CreateTime = DateTime.FromFileTimeUtc(x.CreateTime * 10000).ToString("yyyy-mm-dd HH:mm:ss"),
                OrderStatus = x.OrderStatus.GetDescription(),
        }).ToList();
            return result;
        }
    }
}
