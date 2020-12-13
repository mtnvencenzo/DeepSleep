namespace Api.DeepSleep.Controllers
{
    using Api.DeepSleep.Controllers.Binding;
    using Api.DeepSleep.Controllers.Exceptions;
    using Api.DeepSleep.Controllers.Formatters;
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Validation;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;

    public static class ServiceStartup
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<SimpleUrlBindingController>();
            services.AddTransient<MultipartController>();
            services.AddTransient<ExceptionController>();
            services.AddTransient<CommonErrorResponseProvider>();
            services.AddTransient<NotImplementedExceptionThrowValidator>(); // Only one of these to check both injection and no-injection resolution
            services.AddScoped<IAuthenticationProvider, Ex500AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex501AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex502AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex503AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex504AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, SuccessAuthenticationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex500AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex501AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex502AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex503AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex504AuthorizationProvider>();
        }

        public static IApiRoutingTable InitialzeRoutes()
        {
            var table = new DefaultApiRoutingTable();

            table.AddRoute(
                template: $"binding/simple/url",
                httpMethod: "GET",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "binding/simple/url/{stringVar}/resource",
                httpMethod: "GET",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithRoute),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "binding/simple/url/{stringVar}/mixed",
                httpMethod: "GET",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithMixed),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "formatters/multipart/formdata",
                httpMethod: "POST",
                controller: typeof(MultipartController),
                endpoint: nameof(MultipartController.Post),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "formatters/multipart/formdata/custom",
                httpMethod: "POST",
                controller: typeof(MultipartController),
                endpoint: nameof(MultipartController.PostCustom),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/notimplemented",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplemented),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/validator/notimplemented",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromValidator),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/authentication/notimplemented",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-501" }
                });

            table.AddRoute(
                template: "exceptions/authorization/notimplemented",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "EX-501"
                    }
                });

            table.AddRoute(
                template: "exceptions/badgateway",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGateway),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/validator/badgateway",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromValidator),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/authentication/badgateway",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-502" }
                });

            table.AddRoute(
                template: "exceptions/authorization/badgateway",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "EX-502"
                    }
                });

            table.AddRoute(
                template: "exceptions/gatewaytimeout",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeout),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/validator/gatewaytimeout",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromValidator),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/authentication/gatewaytimeout",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-504" }
                });

            table.AddRoute(
                template: "exceptions/authorization/gatewaytimeout",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "EX-504"
                    }
                });

            table.AddRoute(
                template: "exceptions/serviceunavailable",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailable),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/validator/serviceunavailable",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromValidator),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/authentication/serviceunavailable",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-503" }
                });

            table.AddRoute(
                template: "exceptions/authorization/serviceunavailable",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "EX-503"
                    }
                });

            table.AddRoute(
                template: "exceptions/unhandled",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.Unhandled),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/validator/unhandled",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromValidator),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "exceptions/authentication/unhandled",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-500" }
                });

            table.AddRoute(
                template: "exceptions/authorization/unhandled",
                httpMethod: "GET",
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "EX-500"
                    }
                });

            return table;
        }

        public static IApiRequestConfiguration DefaultRequestConfiguration()
        {
            return new DefaultApiRequestConfiguration
            {
                AllowAnonymous = true,
                ApiErrorResponseProvider = (p) => p.GetService<CommonErrorResponseProvider>()
            };
        }

        public static IApiValidationProvider DefaultValidationProvider(IServiceProvider serviceProvider)
        {
            return new DefaultApiValidationProvider(serviceProvider)
                .RegisterInvoker<TypeBasedValidationInvoker>();
        }
    }
}
