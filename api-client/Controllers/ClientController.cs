namespace web_api.Controllers;

using System.Net;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ClientController: ControllerBase
{
    private readonly ILogger<ClientController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientController(ILogger<ClientController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    [Route("call-ok")]
    public async Task<IActionResult> CallOk()
    {
        var client = _httpClientFactory.CreateClient("retry-api");
        var response = await client.GetAsync("/server/ok");
        var content = await response.Content.ReadAsStringAsync();
        
        _logger.LogInformation("Response: {}", content);
        
        return response.StatusCode == HttpStatusCode.OK
                   ? Ok(content)
                   : StatusCode(response.StatusCode.GetHashCode(), content);
    } 
    
    [HttpGet]
    [Route("call-not-found")]
    public async Task<IActionResult> CallNotFound()
    {
        var client = _httpClientFactory.CreateClient("retry-api");
        var response = await client.GetAsync("/server/not-found");
        var content = await response.Content.ReadAsStringAsync();
        
        _logger.LogInformation("Response: {}", content);
        
        return response.StatusCode == HttpStatusCode.OK
                   ? Ok(content)
                   : StatusCode(response.StatusCode.GetHashCode(), content);
    } 
    
    [HttpGet]
    [Route("call-internal-server-error")]
    public async Task<IActionResult> CallInternalServerError()
    {
        var client = _httpClientFactory.CreateClient("retry-api");
        var response = await client.GetAsync("/server/internal-server-error");
        var content = await response.Content.ReadAsStringAsync();
        
        _logger.LogInformation("Response: {}", content);
        
        return response.StatusCode == HttpStatusCode.OK
                   ? Ok(content)
                   : StatusCode(response.StatusCode.GetHashCode(), content);
    } 
    
    [HttpGet]
    [Route("call-bad-request")]
    public async Task<IActionResult> CallBadRequest()
    {
        var client = _httpClientFactory.CreateClient("retry-api");
        var response = await client.GetAsync("/server/bad-request");
        var content = await response.Content.ReadAsStringAsync();
        
        _logger.LogInformation("Response: {}", content);
        
        return response.StatusCode == HttpStatusCode.OK
                   ? Ok(content)
                   : StatusCode(response.StatusCode.GetHashCode(), content);
    } 
    
    [HttpGet]
    [Route("call-time-out-randomly")]
    public async Task<IActionResult> CallTimeoutRandomly()
    {
        var client = _httpClientFactory.CreateClient("retry-api");
        var response = await client.GetAsync("/server/time-out-randomly");
        var content = await response.Content.ReadAsStringAsync();
        
        _logger.LogInformation("Response: {}", content);
        
        return response.StatusCode == HttpStatusCode.OK
                   ? Ok(content)
                   : StatusCode(response.StatusCode.GetHashCode(), content);
    } 
}