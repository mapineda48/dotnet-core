using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Climate.Models;
using System.Net.Http;

namespace Climate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly string _keyWeather = Environment.GetEnvironmentVariable("WEATHER_KEY_API");

        private readonly string _keyNews = Environment.GetEnvironmentVariable("NEWS_KEY_API");

        private readonly HttpClient httpClient = new HttpClient();
        public ExternalController(IConfiguration config, AppDbContext context)
        {
            _context = context;

            if (_keyWeather == null)
            {
                throw new InvalidOperationException("missing enviroment variable \"WEATHER_KEY_API\"");
            }


            if (_keyNews == null)
            {
                throw new InvalidOperationException("missing enviroment variable \"NEWS_KEY_API\"");
            }
        }

        [HttpGet]
        public async Task<IActionResult> fetchExternalApi([FromHeader(Name = "History-ID")] int historyId, [FromQuery] string location)
        {
            if (historyId < 1)
            {
                return BadRequest("missing header \"History-ID\"");
            }

            if (String.IsNullOrEmpty(location))
            {
                return BadRequest("missing query location");
            }

            var history = await _context.Historys.FindAsync(historyId);

            if (history == null)
            {
                return BadRequest("Not Found History");
            }


            //Fetch the JSON string from URL.
            string weather = await httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={_keyWeather}");

            string news = await httpClient.GetStringAsync($"https://newsapi.org/v2/top-headlines?q={location}&apiKey={_keyNews}"); ;

            string data = $"{{\"news\":{news},\"weather\":{weather}}}";

            Location Location = new Location { History = history, Name = location };

            _context.Locations.Add(Location);

            await _context.SaveChangesAsync();


            //Return the JSON string.
            return Content(data);

        }
    }
}