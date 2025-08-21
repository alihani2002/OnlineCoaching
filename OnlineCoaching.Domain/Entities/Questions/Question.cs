namespace OnlineCoaching.Domain.Entities
{
    public class Question : BaseEntity
    {
        public int NumberOfQuestion { get; set; }
        public string? Text { get; set; }
        public QuestionType Type { get; set; }

        public ICollection<Option>? Options { get; set; } = [];
        public ICollection<ClientAnswer>? ClientAnswers { get; set; }
        public bool AllowMultipleAnswers { get; set; } = false;

    }
}
