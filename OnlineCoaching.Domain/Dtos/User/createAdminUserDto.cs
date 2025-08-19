using OnlineCoaching.Domain.Consts;

namespace OnlineCoaching.Domain.Dtos.User
{
    public record createAdminUserDto(
     string UserName,
     string Password,
     bool IsCompelteProfile ,
     string Role,
     int SubscriptionDurationMonths,
     string? FullName = null,
     string? Email = null ,
     string? CreatedById =null ,
     string? role = null );
}
/*
   public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
        public override string? Email { get => base.Email; set => base.Email = value; }
        public bool IsCompelteProfile { get; set; } = false;
        public int? Age { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public Client? ClientProfile { get; set; }
        public string Role { get; set; } = null!;
    }
*/