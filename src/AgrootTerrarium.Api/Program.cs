using Microsoft.EntityFrameworkCore;
using AgrootTerrarium.Api.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AgrootDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();