namespace OnlineCoaching.Web.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new()
            {
                UserName = "admin",
                Email = "admin@OnlineCoaching.com",
                FullName = "Admin",
                EmailConfirmed = true,
                Role = AppRoles.Admin,
                IsCompelteProfile = true
            };

            var user = await userManager.FindByEmailAsync(admin.Email);

            if (user is null)
            {
                await userManager.CreateAsync(admin, "P@ssword123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }

        public static async Task SeedCoachUserAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser coach = new()
            {
                UserName = "BadrReda",
                Email = "Badr@Coaching.com",
                FullName = "Badr Reda",
                EmailConfirmed = true,
                Role = AppRoles.Coach,
                IsCompelteProfile = true
            };

            var user = await userManager.FindByEmailAsync(coach.Email);

            if (user is null)
            {
                await userManager.CreateAsync(coach, "P@ssword123");
                await userManager.AddToRoleAsync(coach, AppRoles.Coach);
            }
        }

    }
}