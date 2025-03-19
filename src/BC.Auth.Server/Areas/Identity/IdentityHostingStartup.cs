[assembly: HostingStartup(typeof(BC.Auth.Server.Areas.Identity.IdentityHostingStartup))]
namespace BC.Auth.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
