namespace Domain.Entities
{
    public sealed class Role
    {
        public int? Id { get; set; }
        public string RoleName { get; set; }
        public List<User>? Users { get; set; }

        public static string Athlete => nameof(Athlete);
        public static string Coach => nameof(Coach);
    }
}
