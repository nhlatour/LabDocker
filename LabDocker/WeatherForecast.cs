using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace LabDocker
{
    public class WeatherForecast
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
