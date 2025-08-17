namespace OnlineCoaching.Domain.Entities
{
    public class Muscle : BaseEntity
    {
        [Required, MaxLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public ICollection<Exercise>? Exercises { get; set; }
    }
}
