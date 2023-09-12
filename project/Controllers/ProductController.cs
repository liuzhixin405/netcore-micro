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
using project.Elasticsearchs.Product.Search;
using project.Elasticsearchs.Product.Parameters;

namespace project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [FormatResponse]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        
        private readonly IMQPublisher _publisher;
        private readonly EsProductContext _esContext;
      
        public ProductsController(IProductService productService, IMQPublisher publisher,EsProductContext productEsContext)
        {
            _productService = productService;
            _publisher = publisher;
            _esContext = productEsContext;
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
        public async Task<ActionResult<Product>> Get(string id)
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
        public async Task<IActionResult> Put(string id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.Update(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.Delete(product);
            return NoContent();
        }
        [HttpGet("PublisherFromKafka")]
        public Task PublisherFromKafka() //测试通过
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

        [HttpGet("PublisherFromRabbitMq")]
        public Task PublisherFromMq()  //测试通过
        {
            _ = Task.Factory.StartNew(async () => {
                int i = 1200;
                while (i > 0)
                {
                    var str = "test";
                    _publisher.Publish<string>(str, "xx", "xx");
                    Console.WriteLine($"发送消息:{str}");
                    i--;
                    await Task.Delay(1000*5);
                }
            });
            return Task.CompletedTask;
        }
        private async Task<bool> ProductExists(string id)
        {
            return (await _productService.GetById(id)) != null;
        }

        [HttpPost("GetPageByEs")]
        public async Task<IActionResult> GetPageByEs(EsProductParameter parameter)
        {
            var res = await _esContext.Search(parameter);
            return Ok(res);
        }
    }


}
