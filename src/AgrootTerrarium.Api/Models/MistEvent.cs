using AgrootTerrarium.Api.Enums;

namespace AgrootTerrarium.Api.Models
{
    public class MistEvent
    {
        public int Id {get; set;}

        public int ZoneId {get; set;}

        public Zone? ParentZone {get; set;}

        public DateTime TriggeredAtUtc {get; set;} = DateTime.UtcNow;

        public TriggerType TriggerType {get;set;}

        public int DurationSeconds {get; set;}
    }
}