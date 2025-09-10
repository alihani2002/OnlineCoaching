using OnlineCoaching.Domain.Enums;

namespace OnlineCoaching.Application.Services
{
    public class QuestionServices : IQuestionServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Question> GetQuestions() => _unitOfWork.Questions
                .GetQueryable().Include(q => q.Options!)
                .ThenInclude(o => o.SelectedOptions)
                .Where(q=> !q.IsDeleted)
                .OrderBy(o => o.NumberOfQuestion).ToList();

        public async Task<Question?> GetQuestionById(int id) =>
            await _unitOfWork.Questions
            .GetQueryable().
            Include(q => q.Options!).
            FirstOrDefaultAsync(q => q.Id == id);

        public int GetQuestionByOptionId(int optionId)
        {
            var option = _unitOfWork.Options.GetQueryable().Include(o => o.Question).FirstOrDefault(q => q.Id == optionId);
            return option?.QuestionId ?? 0;
        }

        public Option? GetOptionById(int id) => _unitOfWork.Options
                .GetQueryable()
                .FirstOrDefault(o => o.Id == id);

        public Question AddQuestionWithOption(Question question)
        {
            // If it's an OpenText (or OpenEnded) question -> skip options
            if (question.Type == QuestionType.OpenText)
            {
                question.Options = null!; // ✅ No options created
            }
            else
            {
                if(question.Type == QuestionType.MultipleChoice)
                {
                    question.AllowMultipleAnswers = true;
                }
                // Normal handling for choice questions
                question.Options ??= [];

                var distinctOptions = question.Options
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text)) // remove blanks
                    .DistinctBy(o => o.Text!.Trim())
                    .Select(o => new Option
                    {
                        Text = o.Text!.Trim()
                    })
                    .ToList();

                question.Options = distinctOptions;
            }

            _unitOfWork.Questions.Add(question);
            _unitOfWork.Complete();

            return question;
        }


        public void UpdateQuestion(Question question)
        {
            var existingQuestion = _unitOfWork.Questions.GetQueryable()
                .Include(q => q.Options!)
                .FirstOrDefault(q => q.Id == question.Id);

            if (existingQuestion != null)
            {
                existingQuestion.AllowMultipleAnswers = question.AllowMultipleAnswers;
                existingQuestion.NumberOfQuestion = question.NumberOfQuestion;
                existingQuestion.Text = question.Text;
                existingQuestion.Type = question.Type;

                if (existingQuestion.Options != null)
                {
                    foreach (var optionDto in question.Options!)
                    {
                        if (optionDto.Id == 0)
                        {
                            var newOption = new Option
                            {
                                Text = optionDto.Text,
                                Question = existingQuestion
                            };
                            existingQuestion.Options.Add(newOption);
                        }
                        else
                        {
                            var existingOption = existingQuestion.Options.FirstOrDefault(o => o.Id == optionDto.Id);
                            if (existingOption != null)
                            {
                                existingOption.Text = optionDto.Text;
                            }
                        }
                    }

                    var optionsToRemove = existingQuestion.Options
                        .Where(o => o.Id > 0 && !question.Options.Any(dto => dto.Id == o.Id))
                        .ToList();

                    foreach (var option in optionsToRemove)
                    {
                        _unitOfWork.Options.Remove(option);
                    }
                }
            }

            _unitOfWork.Complete();
        }

        public void UpdateOption(Option option)
        {
            var exitingOption = _unitOfWork.Options.GetQueryable().FirstOrDefault(o => o.Id == option.Id)
            ?? throw new ArgumentException("Option not found");
            exitingOption.Text = option.Text;
            _unitOfWork.Complete();
        }


        public void DeleteQuestion(int id)
        {
            var question = _unitOfWork.Questions.GetQueryable()
                .Include(q => q.Options!)
                .ThenInclude(c => c.SelectedOptions)
                .FirstOrDefault(q => q.Id == id);

            if (question != null)
            {
                foreach (var option in question.Options!)
                {
                    _unitOfWork.Options.Remove(option);
                }

                _unitOfWork.Questions.Remove(question);
                _unitOfWork.Complete();
            }
        }


        public async Task<bool> QuestionExists(int id) =>
            await _unitOfWork.Questions.GetQueryable().AnyAsync(e => e.Id == id);


        public async Task<bool> OptionExists(int id) =>
            await _unitOfWork.Options.GetQueryable().AnyAsync(e => e.Id == id);


        public IEnumerable<ClientAnswer> GetClientAnswer(int clientId)
        {
            return _unitOfWork.ClientAnswers.GetQueryable()
                .Include(ca => ca.Question) 
                .Include(ca => ca.SelectedOptions)
                    .ThenInclude(cao => cao.Option)
                .ThenInclude(o => o!.Question) 
                .Where(ca => ca.ClientId == clientId)
                .ToList();
        }
    }
}