namespace OnlineCoaching.Domain.Dtos.User
{
    public record CreateUserDto(
     string UserName,
     string Password,
     string Role,
     int SubscriptionDurationMonths,
     string? FullName = null,
     string? Email = null
 );

}
