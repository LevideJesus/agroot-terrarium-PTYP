using AgrootTerrarium.Api.Data;
using AgrootTerrarium.Api.Dtos;
using AgrootTerrarium.Api.Enums;
using AgrootTerrarium.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgrootTerrarium.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZonesController : ControllerBase
    {
        private readonly AgrootDbContext _context;

        public ZonesController(AgrootDbContext zoneContext)
        {
            _context = zoneContext;
        }

        [HttpGet]

        public async Task<ActionResult<List<ZoneResponseDto>>> GetZones()
        {
            var zones = await _context.Zones.Select(z => new ZoneResponseDto
            {
                Id = z.Id,
                Name = z.Name,
                MoistureThreshold = z.MoistureThreshold,
                MistDurationSeconds = z.MistDurationSeconds,
                IsActive = z.IsActive,
                ZoneType = z.ZoneType
                
                
            })
            .ToListAsync();

            return Ok(zones);

        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ZoneResponseDto>> GetZonesById(int id)
        {
            var zones = await _context.Zones.FindAsync(id);

            if(zones == null)
            {
                return NotFound();
            }

            var dto = new ZoneResponseDto
            {
                Id = zones.Id,
                Name = zones.Name,
                MoistureThreshold = zones.MoistureThreshold,
                MistDurationSeconds = zones.MistDurationSeconds,
                IsActive = zones.IsActive,
                ZoneType = zones.ZoneType
            };

            return Ok(dto);
        }

        [HttpGet("{zoneId}/schedule")]

        public async Task<ActionResult<IEnumerable<ScheduleEntryResponseDto>>> GetScheduleEntries(int zoneId)
        {
            var zone = await _context.Zones.FindAsync(zoneId);

            if(zone == null)
            {
                return NotFound();
            }

            var entries = await _context.ScheduleEntries
            .Where(e => e.ZoneId == zoneId)
            .Select(e => new ScheduleEntryResponseDto
            {
                Id = e.Id,
                ZoneId = e.ZoneId,
                TimeOfDay = e.TimeOfDay,
                IsEnabled = e.IsEnabled
            })
            .ToListAsync();

            return Ok(entries);
        }

        [HttpGet("{zoneId}/events")]

        public async Task <ActionResult<IEnumerable<MistEventResponseDto>>> GetMistEvents(int zoneId)
        {
            var zone = await _context.Zones.FindAsync(zoneId);

            if(zone == null)
            {
                return NotFound();
            }

            var events = await _context.MistEvents
                .Where(e => e.ZoneId == zoneId)
                .OrderByDescending(e => e.TriggeredAtUtc)
                .Select(e => new MistEventResponseDto
                {
                    Id = e.Id,
                    ZoneId = e.ZoneId,
                    TriggeredAtUtc = e.TriggeredAtUtc,
                    TriggerType = e.TriggerType,
                    DurationSeconds = e.DurationSeconds
                })
                .ToListAsync();

                return Ok(events);

            
        }

        [HttpPost]

        public async Task<ActionResult<ZoneResponseDto>> CreateZone([FromBody] CreateZoneDto dto)
        {
              
            var novaZona = new Zone
            {
                Name = dto.Name,
                MoistureThreshold = dto.MoistureThreshold,
                MistDurationSeconds = dto.MistDurationSeconds,
                ZoneType = dto.ZoneType
            };

            _context.Zones.Add(novaZona);

            await _context.SaveChangesAsync();

              var newDto = new ZoneResponseDto
            {
                Id = novaZona.Id,
                Name = novaZona.Name,
                MoistureThreshold = novaZona.MoistureThreshold,
                MistDurationSeconds = novaZona.MistDurationSeconds,
                IsActive = novaZona.IsActive,
                ZoneType = novaZona.ZoneType
            };

            return CreatedAtAction(nameof(GetZonesById), new {id = novaZona.Id}, newDto);
        }

       

        [HttpPost("{zoneId}/schedule")]

        public async Task<ActionResult<ScheduleEntryResponseDto>> CreateScheduleZone(int zoneId, [FromBody] CreateScheduleEntryDto dto)
        {
            var zone = await _context.Zones.FindAsync(zoneId);

            if(zone == null)
            {
                return NotFound();
            }

            var scheduleEntry = new ScheduleEntry
            {
                ZoneId = zoneId,
                TimeOfDay = dto.TimeOfDay
                
            };

            _context.ScheduleEntries.Add(scheduleEntry);

            await _context.SaveChangesAsync();

            var responseDto = new ScheduleEntryResponseDto
            {
                Id = scheduleEntry.Id,
                ZoneId = scheduleEntry.ZoneId,
                TimeOfDay = scheduleEntry.TimeOfDay,
                IsEnabled = scheduleEntry.IsEnabled
            };

            return CreatedAtAction(nameof(GetScheduleEntries), new { zoneId }, responseDto);
        }

        [HttpPost("{zoneId}/mist")]

        public async Task<ActionResult<MistEventResponseDto>> CreateMistEvent (int zoneId)
        {
            var zone = await _context.Zones.FindAsync(zoneId);

            if(zone == null)
            {
                return NotFound();
            }

            if(!zone.IsActive)
            {
                return BadRequest(new {message = "Zone is inactive."});
            }

            var mistEvent = new MistEvent
            {
                ZoneId = zone.Id,
                TriggeredAtUtc = DateTime.UtcNow,
                TriggerType = TriggerType.Manual,
                DurationSeconds = zone.MistDurationSeconds
            };

            _context.MistEvents.Add(mistEvent);

            await _context.SaveChangesAsync();

            Console.WriteLine($"Zone {zone.Name} misted (manual)");

            var dto = new MistEventResponseDto
            {
                Id = mistEvent.Id,
                ZoneId = mistEvent.ZoneId,
                TriggeredAtUtc = mistEvent.TriggeredAtUtc,
                TriggerType = mistEvent.TriggerType,
                DurationSeconds = mistEvent.DurationSeconds
            };

            return Ok(dto);
        }

        [HttpPatch("{id}")]

        public async Task<ActionResult<ZoneResponseDto>> PatchZone (int id, [FromBody] PatchZoneDto dto)
        {
            var zone = await _context.Zones.FindAsync(id);

            if(zone == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                zone.Name = dto.Name;
            }

            if (dto.MoistureThreshold.HasValue)
            {
               zone.MoistureThreshold = dto.MoistureThreshold.Value;
            }

            if (dto.MistDurationSeconds.HasValue)
            {
                zone.MistDurationSeconds = dto.MistDurationSeconds.Value;
            }

            if (dto.IsActive.HasValue)
            {
                zone.IsActive = dto.IsActive.Value;

            }

            if (dto.ZoneType.HasValue)
            {
               zone.ZoneType = dto.ZoneType.Value;
            }

            await _context.SaveChangesAsync();

            var responseDto = new ZoneResponseDto
            {
                Id = zone.Id,
                Name = zone.Name,
                MoistureThreshold = zone.MoistureThreshold,
                IsActive = zone.IsActive,
                ZoneType = zone.ZoneType,
                MistDurationSeconds = zone.MistDurationSeconds


            };

            return Ok(responseDto);
        }
    }
}