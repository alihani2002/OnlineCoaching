using Microsoft.Extensions.DependencyInjection;
using OnlineCoaching.Application.Services;

namespace OnlineCoaching.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICoachingPackageRequestService, CoachingPackageRequestService>();
            services.AddScoped<ICoachingPackageServices, CoachingPackageServices>();
            services.AddScoped<IExerciseServices, ExerciseServices>();
            services.AddScoped<IMuscleServices, MuscleServices>();
            services.AddScoped<IFoodServices, FoodServices>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionServices, QuestionServices>();

            return services;

        }
    }
}
