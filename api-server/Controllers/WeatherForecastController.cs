using Microsoft.AspNetCore.Mvc;

namespace api_server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private static int count = 0;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("{}: {}", DateTime.Now, ++WeatherForecastController.count);
        return StatusCode(StatusCodes.Status500InternalServerError);
        // return NotFound();
        // return Conflict();
        // return BadRequest("Bad Request");
        // return Ok();
    }
}
