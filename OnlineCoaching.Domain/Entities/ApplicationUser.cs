using Microsoft.AspNetCore.Identity;
namespace OnlineCoaching.Domain.Entities
{
    [Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true)]
    [Microsoft.EntityFrameworkCore.Index(nameof(UserName), IsUnique = true)]
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
        public override string? Email { get => base.Email; set => base.Email = value; }
        public int? Age { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public Client? ClientProfile { get; set; }
        public string Role { get; set; } = null!;
    }
}
