namespace OnlineCoaching.Domain.Dtos.ExerciseSheet
{
    public class CreateExerciseSheetDto
    {
        public int FirstSet { get; set; }
        public int SecondSet { get; set; }
        public int ThirdSet { get; set; }
        public int FourthSet { get; set; }
        public int FifthSet { get; set; }

        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }


        public int ClientId { get; set; }
        public string? Client { get; set; }
        public string ? ClientName { get; set; }

        public int ExerciseId { get; set; }
        public string? Exercise { get; set; }
        public string? ExerciseName { get; set; }
    }
}
