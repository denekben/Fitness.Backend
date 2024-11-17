namespace Domain.Entities
{
    public sealed class User
    {
        public int? Id { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Sex { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime RefreshTokenExpires { get; private set; }
        public int RoleId { get; set; } 
        public Role Role { get; set; }
        public List<Report>? Reports {  get; set; }
        public ICollection<Request> AthleteRequests { get; set; } = new List<Request>();

        public ICollection<Request> CoachRequests { get; set; } = new List<Request>();
        public List<TimeInterval>? Schedule { get; set; }
    }
}
