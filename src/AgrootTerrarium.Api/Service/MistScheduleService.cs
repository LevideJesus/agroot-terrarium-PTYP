using AgrootTerrarium.Api.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AgrootTerrarium.Api.Service
{
    public class MistScheduleService : BackgroundService
    {
        
            private readonly IServiceScopeFactory _scopeFactory;

            public MistScheduleService(IServiceScopeFactory scopeFactory)
            {
                _scopeFactory = scopeFactory;
            }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            TimeZoneInfo saoPauloZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

            while (!stoppingToken.IsCancellationRequested)
            {
                

                DateTime utcNow = DateTime.UtcNow;

                DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, saoPauloZone);

                TimeOnly currentlyTimeOnly = TimeOnly.FromDateTime(localNow);

                Console.WriteLine($"[Time Check] UTC: {utcNow} | {DateTime.Now:T} São Paulo Local: {localNow:T} | TimeOnly Object: {currentlyTimeOnly}");

                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<AgrootDbContext>();

                Console.WriteLine(dbContext.Zones.Count());

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("[Worker Heartbeat] Service shutdown requested during delay. Exiting gracefully.");
                    break;

                }
            }

           
        }
    }    
}