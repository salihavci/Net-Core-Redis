using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NetCoreRedis.Web.InMemory.Models;
using System;

namespace NetCoreRedis.Web.InMemory.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //1. Yol
            //if (string.IsNullOrWhiteSpace(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("date", DateTime.Now.ToString());
            //}
            //2. Yol
            //if (!_memoryCache.TryGetValue("zaman",out string output))
            //{
            var options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.Low;
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep: {reason}");
            });
            _memoryCache.Set<string>("date", DateTime.Now.ToString(), options);

            Products p = new Products() { Id = 1, Name = "Kalem", Price = 100, Stock = 10 };
            _memoryCache.Set("product:1", p);
            //}
            return View();
        }
        public IActionResult Show()
        {
            //_memoryCache.Remove("date"); //Memory'den silme işlemi
            //_memoryCache.GetOrCreate<string>("date", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});
            _memoryCache.TryGetValue("date", out string output);
            _memoryCache.TryGetValue("callback", out string callback);
            TempData["date"] = output ?? "";
            TempData["callback"] = callback ?? "";
            ViewBag.product = _memoryCache.Get<Products>("product:1") ?? new Products();
            return View();
        }
    }
}
