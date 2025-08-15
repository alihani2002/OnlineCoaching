using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

namespace OnlineCoaching.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString!));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.Configure<SecurityStampValidatorOptions>(options =>
                options.ValidationInterval = TimeSpan.Zero);

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = false;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

            });


            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
                options.LoginPath = "/Identity/Account/Login";
            });



            //services.AddTransient<IImageService, ImageService>();
            //services.AddTransient<IEmailSender, EmailSender>();
            //services.AddTransient<IEmailBodyBuilder, EmailBodyBuilder>();

            services.AddControllersWithViews();

            services.AddExpressiveAnnotations();

            //services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
            //services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));
            //services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));


            //services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            //services.AddHangfireServer();



            return services;
        }
    }
}