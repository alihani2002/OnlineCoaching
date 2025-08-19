namespace OnlineCoaching.WebUI.Models.Auth
{
    public record AdminRegisterViewModel(
     string UserName,
     string Password,
     bool IsCompelteProfile,
     string Role,
     int SubscriptionDurationMonths,
     string? FullName = null,
     string? Email = null,
     string? CreatedById = null,
     string? role = null );
}
