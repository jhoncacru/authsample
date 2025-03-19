using BC.Auth.Domain;
using BC.Auth.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace BC.Auth.Server.Configurations
{
    public static class IdentityConfiguration
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {


            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                //opt.SignIn.RequireConfirmedEmail = true;
                //opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders()
              //.AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation")
              .AddDefaultUI();
            //.AddTokenProvider<Fido2UserTwoFactorTokenProvider>("FIDO2");

            //services.Configure<Fido2Configuration>(Configuration.GetSection("fido2"));
            //services.AddScoped<Fido2Store>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Configure Identity to use the same JWT claims as OpenIddict instead
                // of the legacy WS-Federation claims it uses by default (ClaimTypes),
                // which saves you from doing the mapping in your authorization controller.
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
                options.ClaimsIdentity.EmailClaimType = Claims.Email;

                // Note: to require account confirmation before login,
                // register an email sender service (IEmailSender) and
                // set options.SignIn.RequireConfirmedAccount to true.
                //
                // For more information, visit https://aka.ms/aspaccountconf.
                options.SignIn.RequireConfirmedAccount = false;
            });

            // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
            // (like pruning orphaned authorizations/tokens from the database) at regular intervals.
            //services.AddQuartz(options =>
            //{
            //    options.UseSimpleTypeLoader();
            //    options.UseInMemoryStore();
            //});

            // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
            //services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            //.AddOpenIdConnect("KeyCloak", "KeyCloak", options =>
            //{
            //    options.SignInScheme = "Identity.External";
            //    //Keycloak server
            //    options.Authority = Configuration.GetSection("Keycloak")["ServerRealm"];
            //    //Keycloak client ID
            //    options.ClientId = Configuration.GetSection("Keycloak")["ClientId"];
            //    //Keycloak client secret in user secrets for dev
            //    options.ClientSecret = Configuration.GetSection("Keycloak")["ClientSecret"];
            //    //Keycloak .wellknown config origin to fetch config
            //    options.MetadataAddress = Configuration.GetSection("Keycloak")["Metadata"];
            //    //Require keycloak to use SSL

            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.SaveTokens = true;
            //    options.ResponseType = OpenIdConnectResponseType.Code;
            //    options.RequireHttpsMetadata = false; //dev

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = ClaimTypes.Role,
            //        ValidateIssuer = true
            //    };
            //});
        }
    }
}
