namespace OnlineCoaching.Domain.Entities
{
    public class ClientAnswerOption : BaseEntity
    {
        public int ClientAnswerId { get; set; }
        public ClientAnswer? ClientAnswer { get; set; }

        public int OptionId { get; set; }
        public Option? Option { get; set; }
    }
}
