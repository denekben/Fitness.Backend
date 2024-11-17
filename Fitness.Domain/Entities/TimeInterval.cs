namespace Domain.Entities
{
    public sealed class TimeInterval
    {
        public int? Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public TimeInterval(DateTime start, DateTime end, int userId)
        {
            StartTime = start;
            EndTime = end;
            UserId = userId;
        }

        public TimeInterval(int id, DateTime start, DateTime end, int userId)
        {
            Id = id;
            StartTime = start;
            EndTime = end;
            UserId = userId;
        }
    }
}
