using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using BC.Auth.Server.Data;
using BC.Auth.Server.Configurations;

namespace BC.Auth.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddRazorPages();

        services.AddLocalization();

        services.AddDbContext<ApplicationDbContext>(options =>
        {            
            options.UseSqlServer(Configuration.GetConnectionString("AuthDBConnection"));            
            options.UseOpenIddict();
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        

        services.AddIdentityConfiguration();
        services.AddOpenIddictConfiguration(Configuration);

        services.AddHostedService<Worker>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        IdentityModelEventSource.ShowPII = true;

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseStatusCodePagesWithReExecute("~/error");
            //app.UseExceptionHandler("~/error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseRequestLocalization();

        //para habilitar por jhonca
        app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());


        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}"
                    );


            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        });
    }
}
