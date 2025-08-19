using Microsoft.AspNetCore.Identity;
using OnlineCoaching.Domain.Dtos.User;

namespace OnlineCoaching.Application.Services
{
    public interface IAuthService
    {
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser?> GetUsersByIdAsync(string id);

        Task<ApplicationUser> RegisterAdminUser(ApplicationUser dto, string createdById);
        Task<IList<string>> GetUsersRolesAsync(ApplicationUser user);
        Task<IEnumerable<IdentityRole>> GetRolesAsync();
        //Task<ApplicationUser> AddUserAsync(CreateUserDto dto, string createdById);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user, IEnumerable<string> selectedRoles, string updatedById);
        //Task<ApplicationUser?> ToggleUserStatusAsync(string id, string updatedById);
        //Task<bool> ToggleClientSubscriptionAsync(string userId, string updatedById, int subscriptionDurationMonths);
        //Task MarkClientAsDeleted(string userId);
        Task<bool> AllowUserNameAsync(string? id, string username);
        Task<bool> AllowEmailAsync(string? id, string email);
        Task<bool> IsEmailConfirmedAsync(string userId);
    }
}
