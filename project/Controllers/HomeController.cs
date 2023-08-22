using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Services;
using project.Utility.BaseController;
using project.Utility.Helper;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace project.Controllers
{
    public class HomeController : AbsEfWorkController<Product>
    {
        private readonly IProductService _productService;
        
        public HomeController(IProductService eFCoreService)
        {
            _productService = eFCoreService;
        }

        protected override Task<IActionResult> CreateOrUpdate(Product product)
        {
          
            throw new NotImplementedException();
        }

        protected override Task<IActionResult> Delete<KeyType>(KeyType entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<IActionResult> PageResult()
        {
            throw new NotImplementedException();
        }

        protected async override Task<IActionResult> Search<KeyType>(KeyType searchId)
        {
            var result = await CacheHelper.GetAsync<string>("test");
           return Ok(result);
        }
    }
}

