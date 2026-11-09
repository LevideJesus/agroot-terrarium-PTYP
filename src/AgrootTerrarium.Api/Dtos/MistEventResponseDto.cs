using AgrootTerrarium.Api.Enums;

namespace AgrootTerrarium.Api.Dtos
{
    public class MistEventResponseDto
    {
        public int Id {get; set;}

        public int ZoneId {get; set;}

        public DateTime TriggeredAtUtc {get; set;}

        public TriggerType TriggerType {get; set;}

        public int DurationSeconds {get; set;}
        


    }
}