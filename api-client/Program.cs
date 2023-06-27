using System.Net;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(name: "retry-api",
                               configureClient: client =>
                               {
                                   client.BaseAddress = new Uri("http://localhost:5122");
                                   client.DefaultRequestHeaders.Add("Accept", "application/json");
                               })
       .SetHandlerLifetime(TimeSpan.FromSeconds(5))
       .AddPolicyHandler(getRetryPolicy());

IAsyncPolicy<HttpResponseMessage> getRetryPolicy()
{
    return HttpPolicyExtensions
          .HandleTransientHttpError()
          .OrResult(response => response.StatusCode
                                    is HttpStatusCode.Conflict
                                    or HttpStatusCode.RequestTimeout)
          .WaitAndRetryAsync(retryCount: 3,
                             sleepDurationProvider: retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)),
                             onRetry: (outcome, timespan, retryAttempt, context) =>
                                      {
                                          Console
                                             .WriteLine($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}. Outcome was: {outcome.Result?.StatusCode}");
                                      });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
