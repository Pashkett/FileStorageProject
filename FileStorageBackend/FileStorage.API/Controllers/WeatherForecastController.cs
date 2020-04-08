using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FileStorage.Logger;

namespace FileStorage.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private ILoggerManager logger;

        public WeatherForecastController(ILoggerManager logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            logger.LogInfo("Here is info message from our values controller.");
            logger.LogDebug("Here is debug message from our values controller.");
            logger.LogWarn("Here is warn message from our values controller.");
            logger.LogError("Here is an error message from our values controller.");

            return new string[] { "value1", "value2" };
        }
    }
}
