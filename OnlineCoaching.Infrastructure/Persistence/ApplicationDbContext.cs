using Microsoft.EntityFrameworkCore;
using OnlineCoaching.Domain.Entities.Gallery;

namespace OnlineCoaching.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Exercise> Exercises { get; set; } 
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<AssignExercise> AssignExercises { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<AssignFood> AssignFoods { get; set; }
        public DbSet<Transformation> Transformations { get; set; }
        public DbSet<CoachFeedback> CoachFeedbacks { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseVideo> CourseVideos { get; set; }
        public DbSet<CourseRequest> CourseRequests { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookRequest> BookRequests { get; set; }
        public DbSet<CoachingPackage> CoachingPackages { get; set; }
        public DbSet<CoachingPackageRequest> CoachingPackageRequests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ClientAnswer> ClientAnswers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Meal> Meals { get; set ; }
        public DbSet<ExerciseSheetLog> ExerciseSheetLogs { get ; set; }
        public DbSet<ClientAnswerOption> ClientAnswerOptions { get; set; }
        public DbSet<HomeGallary> Images { get; set; }
        public DbSet<Cooking> Cookings { get; set; }
        public DbSet<ExerciseAlternative> ExerciseAlternatives { get; set; }
        public DbSet<ExerciseNotes> ExerciseNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            builder.Entity<ExerciseAlternative>()
           .HasKey(ea => new { ea.ExerciseId, ea.AlternativeExerciseId });

            builder.Entity<ExerciseAlternative>()
                .HasOne(ea => ea.Exercise)
                .WithMany(e => e.Alternatives)
                .HasForeignKey(ea => ea.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ExerciseAlternative>()
                .HasOne(ea => ea.AlternativeExercise)
                .WithMany(e => e.AlternativeTo)
                .HasForeignKey(ea => ea.AlternativeExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

        }

    }
}
