﻿namespace Api.DeepSleep.Controllers
{
    using Api.DeepSleep.Controllers.Authentication;
    using Api.DeepSleep.Controllers.Authorization;
    using Api.DeepSleep.Controllers.Binding;
    using Api.DeepSleep.Controllers.Cookies;
    using Api.DeepSleep.Controllers.Discovery;
    using Api.DeepSleep.Controllers.Exceptions;
    using Api.DeepSleep.Controllers.Formatters;
    using Api.DeepSleep.Controllers.HelperResponses;
    using Api.DeepSleep.Controllers.Items;
    using Api.DeepSleep.Controllers.Pipeline;
    using Api.DeepSleep.Controllers.RequestPipeline;
    using Api.DeepSleep.Controllers.Validation;
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Discovery;
    using global::DeepSleep.Media;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public static class ServiceStartup
    {
        /// <summary>Registers the services.</summary>
        /// <param name="services">The services.</param>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<SimpleUrlBindingController>();
            services.AddTransient<MultipartController>();
            services.AddTransient<ExceptionController>();
            services.AddTransient<ReadWriteConfigurationController>();
            services.AddTransient<AuthorizationController>();
            services.AddTransient<RequestIdController>();
            services.AddTransient<AuthenticationController>();
            services.AddTransient<EnableHeadController>();
            services.AddTransient<CommonErrorResponseProvider>();
            services.AddTransient<MethodNotFoundController>();
            services.AddTransient<ItemsController>();
            services.AddTransient<ContextDumpController>();
            services.AddTransient<DelegatedDiscoveryController>();
            services.AddTransient<AttributeDiscoveryController>();
            services.AddTransient<ErrorResponseProviderController>();
            services.AddTransient<AttributeDiscoveryErrorResponseProviderController>();
            services.AddTransient<RequestPipelineController>();
            services.AddTransient<CookiesController>();

            // Only one of these to check both injection and no-injection resolution
            services.AddTransient<NotImplementedExceptionThrowValidator>();

            services.AddScoped<IAuthenticationProvider, Ex500AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex501AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex502AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex503AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Ex504AuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, SuccessAuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, StaticTokenAuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, Static2TokenAuthenticationProvider>();

            services.AddScoped<IAuthorizationProvider, Ex500AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex501AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex502AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex503AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, Ex504AuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, DefaultAuthorizationProvider>();
            services.AddScoped<IAuthorizationProvider, DefaultFailingAuthorizationProvider>();

            services.AddScoped<CustomXmlFormatStreamReaderWriter>();
        }

        /// <summary>Discoveries the strategies.</summary>
        /// <returns></returns>
        public static IList<IDeepSleepDiscoveryStrategy> DiscoveryStrategies()
        {
            var staticDiscovery = new StaticRouteDiscoveryStrategy();

            staticDiscovery.AddRoute(
                template: $"binding/simple/url",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: $"binding/simple/url/empty/binding/error",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DeepSleepRequestConfiguration
                {
                    ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                    {
                        UriBindingValueError = string.Empty
                    }
                });

            staticDiscovery.AddRoute(
                template: $"binding/simple/url/custom/binding/error",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DeepSleepRequestConfiguration
                {
                    ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                    {
                        UriBindingValueError = "Test {paramName}."
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/simple/url/{stringVar}/resource",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithRoute),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "binding/simple/url/{stringVar}/mixed",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithMixed),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "formatters/multipart/formdata",
                httpMethods: new[] { "POST" },
                controller: typeof(MultipartController),
                endpoint: nameof(MultipartController.Post),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "formatters/multipart/formdata/custom",
                httpMethods: new[] { "POST" },
                controller: typeof(MultipartController),
                endpoint: nameof(MultipartController.PostCustom),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplemented),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromValidator),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromAuthentication),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<Ex501AuthenticationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<SuccessAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<Ex501AuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGateway),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromValidator),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromAuthentication),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<Ex502AuthenticationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<SuccessAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<Ex502AuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeout),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromValidator),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromAuthentication),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<Ex504AuthenticationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<SuccessAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<Ex504AuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailable),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromValidator),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromAuthentication),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<Ex503AuthenticationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<SuccessAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<Ex503AuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.Unhandled),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromValidator),
                config: new DeepSleepRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromAuthentication),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<Ex500AuthenticationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<SuccessAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<Ex500AuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/body/max/request/length",
                httpMethods: new[] { "POST" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PostForMaxRequestLength),
                config: new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestLength = 5
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/body/max/request/length",
                httpMethods: new[] { "PUT" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PutForMaxRequestLength),
                config: new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestLength = 5
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/body/max/request/length",
                httpMethods: new[] { "PATCH" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PatchForMaxRequestLength),
                config: new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestLength = 5
                    }
                });


            staticDiscovery.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethods: new[] { "POST" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PostForBadRequestFormat),
                config: new DeepSleepRequestConfiguration
                {
                    ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                    {
                        HttpStatusMode = ValidationHttpStatusMode.CommonHttpSpecification
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethods: new[] { "PUT" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PutForBadRequestFormat),
                config: new DeepSleepRequestConfiguration
                {
                    ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                    {
                        HttpStatusMode = ValidationHttpStatusMode.CommonHttpSpecification
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethods: new[] { "PATCH" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PatchForBadRequestFormat),
                config: new DeepSleepRequestConfiguration
                {
                    ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                    {
                        HttpStatusMode = ValidationHttpStatusMode.CommonHttpSpecification
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/simple/post",
                httpMethods: new[] { "POST" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimplePost));

            staticDiscovery.AddRoute(
                template: "binding/simple/put",
                httpMethods: new[] { "PUT" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimplePut));

            staticDiscovery.AddRoute(
                template: "binding/simple/patch",
                httpMethods: new[] { "PATCH" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimplePatch));

            staticDiscovery.AddRoute(
                template: "binding/simple/multipart",
                httpMethods: new[] { "POST" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimpleMultipart));

            staticDiscovery.AddRoute(
                template: "binding/multipart/custom",
                httpMethods: new[] { "POST" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.MultipartCustom));

            staticDiscovery.AddRoute(
                template: "formatters/accept",
                httpMethods: new[] { "GET" },
                controller: typeof(AcceptController),
                endpoint: nameof(AcceptController.Get));

            staticDiscovery.AddRoute(
                template: "pipeline/request/id",
                httpMethods: new[] { "GET" },
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.Get));

            staticDiscovery.AddRoute(
                template: "pipeline/request/id/exception",
                httpMethods: new[] { "GET" },
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.GetException));

            staticDiscovery.AddRoute(
                template: "pipeline/request/id/nocontent",
                httpMethods: new[] { "GET" },
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.GetNoContent));

            staticDiscovery.AddRoute(
                template: "pipeline/request/id/disabled",
                httpMethods: new[] { "GET" },
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.GetDisabled),
                config: new DeepSleepRequestConfiguration
                {
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/acceptheaderoverride/xml",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithAcceptOverride),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        AcceptHeaderOverride = "text/xml"
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/acceptheaderoverride/406",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWith406AcceptOverride),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        AcceptHeaderOverride = "image/jpg; q=1, text/xml; q=0, application/json; q=0"
                    }
                });


            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/writeabletypes/text-xml",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableTypesTextXml),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        WriteableMediaTypes = new[] { "text/xml", "other/xml" }
                    }
                });


            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readabletypes/text-xml",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesTextXml),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        ReadableMediaTypes = new[] { "text/xml", "other/xml" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/writeresolver/text-xml",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableOverrides),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        WriterResolver = (serviceProvider) =>
                        {
                            var formatters = new List<IDeepSleepMediaSerializer>();
                            var formatter = serviceProvider.GetService<CustomXmlFormatStreamReaderWriter>();

                            if (formatter != null)
                            {
                                formatters.Add(formatter);
                            }

                            return Task.FromResult(new MediaSerializerWriteOverrides(formatters));
                        }
                    }
                });

            staticDiscovery.AddRoute(
                 template: "pipeline/readwrite/configuration/writeresolver/none",
                 httpMethods: new[] { "GET" },
                 controller: typeof(ReadWriteConfigurationController),
                 endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableOverrides),
                 config: new DeepSleepRequestConfiguration
                 {
                     ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                     {
                         WriterResolver = (args) =>
                         {
                             var formatters = new List<IDeepSleepMediaSerializer>();
                             return Task.FromResult(new MediaSerializerWriteOverrides(formatters));
                         }
                     }
                 });

            staticDiscovery.AddRoute(
                 template: "pipeline/readwrite/configuration/writeresolver/writeabletypes",
                 httpMethods: new[] { "GET" },
                 controller: typeof(ReadWriteConfigurationController),
                 endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableOverrides),
                 config: new DeepSleepRequestConfiguration
                 {
                     ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                     {
                         WriterResolver = (serviceProvider) =>
                         {
                             var formatters = new List<IDeepSleepMediaSerializer>();
                             var formatter = serviceProvider.GetService<CustomXmlFormatStreamReaderWriter>();

                             if (formatter != null)
                             {
                                 formatters.Add(formatter);
                             }

                             return Task.FromResult(new MediaSerializerWriteOverrides(formatters));
                         },
                         WriteableMediaTypes = new[] { "other/xml", "text/xml" }
                     }
                 });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/xml",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesTextXml),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        ReaderResolver = (serviceProvider) =>
                        {
                            var formatters = new List<IDeepSleepMediaSerializer>();
                            var formatter = serviceProvider.GetService<CustomXmlFormatStreamReaderWriter>();

                            if (formatter != null)
                            {
                                formatters.Add(formatter);
                            }

                            return Task.FromResult(new MediaSerializerReadOverrides(formatters));
                        },
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/plaintext",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesPlainText),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        ReaderResolver = (args) =>
                        {

                            var formatters = new List<IDeepSleepMediaSerializer>
                            {
                                new PlainTextFormatStreamReaderWriter()
                            };

                            return Task.FromResult(new MediaSerializerReadOverrides(formatters));
                        },
                        WriterResolver = (args) =>
                        {
                            var formatters = new List<IDeepSleepMediaSerializer>
                            {
                                new PlainTextFormatStreamReaderWriter()
                            };

                            return Task.FromResult(new MediaSerializerWriteOverrides(formatters));
                        }
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/all/plus/plaintext",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesPlainText),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        ReaderResolver = (serviceProvider) =>
                        {
                            var formatters = serviceProvider.GetServices<IDeepSleepMediaSerializer>().ToList();

                            formatters.Add(new PlainTextFormatStreamReaderWriter());

                            return Task.FromResult(new MediaSerializerReadOverrides(formatters));
                        },
                        WriterResolver = (serviceProvider) =>
                        {
                            var formatters = serviceProvider.GetServices<IDeepSleepMediaSerializer>().ToList();

                            formatters.Add(new PlainTextFormatStreamReaderWriter());

                            return Task.FromResult(new MediaSerializerWriteOverrides(formatters));
                        },
                    }
                });


            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/readabletypes",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesTextXml),
                config: new DeepSleepRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                    {
                        ReaderResolver = (serviceProvider) =>
                        {
                            var formatters = new List<IDeepSleepMediaSerializer>();
                            var formatter = serviceProvider.GetService<CustomXmlFormatStreamReaderWriter>();

                            if (formatter != null)
                            {
                                formatters.Add(formatter);
                            }

                            return Task.FromResult(new MediaSerializerReadOverrides(formatters));
                        },
                        ReadableMediaTypes = new[] { "other/xml" }
                    }
                });


            staticDiscovery.AddRoute(
                template: "authentication/single/supported/schemes",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithSingleSupportedScheme),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<StaticTokenAuthenticationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "authentication/multiple/supported/schemes",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithMultipleSupportedScheme),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<StaticTokenAuthenticationProvider>(),
                        new ApiAuthenticationComponent<Static2TokenAuthenticationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "authentication/not/defined/supported/schemes",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithSupportedSchemesNotDefined),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false
                });

            staticDiscovery.AddRoute(
                template: "authentication/empty/defined/supported/scheme",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithNoSupportedScheme),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false
                });

            staticDiscovery.AddRoute(
                template: "authentication/anonymous/allowed",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithAllowAnonymousTrue),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = true
                });

            staticDiscovery.AddRoute(
                template: "head/configured/disabled",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            staticDiscovery.AddRoute(
                template: "head/configured/disabled/maxage",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = false,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        MaxAgeSeconds = 100
                    }
                });


            staticDiscovery.AddRoute(
                template: "head/configured/disabled/origin",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = false,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "https://test1.com", "https://test2.com" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/disabled/exposeheaders",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = false,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        ExposeHeaders = new string[] { "X-Header1", "X-Header2" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/disabled/allowheaders",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = false,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedHeaders = new string[] { "Content-Type" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/enabled",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadEnabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = true,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowCredentials = false
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/enabled/maxage",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadEnabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = true,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowCredentials = false,
                        MaxAgeSeconds = 150
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/enabled/origin",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadEnabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = true,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowCredentials = false,
                        AllowedOrigins = new string[] { "https://test1.com", "https://test2.com" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/enabled/exposeheaders",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadEnabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = true,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowCredentials = false,
                        ExposeHeaders = new string[] { "X-Header1", "X-Header2" },
                        AllowedOrigins = new string[] { "https://test1.com", "https://test2.com" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/enabled/allowheaders",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadEnabled),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = true,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowCredentials = false,
                        AllowedHeaders = new string[] { "Content-Type" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/default",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithDefaultEnableHead));

            staticDiscovery.AddRoute(
                template: "head/explicit",
                httpMethods: new[] { "HEAD" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.ExplicitHead));

            staticDiscovery.AddRoute(
                template: "head/explicit",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithExplicitHead),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            staticDiscovery.AddRoute(
                template: "method/not/found",
                httpMethods: new[] { "GET" },
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.Get),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = true
                });

            staticDiscovery.AddRoute(
                template: "method/not/found",
                httpMethods: new[] { "PUT" },
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.Put));

            staticDiscovery.AddRoute(
                template: "method/not/found/nohead",
                httpMethods: new[] { "GET" },
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.GetNoHead),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            staticDiscovery.AddRoute(
                template: "method/not/found/nohead",
                httpMethods: new[] { "PUT" },
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.PutNoHead));

            staticDiscovery.AddRoute(
                template: "route/not/found",
                httpMethods: new[] { "GET" },
                controller: typeof(NotFoundController),
                endpoint: nameof(NotFoundController.Get),
                config: new DeepSleepRequestConfiguration
                {
                    EnableHeadForGetRequests = true
                });

            staticDiscovery.AddRoute(
                template: "authorization/anonymous/allowed",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = true
                });

            staticDiscovery.AddRoute(
                template: "authorization/policy/configured/success/provider",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<StaticTokenAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<DefaultAuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "authorization/policy/configured/failing/provider",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<StaticTokenAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<DefaultFailingAuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "authorization/policy/configured/mixed/providers",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<StaticTokenAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<DefaultAuthorizationProvider>(),
                        new ApiAuthorizationComponent<DefaultFailingAuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "authorization/policy/configured/no/provider",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = false,
                    AuthenticationProviders = new List<IAuthenticationComponent>
                    {
                        new ApiAuthenticationComponent<StaticTokenAuthenticationProvider>()
                    },
                    AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<DefaultAuthorizationProvider>()
                    }
                });

            staticDiscovery.AddRoute(
                template: "context/items",
                httpMethods: new[] { "GET" },
                controller: typeof(ItemsController),
                endpoint: nameof(ItemsController.GetWithItems));

            staticDiscovery.AddRoute(
                template: "context/dump",
                httpMethods: new[] { "GET" },
                controller: typeof(ContextDumpController),
                endpoint: nameof(ContextDumpController.GetDump));

            staticDiscovery.AddRoute(
                template: "context/dump",
                httpMethods: new[] { "POST" },
                controller: typeof(ContextDumpController),
                endpoint: nameof(ContextDumpController.PostDump));

            staticDiscovery.AddRoute(
                template: "helper/responses/ok",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Ok));

            staticDiscovery.AddRoute(
                template: "helper/responses/ok/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Ok_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/ok/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Ok_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/created",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Created));

            staticDiscovery.AddRoute(
                template: "helper/responses/created/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Created_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/created/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Created_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/accepted",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Accepted));

            staticDiscovery.AddRoute(
                template: "helper/responses/accepted/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Accepted_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/accepted/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Accepted_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/nocontent",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NoContent));

            staticDiscovery.AddRoute(
                template: "helper/responses/nocontent/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NoContent_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/badrequest",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.BadRequest));

            staticDiscovery.AddRoute(
                template: "helper/responses/badrequest/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.BadRequest_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/badrequest/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.BadRequest_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/unauthorized",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Unauthorized));

            staticDiscovery.AddRoute(
                template: "helper/responses/unauthorized/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Unauthorized_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/unauthorized/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Unauthorized_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/forbidden",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Forbidden));

            staticDiscovery.AddRoute(
                template: "helper/responses/forbidden/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Forbidden_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/forbidden/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Forbidden_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/notfound",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NotFound));

            staticDiscovery.AddRoute(
                template: "helper/responses/notfound/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NotFound_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/notfound/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NotFound_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/conflict",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Conflict));

            staticDiscovery.AddRoute(
                template: "helper/responses/conflict/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Conflict_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/conflict/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Conflict_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/movedpermanently",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.MovedPermanently));

            staticDiscovery.AddRoute(
                template: "helper/responses/movedpermanently/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.MovedPermanently_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/found",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Found));

            staticDiscovery.AddRoute(
                template: "helper/responses/found/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Found_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/permanentredirect",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.PermanentRedirect));

            staticDiscovery.AddRoute(
                template: "helper/responses/permanentredirect/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.PermanentRedirect_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/temporaryredirect",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.TemporaryRedirect));

            staticDiscovery.AddRoute(
                template: "helper/responses/temporaryredirect/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.TemporaryRedirect_Headers));

            staticDiscovery.AddRoute(
                template: "helper/responses/unprocessableentity",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.UnprocessableEntity));

            staticDiscovery.AddRoute(
                template: "helper/responses/unprocessableentity/null",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.UnprocessableEntity_Null));

            staticDiscovery.AddRoute(
                template: "helper/responses/unprocessableentity/headers",
                httpMethods: new[] { "GET" },
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.UnprocessableEntity_Headers));


            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return new List<IDeepSleepDiscoveryStrategy>
            {
                staticDiscovery,
                new DelegatedRouteDiscoveryStrategy(
                    assemblyDirectoryPath: assemblyPath,
                    assemblyMatchPattern: "Api.DeepSleep.Controllers.dll"),
                new AttributeRouteDiscoveryStrategy(
                    assemblyDirectoryPath: assemblyPath,
                    assemblyMatchPattern: "Api.DeepSleep.Controllers.dll"
                )
            };
        }

        /// <summary>Defaults the request configuration.</summary>
        /// <returns></returns>
        public static IDeepSleepRequestConfiguration DefaultRequestConfiguration()
        {
            return new DeepSleepRequestConfiguration
            {
                AllowAnonymous = true,
                ApiErrorResponseProvider = (p) => p.GetService<CommonErrorResponseProvider>(),
                AuthenticationProviders = new List<IAuthenticationComponent>
                {
                    new ApiAuthenticationComponent<Ex500AuthenticationProvider>(),
                    new ApiAuthenticationComponent<Ex501AuthenticationProvider>(),
                    new ApiAuthenticationComponent<Ex502AuthenticationProvider>(),
                    new ApiAuthenticationComponent<Ex503AuthenticationProvider>(),
                    new ApiAuthenticationComponent<Ex504AuthenticationProvider>(),
                    new ApiAuthenticationComponent<SuccessAuthenticationProvider>(),
                    new ApiAuthenticationComponent<StaticTokenAuthenticationProvider>(),
                    new ApiAuthenticationComponent<Static2TokenAuthenticationProvider>(),
                }
            };
        }
    }
}
