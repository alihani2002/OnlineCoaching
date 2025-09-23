namespace OnlineCoaching.Domain.Dtos.Notes
{
    public class CreateExerciseNoteDto
    {
        public string Note { get; set; } = null!;
        public int ExerciseId { get; set; }
        public int ClientId { get; set; }   
    }
}
