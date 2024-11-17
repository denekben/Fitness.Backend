namespace Domain.Entities
{
    public sealed class Tariff
    {
        public int? Id { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public List<Report>? Reports { get; set; }

        public Tariff(float price, string? description)
        {
            Price = price;
            Description = description;
        }

        public Tariff(int id, float price, string? description)
        {
            Id = id;
            Price = price;
            Description = description;
        }
    }
}
