using Microsoft.AspNetCore.Mvc;
using NetCoreRedis.Web.StackExchangeAPI.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreRedis.Web.StackExchangeAPI.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        private string listName = "dictionaries";

        public HashTypeController(RedisService redis)
        {
            _redis = redis;
            db = _redis.GetDb(4);
        }

        public IActionResult Index()
        {
            var dictionary = new Dictionary<string,string>();
            if (db.KeyExists(listName))
            {
                db.HashGetAll(listName).ToList().ForEach(x =>
                {
                    dictionary.Add(x.Name.ToString(),x.Value.ToString());
                });
            }
            return View(dictionary);
        }

        [HttpPost]
        public IActionResult Add(string key,string value)
        {
            db.HashSet(listName,key,value);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(string key)
        {
            db.HashDelete(listName,key);
            return RedirectToAction(nameof(Index));
        }
    }
}
