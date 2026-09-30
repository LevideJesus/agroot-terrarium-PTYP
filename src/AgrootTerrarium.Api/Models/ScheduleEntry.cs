namespace AgrootTerrarium.Api.Models
{
    public class ScheduleEntry
    {
        public int Id {get; set;}

        public int ZoneId {get; set;}

        public Zone? ParentZone {get; set;}

        public TimeOnly TimeOfDay {get; set;}

        public bool IsEnabled {get; set;} = true;
    }
}