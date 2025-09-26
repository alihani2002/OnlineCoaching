namespace OnlineCoaching.WebUI.Models.RequestPackage
{
    public class AssignedFreePackVM
    {
        public int ClientId { get; set; }
        public int RequestId { get; set; }
        public int? PackageId { get; set; }

        public List<AssignExercise> AssignedExercises { get; set; } = new List<AssignExercise>();
        public List<AssignFood> AssignedFoods { get; set; } = new List<AssignFood>();

        public List<Exercise> AvailableExercises { get; set; } = new List<Exercise>();
        public List<Food> AvailableFoods { get; set; } = new List<Food>();
    }
}
