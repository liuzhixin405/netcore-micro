using project.Context;
using project.Models;
using project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Dtos;
using project.Utility.Helper;
using RepositoryComponent.Page;
using project.Attributes;
using MessageMiddleware;
using Redis.Extensions;

namespace project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [FormatResponse]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        
        private readonly IMQPublisher _publisher;
        private readonly IProductRedis _productRedis;
        private readonly IRedisCache _redisCache;
        public ProductsController(IProductService productService, IMQPublisher publisher, IProductRedis productRedis, IRedisCache redisCache)
        {
            _productService = productService;
            _publisher = publisher;
            _productRedis = productRedis;
            _redisCache = redisCache;
        }

        [HttpPost("PageList")]
        public async Task<PaginatedList<Product>> PageList(PaginatedOptions<PageProductDto> query)
        {
            return await _productService.PageList(query);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetList()
        {
            await CacheHelper.SetAsync("test", "test", TimeSpan.FromSeconds(60));
            var products = await _productService.GetList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductDto product)
        {
            var res = await _productService.Add(product);
            
            return Ok(res);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.Update(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.Delete(product);
            return NoContent();
        }
        [HttpGet("Publisher")]
        public Task Publisher()
        {
            _ = Task.Factory.StartNew(() => {
                int i = 1200;
                while (i > 0)
                {
                    var str = "test";
                    _publisher.Publish<string>(str, "xx", "xx");
                    Console.WriteLine($"发送消息:{str}");
                    i--;
                }
            });
            return Task.CompletedTask;
        }
        private async Task<bool> ProductExists(int id)
        {
            return (await _productService.GetById(id)) != null;
        }


    }


}
