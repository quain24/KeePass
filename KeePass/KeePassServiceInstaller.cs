using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        /// <param name="settings">Optional settings object. If provided, will completely override appsettings options</param>
        public static IServiceCollection SetupKeePassServices(this IServiceCollection services, IConfiguration configuration, KeePassSettings settings = null)
        {
            settings ??= TryToReadSettingsFrom(configuration);

            services.AddSingleton(settings);
            services.AddHttpClient(Name, (provider, client) =>
            {
                client.BaseAddress = new Uri(settings.BaseAddress);
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddPolicyHandler(KeePassPolicies.WaitAndRetryAsyncPolicy(Name, 3));

            services.AddScoped<IKeePassService, KeePassService>();
            return services;
        }

        private static KeePassSettings TryToReadSettingsFrom(IConfiguration configuration)
        {
            // Binding this way to force usage of KeePassSettings constructor with parameters - forced validation on creation.
            var tmpSettings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            configuration.GetSection(Name).Bind(tmpSettings);

            return new KeePassSettings
            (
                tmpSettings.TryGetValue(nameof(KeePassSettings.Username), out var username) ? username : string.Empty,
                tmpSettings.TryGetValue(nameof(KeePassSettings.Password), out var password) ? password : string.Empty,
                tmpSettings.TryGetValue(nameof(KeePassSettings.BaseAddress), out var baseAddress) ? baseAddress : string.Empty,
                tmpSettings.TryGetValue(nameof(KeePassSettings.TokenEndpoint), out var tokenEndpoint) ? tokenEndpoint : string.Empty,
                tmpSettings.TryGetValue(nameof(KeePassSettings.RestEndpoint), out var restEndpoint) ? restEndpoint : string.Empty
            );
        }
    }
}