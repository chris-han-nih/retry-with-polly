using System.Net;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(name: "meta-api",
                               client =>
                               {
                                   // client.BaseAddress = new Uri("http://localhost:5122");
                                   // client.DefaultRequestHeaders.Add("Accept", "application/json");
                               })
       .SetHandlerLifetime(TimeSpan.FromSeconds(5))
        // .AddPolicyHandler(getRetryPolicy());
       .AddTransientHttpErrorPolicy(policy => policy.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                                                    .OrResult(msg => msg.StatusCode == HttpStatusCode.Forbidden)
                                                    .OrResult(msg => msg.StatusCode
                                                                  == HttpStatusCode.InternalServerError)
                                                    .WaitAndRetryAsync(2,
                                                                       retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)),
                                                                       (outcome, timespan, retryAttempt, context) =>
                                                                       {
                                                                           Console .WriteLine($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}. Outcome was: {outcome.Result?.StatusCode}");
                                                                       }));
static IAsyncPolicy<HttpResponseMessage> getRetryPolicy()
{
    return HttpPolicyExtensions
          .HandleTransientHttpError()
          .OrResult(response => response.StatusCode == HttpStatusCode.NotFound)
          .WaitAndRetryAsync(6, 
                             _ => TimeSpan.FromSeconds(5),
                             (outcome, timespan, retryAttempt, context) =>
                             {
                                 Console.WriteLine($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}. Outcome was: {outcome.Result?.StatusCode}");
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
