using Microsoft.EntityFrameworkCore;
using AgrootTerrarium.Api.Data;
using AgrootTerrarium.Api.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AgrootDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddOpenApi();

builder.Services.AddHostedService<MistScheduleService>();

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    
    app.MapOpenApi();
    app.MapScalarApiReference();
    
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();