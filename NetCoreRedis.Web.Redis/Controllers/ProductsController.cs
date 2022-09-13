using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NetCoreRedis.Web.Redis.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace NetCoreRedis.Web.Redis.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly IDistributedCache _cache;

        public ProductsController(ILogger<ProductsController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IActionResult Index()
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };
            //_cache.SetString("name", "salihavci.com",options); 
            var product = new Product()
            {
                Id = 1,
                Name = "Kalem",
                Stock = 100,
                Price = 10
            }; 
            var jsonProduct = JsonConvert.SerializeObject(product);
            _cache.SetString("product:1", jsonProduct, options);

            var byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _cache.Set("product:2", byteProduct);

            return View();
        }

        [HttpGet]
        public IActionResult Show()
        {
            //string name = _cache.GetString("name");

            //ViewBag.name = name;

            var byteProduct = _cache.Get("product:2");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);
            var p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = p;
            return View();
        }

        [HttpGet]
        public IActionResult Remove()
        {
            _cache.Remove("name");
            return View();
        }

        [HttpGet]
        public IActionResult ImageCache()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", "cars.png");
            var imageByte = System.IO.File.ReadAllBytes(path);
            _cache.Set("resim", imageByte);
            return View();
        }

        [HttpGet]
        public IActionResult ImageUrl()
        {
            var resimByte = _cache.Get("resim");
            return File(resimByte,MediaTypeNames.Image.Jpeg);
        }
    }
}
