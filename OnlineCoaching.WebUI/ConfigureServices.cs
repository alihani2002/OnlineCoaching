using CloudinaryDotNet;
using OnlineCoaching.Web.Core.Mapping;
using OnlineCoaching.WebUI;
using OnlineCoaching.WebUI.Helper;
using System.Threading.RateLimiting;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

namespace OnlineCoaching.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // DB Context
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString!));

            services.AddDatabaseDeveloperPageExceptionFilter();


            // Identity To prevent brute force and credential stuffing
            services.AddIdentity<ApplicationUser, IdentityRole> (
                options=> {
                    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // Identity options
            services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.Zero);

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // Lockout (protect against brute-force)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // lock for 15 minutes
                options.Lockout.MaxFailedAccessAttempts = 5; // after 5 failed attempts
                options.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = "OnlineCoaching.Auth";

                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.SlidingExpiration = true;

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });




            services.Configure<SecurityStampValidatorOptions>(options =>
            options.ValidationInterval = TimeSpan.Zero);

            //builder.Services.AddScoped<ImageHelper>(provider =>
            //{
            //    var env = provider.GetRequiredService<IWebHostEnvironment>();
            //    return new ImageHelper(env, "uploads"); 
            //});


            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("LoginPolicy", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 6,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.Redirect("/Identity/Account/LoginRateLimited");
                    await Task.CompletedTask;
                };
            });


            // =====================
            // Cloudinary Integration
            // =====================
            var cloudName = builder.Configuration["Cloudinary:CloudName"];
            var apiKey = builder.Configuration["Cloudinary:ApiKey"];
            var apiSecret = builder.Configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
            builder.Services.AddScoped<IImageService, ImageService>(); 

            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            // MVC + Expressive Annotations
            services.AddControllersWithViews();
            services.AddExpressiveAnnotations();

            return services;
        }
    }
}
