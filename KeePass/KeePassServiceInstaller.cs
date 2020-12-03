using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace KeePass
{
    /// <summary>
    /// All services, dependencies, configuration etc connected do KeePass service are contained in this class,
    /// to be used with .NET DI container as a convenient way to register dependencies.
    /// </summary>
    public static class KeePassServiceInstaller
    {
        private const string Name = "KeePass";

        /// <summary>
        /// <inheritdoc cref="KeePassServiceInstaller"/>
        /// </summary>
        /// <param name="services">KeePass service will be registered into this <see cref="IServiceCollection"/> instance</param>
        /// <param name="configuration">Configuration from which appsettings file section will be read</param>
        /// <param name="settings">Optional settings object. Of provided, will completely override appsettings options</param>
        public static IServiceCollection SetupKeePassServices(this IServiceCollection services, IConfiguration configuration, KeePassSettings settings = null)
        {
            if (settings is null)
            {
                settings = new KeePassSettings();
                configuration.GetSection(Name).Bind(settings);
            }

            services.AddSingleton(settings);
            services.AddHttpClient<IKeePassService, KeePassService>((provider, client) =>
            {
                client.BaseAddress = new Uri(settings.BaseAddress);
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddPolicyHandler(KeePassPolicies.WaitAndRetryAsyncPolicy(Name, 3));

            return services;
        }
    }
}