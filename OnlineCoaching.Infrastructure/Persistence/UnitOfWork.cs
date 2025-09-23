using ActiveGym.Infrastructure.Persistence.Repositories;
using OnlineCoaching.Application.Common.Interfaces.Repositories;
using OnlineCoaching.Domain.Entities.Gallery;

namespace OnlineCoaching.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

        }
        public IBaseRepository<ApplicationUser> Users => new BaseRepository<ApplicationUser>(_context);

        public IBaseRepository<Client> Clients => new BaseRepository<Client>(_context);
        public IBaseRepository<CoachingPackage> CoachingPackages => new BaseRepository<CoachingPackage>(_context);
        public IBaseRepository<CoachingPackageRequest> CoachingPackageRequests => new BaseRepository<CoachingPackageRequest>(_context);

        public IBaseRepository<Book> Books => new BaseRepository<Book>(_context);
        public IBaseRepository<BookRequest> BookRequests => new BaseRepository<BookRequest>(_context);

        public IBaseRepository<Course> Courses => new BaseRepository<Course>(_context);
        public IBaseRepository<CourseRequest> CourseRequests => new BaseRepository<CourseRequest>(_context);
        public IBaseRepository<CourseVideo> CourseVideos => new BaseRepository<CourseVideo>(_context);

        public IBaseRepository<Question> Questions => new BaseRepository<Question>(_context);
        public IBaseRepository<Option> Options => new BaseRepository<Option>(_context);
        public IBaseRepository<ClientAnswer> ClientAnswers => new BaseRepository<ClientAnswer>(_context);

        public IBaseRepository<Exercise> Exercises => new BaseRepository<Exercise>(_context);
        public IBaseRepository<AssignExercise> AssignExercises => new BaseRepository<AssignExercise>(_context);
        public IBaseRepository<Muscle> Muscles => new BaseRepository<Muscle>(_context);

        public IBaseRepository<Food> Foods => new BaseRepository<Food>(_context);
        public IBaseRepository<AssignFood> AssignFoods => new BaseRepository<AssignFood>(_context);

        public IBaseRepository<Transformation> Transformations => new BaseRepository<Transformation>(_context);
        public IBaseRepository<CoachFeedback> CoachFeedbacks => new BaseRepository<CoachFeedback>(_context);
        public IBaseRepository<Notification> Notifications => new BaseRepository<Notification>(_context);

        public IBaseRepository<Meal> Meals => new BaseRepository<Meal>(_context);

        public IBaseRepository<ExerciseSheetLog> ExerciseSheetLogs => new BaseRepository<ExerciseSheetLog>(_context);
        public IBaseRepository<ClientAnswerOption> ClientAnswerOptions => new BaseRepository<ClientAnswerOption>(_context);

        public IBaseRepository<HomeGallary> Images => new BaseRepository<HomeGallary>(_context);

        public IBaseRepository<Cooking> Cookings => new BaseRepository<Cooking>(_context);
        public IBaseRepository<ExerciseAlternative> ExerciseAlternatives => new BaseRepository<ExerciseAlternative>(_context);
        public IBaseRepository<ExerciseNotes> ExerciseNotes => new BaseRepository<ExerciseNotes>(_context);


        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}