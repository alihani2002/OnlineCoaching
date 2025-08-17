namespace OnlineCoaching.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string? Text { get; set; }
        public QuestionType Type { get; set; }

        public ICollection<Option>? Options { get; set; }
        public ICollection<ClientAnswer>? ClientAnswers { get; set; }
    }
}
