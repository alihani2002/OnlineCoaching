namespace OnlineCoaching.Domain.Entities
{
    public class ExerciseSheetLog : BaseEntity
    {
        public int FirstSet { get; set; }
        public int SecondSet { get; set; }
        public int ThirdSet { get; set; }
        public int FourthSet { get; set; }
        public int FifthSet { get; set; }


        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;
    }
}
