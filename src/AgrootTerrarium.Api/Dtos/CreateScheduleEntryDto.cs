using System.ComponentModel.DataAnnotations;

namespace AgrootTerrarium.Api.Dtos
{
    public class CreateScheduleEntryDto
    {
        [Required]

        public TimeOnly TimeOfDay {get; set;}
    }
}