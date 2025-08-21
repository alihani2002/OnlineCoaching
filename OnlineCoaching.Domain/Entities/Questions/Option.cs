namespace OnlineCoaching.Domain.Entities
{
    public class Option : BaseEntity
    {
        public string? Text { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        public ICollection<ClientAnswer>? ClientAnswers { get; set; }
        public ICollection<ClientAnswerOption> SelectedOptions { get; set; } = [];

    }
}
