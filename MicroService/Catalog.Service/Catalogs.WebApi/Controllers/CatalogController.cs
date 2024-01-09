using System.Linq;
using System.Linq.Expressions;
using System.Runtime;
using System.Threading.Channels;
using Catalogs.Domain.Catalogs;
using Catalogs.Domain.Dtos;
using Catalogs.Infrastructure.Database;
using Catalogs.WebApi.ViewModel;
using Common.Redis.Extensions;
using Common.DistributedId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Catalogs.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{

    private readonly ILogger<CatalogController> _logger;
    private readonly CatalogContext _catalogContext;
    private readonly IDistributedId _distributedId;
    private readonly Channel<string> _channel;
    public CatalogController(ILogger<CatalogController> logger, CatalogContext catalogContext,Channel<string> channel,IDistributedId distributedId)
    {
        _logger = logger;
        _catalogContext = catalogContext;
        _channel = channel;
        _distributedId = distributedId;
    }

    /// <summary>
    /// 获取指定的商品目录
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageIndex"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("items")]
    [ProducesResponseType(typeof(PaginatedViewModel<Catalog>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Catalogs([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
    {
        if (!string.IsNullOrEmpty(ids))
        {
            var items = await GetItemByIds(ids);
            if (!items.Any())
            {
                return BadRequest("ids value invalid. Must be comma-separated list of numbers");
            }

            return Ok(items);
        }

        var totalItems = await _catalogContext.Catalogs
            .LongCountAsync();

        var itemsOnPage = await _catalogContext.Catalogs
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
        var result = itemsOnPage.Select(x => new ProductDto(x.Id.ToString(), x.Name, x.Price.ToString(), x.Stock.ToString(), x.ImgPath));
        var model = new PaginatedViewModel<ProductDto>(pageIndex, pageSize, totalItems, result);
        return Ok(model);

    }

    [HttpGet]
    [Route("items/{id:long}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDto>> ItemByIdAsync(long id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var item = await _catalogContext.Catalogs.SingleOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            return new ProductDto(item.Id.ToString(),item.Name,item.Price.ToString(),item.Stock.ToString(),item.ImgPath,item.Description);
        }

        return NotFound();
    }

    //PUT api/v1/[controller]/items
    /* 测试参数
         {
      "id": 1743194721723641856,
      "name": "iphone16",
      "description": "热销产品1",
      "price": 1000,
      "stock": 50,
      "maxStock":100,
      "imgPath": "/Img/R.jpg"
    }
     */
    [Route("items")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateProductAsync([FromBody] Catalog productToUpdate)
    {
        var catalogItem = await _catalogContext.Catalogs.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

        if (catalogItem == null)
        {
            return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
        }

        if (!string.IsNullOrEmpty(productToUpdate.Name))
            catalogItem.SetName(productToUpdate.Name);
        if (!string.IsNullOrEmpty(productToUpdate.Description))
            catalogItem.SetDescription(productToUpdate.Description);
        if (productToUpdate.Price != 0M)
            catalogItem.SetPrice(productToUpdate.Price);
        if (!string.IsNullOrEmpty(productToUpdate.ImgPath))
            catalogItem.SetImgPath(productToUpdate.ImgPath);
        if (productToUpdate.Stock != 0)
        {
            var res = await catalogItem.AddStock(productToUpdate.Stock);
            if(res.Item1==false)
                return Ok(res);
        }
        _catalogContext.Entry(catalogItem).State = EntityState.Modified;
        _catalogContext.Catalogs.Update(catalogItem);

        var result = await _catalogContext.SaveChangesAsync();
        await DeleteCache();
        return Ok();
    }

    //POST api/v1/[controller]/items
    [Route("items")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateProductAsync([FromBody] Catalog product)
    {
        var item = Catalog.CreateNew(_distributedId.NewLongId(), product.Name, product.Price, product.Stock, product.MaxStock, product.Description);


        _catalogContext.Catalogs.Add(item);

        await _catalogContext.SaveChangesAsync();
        await DeleteCache();
        return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
    }

    //DELETE api/v1/[controller]/id
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProductAsync(long id)
    {
        var product = _catalogContext.Catalogs.SingleOrDefault(x => x.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        _catalogContext.Catalogs.Remove(product);
        await _catalogContext.SaveChangesAsync();
        await DeleteCache();
        return Ok();
    }


    private async Task<List<Catalog>> GetItemByIds(string ids)
    {
        var numIds = ids.Split(',').Select(id => (ok: long.TryParse(id, out long x), Value: x));
        if (!numIds.All(nid => nid.ok))
        {
            return new List<Catalog>();
        }

        var idsToSelect = numIds
         .Select(id => id.Value);

        var items = await _catalogContext.Catalogs.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

        return items;
    }

    private async Task DeleteCache()
    {
        //await _redisDb.HashDeleteAsync("products",id); //没必要了
        await _channel.Writer.WriteAsync("delete_catalog_fromredis");
    }
}
