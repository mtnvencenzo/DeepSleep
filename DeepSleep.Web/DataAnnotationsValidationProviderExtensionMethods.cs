namespace DeepSleep.Web
{
    using DeepSleep.Configuration;
    using DeepSleep.Validation;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class DataAnnotationsValidationProviderExtensionMethods
    {
        /// <summary>Uses the data annotation validations.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepDataAnnotationValidations(
            this IServiceCollection services,
            Action<IDataAnnotationsValidationConfiguration> configure = null)
        {
            var configuration = new DataAnnotationsValidationConfiguration
            {
                Continuation = ValidationContinuation.OnlyIfValid,
                ValidateAllProperties = true
            };

            if (configure != null)
            {
                configure(configuration);
            }

            services
                .AddScoped<IApiValidationProvider, DataAnnotationsValidationProvider>((p) =>
                {
                    return new DataAnnotationsValidationProvider(
                        continuation: configuration.Continuation, 
                        validateAllProperties: configuration.ValidateAllProperties);
                });

            return services;
        }
    }
}
