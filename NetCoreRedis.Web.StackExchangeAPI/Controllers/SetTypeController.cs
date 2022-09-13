using Microsoft.AspNetCore.Mvc;
using NetCoreRedis.Web.StackExchangeAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreRedis.Web.StackExchangeAPI.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        private string listName = "hashset";

        public SetTypeController(RedisService redis)
        {
            _redis = redis;
            this.db = _redis.GetDb(2);
        }

        public IActionResult Index()
        {
            var nameList = new HashSet<string>();
            if (db.KeyExists(listName))
            {
                db.SetMembers(listName).ToList().ForEach(x=>
                {
                    nameList.Add(x.ToString());
                });
            }
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            //if(!db.KeyExists(listName))
            //{
            db.KeyExpire(listName, DateTime.Now.AddMinutes(5));
            //}
            db.SetAdd(listName, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteItem(string name)
        {
            db.SetRemove(listName, name);
            return RedirectToAction(nameof(Index));
        }

    }
}
