using Microsoft.AspNetCore.Mvc;
using NetCoreRedis.Web.StackExchangeAPI.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRedis.Web.StackExchangeAPI.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        private string listName = "meyve";

        public ListTypeController(RedisService redis)
        {
            _redis = redis;
            db = _redis.GetDb(1);
        }

        public IActionResult Index()
        {
            var nameList = new List<string>();
            if(db.KeyExists(listName))
            {
                db.ListRange(listName).ToList().ForEach(x=>
                {
                    nameList.Add(x.ToString());
                });
            }
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listName, name);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(string name)
        {
            await db.ListRemoveAsync(listName, name).ConfigureAwait(false); 
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listName);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeleteLastItem()
        {
            db.ListRightPop(listName);
            return RedirectToAction(nameof(Index));
        }
    }
}
