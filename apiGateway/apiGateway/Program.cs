using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ApiGateway.json", optional: false, reloadOnChange: true);

// Register services
builder.Services.AddHealthChecks();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseAuthorization();

// 🔹 Map health checks before Ocelot
app.MapHealthChecks("/health");

// 🔹 Map controllers (if any)
app.MapControllers();

// 🔹 Await Ocelot LAST – it takes over the pipeline
await app.UseOcelot();

app.Run();
