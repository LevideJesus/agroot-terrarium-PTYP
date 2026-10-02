using AgrootTerrarium.Api.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AgrootTerrarium.Api.Models;
using AgrootTerrarium.Api.Enums;

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

            var saoPauloZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

            while (!stoppingToken.IsCancellationRequested)
            {
                

                DateTime utcNow = DateTime.UtcNow;

                DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, saoPauloZone);

                TimeOnly currentlyTimeOnly = TimeOnly.FromDateTime(localNow);

                Console.WriteLine($"[Time Check] UTC: {utcNow} | {DateTime.Now:T} São Paulo Local: {localNow:T} | TimeOnly Object: {currentlyTimeOnly}");

                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<AgrootDbContext>();

                var dueEntries = await dbContext.ScheduleEntries.Include(s => s.ParentZone)
                    .Where(s => s.IsEnabled &&
                    s.ParentZone != null && s.ParentZone.IsActive
                    && s.TimeOfDay.Hour == currentlyTimeOnly.Hour
                    && s.TimeOfDay.Minute == currentlyTimeOnly.Minute)
                    .ToListAsync();


                foreach(var entry in dueEntries)
                {
                    
                    var oneHourAgo = DateTime.UtcNow.AddHours(-1);

                    var recentEvents = await dbContext.MistEvents
                    .Where(e => e.ZoneId == entry.ZoneId && 
                        e.TriggerType == TriggerType.Scheduled && 
                        e.TriggeredAtUtc > oneHourAgo)
                    .ToListAsync();

                    bool alreadyExists = recentEvents.Any(e => 
                    {
                        var localEventTime = TimeZoneInfo.ConvertTimeFromUtc(e.TriggeredAtUtc, saoPauloZone);
                        
                        return localEventTime.Date == localNow.Date &&
                            localEventTime.Hour == entry.TimeOfDay.Hour &&
                            localEventTime.Minute == entry.TimeOfDay.Minute;
                    });

                    if (alreadyExists)
                    {
                        continue;
                    }

                    var newMistEvent = new MistEvent
                    {
                        ZoneId = entry.ZoneId,
                        TriggeredAtUtc = DateTime.UtcNow,
                        TriggerType = TriggerType.Scheduled,
                        DurationSeconds = entry.ParentZone!.MistDurationSeconds
                    };

                    dbContext.MistEvents.Add(newMistEvent);

                    Console.WriteLine($"Zone {entry.ParentZone.Name} misted (scheduled)");
                }

                await dbContext.SaveChangesAsync();


                Console.WriteLine($"[Tick {currentlyTimeOnly:HH:mm:ss}] Checked {dueEntries.Count} due entries");
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