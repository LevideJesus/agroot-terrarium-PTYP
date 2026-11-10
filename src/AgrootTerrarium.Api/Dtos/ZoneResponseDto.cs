using AgrootTerrarium.Api.Enums;

namespace AgrootTerrarium.Api.Dtos
{
    public class ZoneResponseDto
    {
        public int Id {get; set;}

        public string? Name {get; set;}

        public double MoistureThreshold {get; set;} 

        public int MistDurationSeconds {get; set;}

        public bool IsActive {get; set;} 

        public ZoneType ZoneType {get; set;}
    }
}