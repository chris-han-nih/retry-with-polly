namespace web_api.Controllers;

using System.Net;
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
        var response = await client.GetAsync("http://localhost:5122/WeatherForecast/");
        var content = await response.Content.ReadAsStringAsync();
        
        return response.StatusCode == HttpStatusCode.OK
                   ? Ok(content)
                   : StatusCode(response.StatusCode.GetHashCode(), content);
    } 
}