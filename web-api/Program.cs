using System.Net;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(name: "meta-api", client =>
       {
           client.BaseAddress = new Uri("http://localhost:5000");
           client.DefaultRequestHeaders.Add("Accept", "application/json");
       })
       .AddTransientHttpErrorPolicy(policy => policy.OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
                                                    .WaitAndRetryAsync(30, retryAttemp => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttemp))));

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
