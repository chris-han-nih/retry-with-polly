namespace web_api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MainController: ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MainController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
   
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var client = _httpClientFactory.CreateClient("meta-api");
        var response = await client.GetAsync("http://localhost:5000");
        var content = await response.Content.ReadAsStringAsync();
        return Ok(content);
    } 
}