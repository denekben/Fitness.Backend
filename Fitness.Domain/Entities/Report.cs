namespace Domain.Entities
{
    public sealed class Report
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public List<Exercise>? Exercises { get; set; }
        public int? TariffId { get; set; }
        public Tariff Tariff { get; set; }
        public User Coach { get; set; }
        public int CoachId { get; set; }
        public User Athlete { get; set; }
        public int AthleteId { get; set; }

        public Report(int id, string? description, DateTime? dateTime, int? tariffId, int coachId, int athleteId)
        {
            Id = id;
            Description = description;
            DateTime = dateTime;
            TariffId = tariffId;
            CoachId = coachId;
            AthleteId = athleteId;
        }

        public Report(string? description, DateTime? dateTime, int? tariffId, int coachId, int athleteId)
        {
            Description = description;
            DateTime = dateTime;
            TariffId = tariffId;
            CoachId = coachId;
            AthleteId = athleteId;
        }
    }
}
