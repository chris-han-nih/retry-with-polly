Http.Polly
---
[Source Repository](https://github.com/dotnet/aspnetcore)
[Document](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly)
[Jitter](https://github.com/App-vNext/Polly/wiki/Retry-with-jitter)
## Install NuGet  Package
```bash
$ dotnet add package Microsoft.Extensions.Http.Polly
```

## Add Polly to HttpClient
```csharp
// EX) 1. 
services.AddHttpClient(name: "ResilientHttpClient",
    configureClient: client =>
    {
        client.BaseAddress = new Uri("https://base-address.com/");
    })
    .SetHandlerLifetime(TimeSpan.FromSeconds(5))  // 타임아웃을 5초로 설정 
    .AddTransientHttpErrorPolicy(builder => 
        builder.WaitAndRetryAsync(3,              // 3번 재시도
                                  retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))) // 2의 제곱초마다 재시도
                                  
// EX) 2. 
services.AddHttpClient(name: "ResilientHttpClient",
    configureClient: client =>
    {
        client.BaseAddress = new Uri("https://base-address.com/");
    })
    .SetHandlerLifetime(TimeSpan.FromSeconds(5))  // 타임아웃을 5초로 설정 
    .AddPolicyHandler(getRetryPolicy())           // 재시도 정책을 추가

IAsyncPolicy<HttpResponseMessage> getRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
```

## Use Polly
```csharp
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("ResilientHttpClient");
        var response = await client.GetAsync("/api/values");
        var content = await response.Content.ReadAsStringAsync();
        return Ok(content);
    }
}
```

