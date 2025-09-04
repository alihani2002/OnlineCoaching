namespace OnlineCoaching.Domain.Dtos
{
    public class CreateFoodDto
    {
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbs { get; set; }
        public int Fats { get; set; }
        public int Gram { get; set; }
        public string? Description { get; set; } = null;
    }
}
