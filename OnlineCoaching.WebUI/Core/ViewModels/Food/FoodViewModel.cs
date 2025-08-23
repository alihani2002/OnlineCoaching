using System.ComponentModel.DataAnnotations;

namespace OnlineCoaching.WebUI.Core.ViewModels
{
    public class FoodViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Food name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Food Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Calories are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Calories must be a positive number")]
        public int Calories { get; set; }

        [Required(ErrorMessage = "Protein is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Protein must be a positive number")]
        public int Protein { get; set; }

        [Required(ErrorMessage = "Carbohydrates are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Carbs must be a positive number")]
        [Display(Name = "Carbohydrates")]
        public int Carbs { get; set; }

        [Required(ErrorMessage = "Fats are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Fats must be a positive number")]
        public int Fats { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
    }
}
