using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineCoaching.Application.Common.Interfaces;
using OnlineCoaching.Domain.Dtos.User;
using OnlineCoaching.Domain.Entities;

namespace OnlineCoaching.Application.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync() => await _userManager.Users.ToListAsync();

       

        public async Task<ApplicationUser?> GetUsersByIdAsync(string id) => await _userManager.FindByIdAsync(id);

        public async Task<IList<string>> GetUsersRolesAsync(ApplicationUser user) => await _userManager.GetRolesAsync(user);

        public async Task<IEnumerable<IdentityRole>> GetRolesAsync() => await _roleManager.Roles.ToListAsync();

        //public async Task<ApplicationUser> AddUserAsync(CreateUserDto dto, string createdById)
        //{
        //    bool isClient = dto.Role == AppRoles.Client;

        //    ApplicationUser user = new()
        //    {
        //        UserName = dto.UserName,
        //        Email = isClient ? null : dto.Email,
        //        FullName = isClient ? null! : dto.FullName!,
        //        CreatedById = createdById,
        //        CreatedOn = DateTime.Now,
        //        EmailConfirmed = true,
        //        Role = dto.Role,
        //        SubscriptionDurationMonths = isClient ? dto.SubscriptionDurationMonths : null,
        //    };

        //    var result = await _userManager.CreateAsync(user, dto.Password);

        //    if (!result.Succeeded)
        //        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        //    await _userManager.AddToRoleAsync(user, dto.Role);

        //    if (dto.Role == AppRoles.Coach || dto.Role == AppRoles.FitnessCoach)
        //    {
        //        var coach = new Coach
        //        {
        //            UserId = user.Id,
        //            CreatedById = createdById,
        //            IsFitnessTrainer = dto.Role == AppRoles.FitnessCoach
        //        };
        //        _unitOfWork.Coaches.Add(coach);
        //    }

        //    _unitOfWork.Complete();
        //    return user;
        //}

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user, IEnumerable<string> selectedRoles, string updatedById)
        {
            user.LastUpdatedById = updatedById;
            user.LastUpdatedOn = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesUpdated = !currentRoles.SequenceEqual(selectedRoles);

                if (rolesUpdated)
                {
                    var removedRoles = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    if (removedRoles.Succeeded)
                    {
                        await _userManager.AddToRolesAsync(user, selectedRoles);


                    }
                }

                await _userManager.UpdateSecurityStampAsync(user);

                return user;
            }

            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<ApplicationUser?> ToggleUserStatusAsync(string id, string updatedById)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return null;

            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedById = updatedById;
            user.LastUpdatedOn = DateTime.Now;

            await _userManager.UpdateAsync(user);

            if (user.IsDeleted)
                await _userManager.UpdateSecurityStampAsync(user);

            return user;
        }

        //public async Task MarkClientAsDeleted(string userId)
        //{
        //    var client = await _unitOfWork.Clients
        //        .GetQueryable()
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(c => c.UserId == userId);

        //    if (client == null)
        //        throw new Exception("Client not found");

        //    client.IsDeleted = true;

        //    if (client.User != null)
        //    {
        //        client.User.IsDeleted = true;
        //        if (!string.IsNullOrEmpty(client.User.SubscriptionJobId)
        //            && client.SubscriptionEndDate.HasValue
        //            && client.SubscriptionEndDate.Value > DateTime.Now)
        //        {
        //            BackgroundJob.Delete(client.User.SubscriptionJobId);
        //            client.User.SubscriptionJobId = null;
        //        }
        //    }

        //    _unitOfWork.Complete();
        //}

        //public async Task<bool> ToggleClientSubscriptionAsync(string id, string updatedById, int subscriptionDurationMonths)
        //{
        //    var client = await _unitOfWork.Clients
        //        .GetQueryable()
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(c => c.UserId == id);

        //    if (client is null)
        //        return false;

        //    if (client.IsDeleted)
        //    {
        //        // Reactivate
        //        client.IsDeleted = false;
        //        client.SubscriptionStartDate = DateTime.Now;
        //        client.SubscriptionEndDate = DateTime.Now.AddMonths(subscriptionDurationMonths);
        //        client.User!.IsDeleted = false;

        //        var jobId = BackgroundJob.Schedule<IAuthService>(svc =>
        //            svc.MarkClientAsDeleted(id), client.SubscriptionEndDate.Value);

        //        client.User.SubscriptionJobId = jobId;
        //    }
        //    else
        //        await MarkClientAsDeleted(id);

        //    client.LastUpdatedById = updatedById;
        //    client.LastUpdatedOn = DateTime.Now;

        //    _unitOfWork.Complete();

        //    return true;
        //}


        public async Task<bool> AllowUserNameAsync(string? id, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user is null || user.Id.Equals(id);
        }

        public async Task<bool> AllowEmailAsync(string? id, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null || user.Id.Equals(id);
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user is null ? throw new Exception("User not found") : await _userManager.IsEmailConfirmedAsync(user);
        }

      
    }
}
