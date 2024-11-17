namespace Domain.Entities
{
    public sealed class Exercise
    {
        public int? Id { get; set; }
        public int? RepeatQuantity { get; set; }
        public int? SetQuantity { get; set; }
        public string? Name { get; set; }
        public int ReportId { get; set; }

        public Exercise(int id, int? repeatQuantity, int? setQuantity, string? name, int reportId)
        {
            Id = id;
            RepeatQuantity = repeatQuantity;
            SetQuantity = setQuantity;
            Name = name;
            ReportId = reportId;
        }

        public Exercise(int? repeatQuantity, int? setQuantity, string? name, int reportId)
        {
            RepeatQuantity = repeatQuantity;
            SetQuantity = setQuantity;
            Name = name;
            ReportId = reportId;
        }
    }
}
