namespace OnlineCoaching.Domain.Entities
{
    public class ExerciseNotes : BaseEntity
    {

        public string? Note { get; set; }

        public int? ExerciseId { get; set; }
        public virtual Exercise? Exercise { get; set; }

        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }
    }
}
