namespace OnlineCoaching.Domain.Dtos.ExerciseSheet
{
    public class ExerciseSheetLogDto
    {
        public int Id { get; set; }
        public int FirstSet { get; set; }
        public int SecondSet { get; set; }
        public int ThirdSet { get; set; }
        public int FourthSet { get; set; }
        public int FifthSet { get; set; }
        public DateTime CreatedOn { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;

        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
    }
}
