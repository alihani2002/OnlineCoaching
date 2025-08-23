namespace OnlineCoaching.Domain.Dtos
{
    public class FoodDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbs { get; set; }
        public int Fats { get; set; }
        public string? Description { get; set; }
        public bool ? IsDeleted { get; set; }
    }
}
