using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LabDocker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private const string ConnectionString = "mongodb://root:rootpassword@mongo:27017/weather?serverSelectionTimeoutMS=5000&connectTimeoutMS=10000&authSource=admin&authMechanism=SCRAM-SHA-256";
        private readonly IMongoCollection<WeatherForecast> _collection;

        public WeatherForecastController()
        {
            _collection = new MongoClient(ConnectionString)
                .GetDatabase("weather")
                .GetCollection<WeatherForecast>(nameof(WeatherForecast));
        }

        [HttpPost]
        public async Task<IActionResult> Post(WeatherForecast weatherForecast)
        {
            weatherForecast.Date = weatherForecast.Date.Date; // Remove time
            var filter = Builders<WeatherForecast>.Filter.Where(wf => wf.Date == weatherForecast.Date);
            var update = Builders<WeatherForecast>.Update
                .SetOnInsert(wf => wf.Date, weatherForecast.Date)
                .Set(wf => wf.Summary, weatherForecast.Summary)
                .Set(wf => wf.TemperatureC, weatherForecast.TemperatureC)
                .Set(wf => wf.TemperatureF, weatherForecast.TemperatureF);
            await _collection.UpdateOneAsync(filter, update, new UpdateOptions()
            {
                IsUpsert = true,
            });

            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            return await _collection.Find(FilterDefinition<WeatherForecast>.Empty).ToListAsync();
        }
    }
}
