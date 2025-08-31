namespace OnlineCoaching.Domain.Entities
{
    public class ClientAnswer : BaseEntity
    {
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        // Instead of OptionId
        public ICollection<ClientAnswerOption> SelectedOptions { get; set; } = new List<ClientAnswerOption>();

        public string? AnswerText { get; set; }

    }
}
