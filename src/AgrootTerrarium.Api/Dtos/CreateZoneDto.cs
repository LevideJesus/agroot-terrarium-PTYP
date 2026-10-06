using System.ComponentModel.DataAnnotations;
namespace AgrootTerrarium.Api.Dtos
{
    public class CreateZoneDto
    {   
        [Required]
        public string? Name {get; set;}

        [Range(0, 100)]
        public double MoistureThreshold {get; set;}

        [Range(1, 60)]
        public int MistDurationSeconds {get; set;}

    }
}