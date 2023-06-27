using Microsoft.AspNetCore.Mvc;

namespace api_server.Controllers;

[ApiController]
[Route("[controller]")]
public class ServerController : ControllerBase
{
    private readonly ILogger<ServerController> _logger;
    private static int count;

    public ServerController(ILogger<ServerController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("ok")]
    public IActionResult OkResult()
    {
        _logger.LogInformation("{}: {}", DateTime.Now, ++ServerController.count);
        return Ok("Ok");
    }
    
    [HttpGet]
    [Route("not-found")]
    public IActionResult NotFoundResult()
    {
        _logger.LogInformation("{}: {}", DateTime.Now, ++ServerController.count);
        return NotFound("Not Found");
    }
    
    [HttpGet]
    [Route("internal-server-error")]
    public IActionResult InternalServerErrorResult()
    {
        _logger.LogInformation("{}: {}", DateTime.Now, ++ServerController.count);
        return StatusCode(500, "Internal Server Error");
    }
    
    [HttpGet]
    [Route("bad-request")]
    public IActionResult BadRequestResult()
    {
        _logger.LogInformation("{}: {}", DateTime.Now, ++ServerController.count);
        return BadRequest("Bad Request");
    }
    
    [HttpGet]
    [Route("time-out-randomly")]
    public async Task<IActionResult> TimeoutRandomlyResult()
    {
        _logger.LogInformation("{}: {}", DateTime.Now, ++ServerController.count);
        await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 10)));
        
        return Ok("Ok");
    }
}
