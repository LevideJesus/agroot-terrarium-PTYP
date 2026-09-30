namespace AgrootTerrarium.Api.Models
{
    public class Zone
    {
        public int Id {get; set;}

        public string? Name {get; set;}

        public double MoistureThreshold {get; set;} = 40.0;

        public int MistDurationSeconds {get; set;}

        public bool IsActive {get; set;} = true;

        public List<ScheduleEntry> DailyTime {get; set;} = new();

        public List<MistEvent> HistoryLog {get; set;} = new();
    }
}