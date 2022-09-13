using Microsoft.AspNetCore.Mvc;
using NetCoreRedis.Web.StackExchangeAPI.Services;
using StackExchange.Redis;

namespace NetCoreRedis.Web.StackExchangeAPI.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        public StringTypeController(RedisService redis)
        {
            _redis = redis;
            db = _redis.GetDb(0);
        }

        public IActionResult Index()
        {
            db.StringSet("name", "salihavci.com * blog");
            db.StringSet("job", "dotnet developer");
            return View();
        }

        public IActionResult Show()
        {
            //var valueCharSubstring = db.StringGetRange("gamer", 0, 3);
            //var length = db.StringLength("name");
            //var image = db.StringSet("resim", new byte[] { });
            var value = db.StringGet("job");
            //var count = db.StringIncrement("counter", 1); //counter++
            if(value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
