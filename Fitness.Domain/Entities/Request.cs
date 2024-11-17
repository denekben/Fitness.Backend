namespace Domain.Entities
{
    public sealed class Request
    {
        public int? Id { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Subject { get; set; }
        public User Coach { get; set; }
        public int CoachId { get; set; }
        public User Athlete { get; set; }
        public int AthleteId { get; set; }
        public Tariff Tariff { get; set; }
        public int? TariffId { get; set; }
        public bool IsAccepted { get; set; }

        public Request(DateTime? dateTime, string? subject, int coachId, int athleteId, int? tariffId)
        {
            DateTime = dateTime;
            Subject = subject;
            CoachId = coachId;
            AthleteId = athleteId;
            TariffId = tariffId;
            IsAccepted = false;
        }

        public Request(int id, DateTime? dateTime, string? subject, int? tariffId)
        {
            Id = id;
            DateTime = dateTime;
            Subject = subject;
            TariffId = tariffId;
            IsAccepted = false;
        }
    }
}
