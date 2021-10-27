using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Services;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infrastructure.Repositories;
using ChatApplication.Infrastructure;
using ChatApplication.Domain.Hubs;

namespace ChatApplication
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config) => _config = config;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            services.AddMvc(mvcOptions =>
            {
                mvcOptions.EnableEndpointRouting = false;
            });
            services.AddSignalR();

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IQueueService, QueueService>();

            services.AddScoped<IChatRepository, ChatRepository>();

            services.AddIdentity<User, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
