namespace OnlineCoaching.Application.Services
{
    public interface IQuestionServices
    {
        IEnumerable<Question> GetQuestions();

        Task<Question?> GetQuestionById(int id);

        int GetQuestionByOptionId(int optionId);

        Option? GetOptionById(int id);

        Question AddQuestionWithOption(Question question);

        void UpdateQuestion(Question question);

        void UpdateOption(Option option);

        public void DeleteQuestion(int id);

        IEnumerable<ClientAnswer> GetClientAnswer(int ClientId);
    }
}
