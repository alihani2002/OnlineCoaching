namespace OnlineCoaching.Domain.Dtos.Notes
{
    public class ExerciseNoteDto
    {
        public int Id { get; set; }
        public string Note { get; set; } = null!;
        public int ExerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }   
        public DateTime CreatedOn { get; set; }
    }
}
