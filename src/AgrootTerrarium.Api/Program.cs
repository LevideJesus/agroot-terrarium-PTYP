using Microsoft.EntityFrameworkCore;
using AgrootTerrarium.Api.Data;
using AgrootTerrarium.Api.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AgrootDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddOpenApi();

builder.Services.AddHostedService<MistScheduleService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();