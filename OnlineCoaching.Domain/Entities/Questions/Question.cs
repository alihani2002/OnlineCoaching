namespace OnlineCoaching.Domain.Entities
{
    public class Question : BaseEntity
    {
        [Range(1, 1000, ErrorMessage = "Question number must be between 1 and 1000.")]
        public int NumberOfQuestion { get; set; }

        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(500, ErrorMessage = "Question text cannot exceed 500 characters.")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Question type is required.")]
        public QuestionType Type { get; set; }
        public bool AllowMultipleAnswers { get; set; } = false;

        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<ClientAnswer> ClientAnswers { get; set; } = new List<ClientAnswer>();

    }
}
