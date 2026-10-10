using Microsoft.EntityFrameworkCore;
using PersonDataManagementSystem.Application.Interfaces;
using PersonDataManagementSystem.Application.Services;
using PersonDataManagementSystem.Data;
using Scalar.AspNetCore;

DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? throw new InvalidOperationException("CONNECTION_STRING is not set.");

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPersonService, PersonService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "I am alive!");

app.Run();
