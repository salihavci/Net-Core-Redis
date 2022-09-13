using Microsoft.AspNetCore.Mvc;
using NetCoreRedis.Web.StackExchangeAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreRedis.Web.StackExchangeAPI.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        private string listName = "sortedset";

        public SortedSetTypeController(RedisService redis)
        {
            _redis = redis;
            db = _redis.GetDb(3);
        }

        public IActionResult Index()
        {
            var nameList = new SortedSet<string>();
            if (db.KeyExists(listName))
            {
                //İki kodun işlevi aynıdır tek farkı ilk komutta score ve element alınabiliyorken diğerinde sadece element değeri geliyor.
                db.SortedSetScan(listName).ToList().ForEach(x =>
                {
                    nameList.Add(x.Element.ToString());
                });

                //db.SortedSetRangeByRank(listName,order:Order.Ascending).ToList().ForEach(x =>
                //{
                //    nameList.Add(x.ToString());
                //});
            }
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            db.KeyExpire(listName, DateTime.Now.AddMinutes(1));
            db.SortedSetAdd(listName, name, score);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(string name)
        {
            db.SortedSetRemove(listName, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
