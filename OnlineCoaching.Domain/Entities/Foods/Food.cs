namespace OnlineCoaching.Domain.Entities
{
    public class Food : BaseEntity
    {
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        
        public int Gram { get; set; } 
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbs { get; set; }
        public int Fats { get; set; }

        public string? Description { get; set; }

        public ICollection<AssignFood>? AssignedFoods { get; set; }
    }
}
