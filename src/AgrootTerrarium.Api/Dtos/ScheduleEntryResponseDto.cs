namespace AgrootTerrarium.Api.Dtos
{
    public class ScheduleEntryResponseDto
    {
        public int Id {get; set;}

        public int ZoneId {get; set;}

        public TimeOnly TimeOfDay {get; set;}

        public bool IsEnabled {get; set;}

    }
}