namespace OnlineCoaching.Domain.Entities
{
    public class CourseRequest : BaseEntity
    {
        public string? CourseName { get; set; }
        public int Price { get; set; } = 0;
        public ClientStatus Status { get; set; } = ClientStatus.Pending;
        public bool IsApproved { get; set; } = false;

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }
       
    }
}
