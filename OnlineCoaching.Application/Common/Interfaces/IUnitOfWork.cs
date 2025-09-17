using OnlineCoaching.Domain.Entities.Gallery;

namespace OnlineCoaching.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IBaseRepository<ApplicationUser> Users { get; }

        // Identity
        IBaseRepository<Client> Clients { get; }

        // Coaching
        IBaseRepository<CoachingPackage> CoachingPackages { get; }
        IBaseRepository<CoachingPackageRequest> CoachingPackageRequests { get; }

        // Books
        IBaseRepository<Book> Books { get; }
        IBaseRepository<BookRequest> BookRequests { get; }

        // Courses
        IBaseRepository<Course> Courses { get; }
        IBaseRepository<CourseRequest> CourseRequests { get; }
        IBaseRepository<CourseVideo> CourseVideos { get; }

        // Q&A
        IBaseRepository<Question> Questions { get; }
        IBaseRepository<Option> Options { get; }
        IBaseRepository<ClientAnswer> ClientAnswers { get; }

        // Exercises
        IBaseRepository<Exercise> Exercises { get; }
        IBaseRepository<AssignExercise> AssignExercises { get; }
        IBaseRepository<Muscle> Muscles { get; }

        // Foods
        IBaseRepository<Food> Foods { get; }
        IBaseRepository<AssignFood> AssignFoods { get; }

        // Progress & Feedback
        IBaseRepository<Transformation> Transformations { get; }
        IBaseRepository<CoachFeedback> CoachFeedbacks { get; }

        //Notification
        IBaseRepository<Notification> Notifications { get; }
        IBaseRepository<Meal> Meals { get; }

        IBaseRepository<ExerciseSheetLog> ExerciseSheetLogs { get; }

        IBaseRepository<ClientAnswerOption> ClientAnswerOptions { get; }
        IBaseRepository<HomeGallary> Images { get; }
        IBaseRepository<Cooking> Cookings { get; }
        IBaseRepository<ExerciseAlternative> ExerciseAlternatives { get; }


        int Complete();

    }
}
