using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using PierresTreats.Models;


namespace PierresTreats
{
  class Program
{
    static void Main(string[] args)
    {

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext <PierresTreatsContext> (
                          dbContextOptions => dbContextOptions
                            .UseMySql(
                              builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                            )
                          )
                        );

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                      .AddRoles<IdentityRole>()
                      .AddEntityFrameworkStores<PierresTreatsContext>()
                      .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
          //Default Password Settings.
          options.Password.RequireDigit = true;
          options.Password.RequireLowercase = true;
          options.Password.RequireNonAlphanumeric = true;
          options.Password.RequireUppercase = true;
          options.Password.RequiredLength = 6;
          options.Password.RequiredUniqueChars = 1;
        });

        builder.Services.ConfigureApplicationCookie(opts =>
        {
          opts.AccessDeniedPath = "/Stop/Index";
        });

        WebApplication app = builder.Build();

        // app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
}