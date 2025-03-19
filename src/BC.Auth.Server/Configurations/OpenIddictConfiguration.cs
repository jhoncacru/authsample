using BC.Auth.Server.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace BC.Auth.Server.Configurations
{
    public static class OpenIddictConfiguration
    {
        public static void AddOpenIddictConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var certPath = configuration["AppSettings:CertificateFilePath"];
            var certPassword = configuration["AppSettings:CertificatePassword"]; // Contraseña del certificado
            var certificate = new X509Certificate2(certPath, certPassword);

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                           .UseDbContext<ApplicationDbContext>();

                    //options.UseQuartz();
                })
                .AddServer(options =>
                {
                    // Enable the authorization, logout, token and userinfo endpoints.
                    options.SetAuthorizationEndpointUris("connect/authorize")
                       //.SetDeviceEndpointUris("connect/device")
                       .SetIntrospectionEndpointUris("connect/introspect")
                       .SetEndSessionEndpointUris("connect/logout")
                       .SetTokenEndpointUris("connect/token")
                       .SetUserInfoEndpointUris("connect/userinfo")
                       .SetEndUserVerificationEndpointUris("connect/verify");

                    options.AllowAuthorizationCodeFlow()
                           .AllowHybridFlow()
                           .AllowClientCredentialsFlow()
                           .AllowRefreshTokenFlow()
                           .SetAccessTokenLifetime(TimeSpan.FromMinutes(60));

                    options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "dataEventRecords");


                    //options.AddDevelopmentEncryptionCertificate()
                    //       .AddDevelopmentSigningCertificate();


                    //options.AddEncryptionKey(new SymmetricSecurityKey(
                    //   Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

                    //options.AddSigningCertificate(certificate);
                    //var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("clave-secreta-de-256-bits-para-p"));
                    //options.AddEncryptionKey(signingKey);
                    //options.AddSigningKey(signingKey);

                    //  options.AddEphemeralEncryptionKey()
                    //.AddEphemeralSigningKey();


                    options.AddEncryptionKey(new SymmetricSecurityKey(
                       Convert.FromBase64String(configuration["AppSettings:EncriptionKeyBase64"])));

                    // Register the signing credentials.
                    //options.AddDevelopmentSigningCertificate();


                    //options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String("Kgva8Jh/K/ui4ECtinfw8JJxdxbw9Fi07x7Qv9xaZXI=")));
                    //options.AddSigningKey(new SymmetricSecurityKey(Convert.FromBase64String("Kgva8Jh/K/ui4ECtinfw8JJxdxbw9Fi07x7Qv9xaZXI=")));


                    options.AddSigningCertificate(certificate);


                    options.UseAspNetCore()
                           .EnableAuthorizationEndpointPassthrough()
                           .EnableEndSessionEndpointPassthrough()
                           .EnableTokenEndpointPassthrough()
                           .EnableUserInfoEndpointPassthrough()
                           .EnableStatusCodePagesIntegration();

                    options.DisableAccessTokenEncryption();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
        }
    }
}
