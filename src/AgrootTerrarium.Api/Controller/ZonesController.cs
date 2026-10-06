using AgrootTerrarium.Api.Data;
using AgrootTerrarium.Api.Dtos;
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

        public async Task<ActionResult<List<Zone>>> GetZones()
        {
            var zones = await _context.Zones.ToListAsync();

            return Ok(zones);

        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetZonesById(int id)
        {
            var zones = await _context.Zones.FindAsync(id);

            if(zones == null)
            {
                return NotFound();
            }

            return NoContent();
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
                TimeOfDay = e.TimeOfDay
            })
            .ToListAsync();

            return Ok(entries);
        }

        [HttpPost]

        public async Task<ActionResult<Zone>> CreateZone([FromBody] CreateZoneDto dto)
        {
              
            var novaZona = new Zone
            {
                Name = dto.Name,
                MoistureThreshold = dto.MoistureThreshold,
                MistDurationSeconds = dto.MistDurationSeconds
            };

             _context.Zones.Add(novaZona);

             await _context.SaveChangesAsync();

             return CreatedAtAction(nameof(GetZonesById), new {id = novaZona.Id}, novaZona);
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
    }
}