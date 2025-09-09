using OnlineCoaching.Domain.Entities;

namespace OnlineCoaching.Domain.Entities
{
    public class CoachingPackageRequest : BaseEntity
    {
        public string? Titles { get; set; }
        public int Price { get; set; } = 0;
        public ClientStatus Status { get; set; } = ClientStatus.Pending;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAnswerQuestion { get; set; } = false;

        public int PackageId { get; set; }
        public CoachingPackage? Package { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }


        public ICollection<AssignExercise>? AssignExercises { get; set; }
        public ICollection<AssignFood>? AssignFoods { get; set; }
        public ICollection<Meal>? Meals { get; set; }

    }

}
