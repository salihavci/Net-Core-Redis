using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace NetCoreRedis.Web.StackExchangeAPI.Services
{
    public class RedisService
    {
        private readonly IConfiguration _configuration;
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redisHost = configuration["Redis:Host"].ToString();
            _redisPort = configuration["Redis:Port"].ToString();
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int dbIndex)
        {
            return _redis.GetDatabase(dbIndex);
        }

    }
}
