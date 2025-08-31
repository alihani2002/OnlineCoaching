namespace OnlineCoaching.Domain.Entities
{
    public class Question : BaseEntity
    {
        public int NumberOfQuestion { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public bool AllowMultipleAnswers { get; set; } = false;

        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<ClientAnswer> ClientAnswers { get; set; } = new List<ClientAnswer>();

    }
}
