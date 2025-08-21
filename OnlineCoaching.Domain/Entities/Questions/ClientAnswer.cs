namespace OnlineCoaching.Domain.Entities
{
    public class ClientAnswer : BaseEntity
    {
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        public int? OptionId { get; set; }
        public Option? Option { get; set; }

        public ICollection<ClientAnswerOption> SelectedOptions { get; set; } = [];

        public string? AnswerText { get; set; }
       
    }
}
