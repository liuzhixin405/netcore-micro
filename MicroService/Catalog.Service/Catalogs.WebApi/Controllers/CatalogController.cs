using System.Linq;
using System.Linq.Expressions;
using System.Runtime;
using Catalogs.Domain.Catalogs;
using Catalogs.Infrastructure.Database;
using Catalogs.WebApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{

    private readonly ILogger<CatalogController> _logger;
    private readonly CatalogContext _catalogContext;

    public CatalogController(ILogger<CatalogController> logger, CatalogContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
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
    [ProducesResponseType(typeof(IEnumerable<Catalog>), StatusCodes.Status200OK)]
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

        var model = new PaginatedViewModel<Catalog>(pageIndex, pageSize, totalItems, itemsOnPage);
        return Ok(model);

    }

    [HttpGet]
    [Route("items/{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Catalog>> ItemByIdAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var item = await _catalogContext.Catalogs.SingleOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            return item;
        }

        return NotFound();
    }

    //PUT api/v1/[controller]/items
    [Route("items")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> UpdateProductAsync([FromBody] Catalog productToUpdate)
    {
        var catalogItem = await _catalogContext.Catalogs.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

        if (catalogItem == null)
        {
            return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
        }

        var oldPrice = catalogItem.Price;
        var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

        // Update current product
        catalogItem = productToUpdate;
        _catalogContext.Catalogs.Update(catalogItem);

        await _catalogContext.SaveChangesAsync();

        return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
    }

    //POST api/v1/[controller]/items
    [Route("items")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateProductAsync([FromBody] Catalog product)
    {
        var item = Catalog.CreateNew(product.Id, product.Name, product.Price, product.Stock, product.MaxStock, product.Description);


        _catalogContext.Catalogs.Add(item);

        await _catalogContext.SaveChangesAsync();

        return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
    }

    //DELETE api/v1/[controller]/id
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteProductAsync(int id)
    {
        var product = _catalogContext.Catalogs.SingleOrDefault(x => x.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        _catalogContext.Catalogs.Remove(product);

        await _catalogContext.SaveChangesAsync();

        return NoContent();
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
}
