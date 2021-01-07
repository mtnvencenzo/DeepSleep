namespace Api.DeepSleep.Controllers
{
    using Api.DeepSleep.Controllers.Authentication;
    using Api.DeepSleep.Controllers.Authorization;
    using Api.DeepSleep.Controllers.Binding;
    using Api.DeepSleep.Controllers.Discovery;
    using Api.DeepSleep.Controllers.Exceptions;
    using Api.DeepSleep.Controllers.Formatters;
    using Api.DeepSleep.Controllers.HelperResponses;
    using Api.DeepSleep.Controllers.Items;
    using Api.DeepSleep.Controllers.Pipeline;
    using Api.DeepSleep.Controllers.ValidationErrors;
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Discovery;
    using global::DeepSleep.Formatting;
    using global::DeepSleep.Validation;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class ServiceStartup
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<SimpleUrlBindingController>();
            services.AddTransient<MultipartController>();
            services.AddTransient<ExceptionController>();
            services.AddTransient<ReadWriteConfigurationController>();
            services.AddSingleton<AuthorizationController>();
            services.AddScoped<RequestIdController>();
            services.AddTransient<AuthenticationController>();
            services.AddTransient<EnableHeadController>();
            services.AddTransient<CommonErrorResponseProvider>();
            services.AddTransient<MethodNotFoundController>();
            services.AddTransient<ItemsController>();
            services.AddTransient<ContextDumpController>();
            services.AddTransient<DelegatedDiscoveryController>();
            services.AddTransient<AttributeDiscoveryController>();
            services.AddTransient<ValidationErrorsController>();
            services.AddTransient<AttributeDiscoveryErrorResponseProviderController>();

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

        public static IList<IRouteDiscoveryStrategy> DiscoveryStrategies()
        {
            var staticDiscovery = new StaticRouteDiscoveryStrategy();

            staticDiscovery.AddRoute(
                template: $"binding/simple/url",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: $"binding/simple/url/empty/binding/error",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "binding/simple/url/{stringVar}/mixed",
                httpMethods: new[] { "GET" },
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithMixed),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "formatters/multipart/formdata",
                httpMethods: new[] { "POST" },
                controller: typeof(MultipartController),
                endpoint: nameof(MultipartController.Post),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "formatters/multipart/formdata/custom",
                httpMethods: new[] { "POST" },
                controller: typeof(MultipartController),
                endpoint: nameof(MultipartController.PostCustom),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplemented),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromValidator),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-501" }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/notimplemented",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.NotImplementedFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "EX-501"
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGateway),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromValidator),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-502" }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/badgateway",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.BadGatewayFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "EX-502"
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeout),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromValidator),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-504" }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/gatewaytimeout",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.GatewayTimeoutFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "EX-504"
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailable),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromValidator),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-503" }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/serviceunavailable",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.ServiceUnavailableFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "EX-503"
                    }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.Unhandled),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/validator/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromValidator),
                config: new DefaultApiRequestConfiguration());

            staticDiscovery.AddRoute(
                template: "exceptions/authentication/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromAuthentication),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "EX-500" }
                });

            staticDiscovery.AddRoute(
                template: "exceptions/authorization/unhandled",
                httpMethods: new[] { "GET" },
                controller: typeof(ExceptionController),
                endpoint: nameof(ExceptionController.UnhandledFromAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new List<string> { "Success" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "EX-500"
                    }
                });

            staticDiscovery.AddRoute(
                template: "binding/body/max/request/length",
                httpMethods: new[] { "POST" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PostForMaxRequestLength),
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                endpoint: nameof(BodyBindingController.PostForBadRequestFormat));

            staticDiscovery.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethods: new[] { "PUT" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PutForBadRequestFormat));

            staticDiscovery.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethods: new[] { "PATCH" },
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PatchForBadRequestFormat));

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
                config: new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = false
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/acceptheaderoverride/xml",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithAcceptOverride),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        AcceptHeaderOverride = "text/xml"
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/acceptheaderoverride/406",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWith406AcceptOverride),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        AcceptHeaderOverride = "image/jpg; q=1, text/xml; q=0, application/json; q=0"
                    }
                });


            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/writeabletypes/text-xml",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableTypesTextXml),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        WriteableMediaTypes = new[] { "text/xml", "other/xml" }
                    }
                });


            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readabletypes/text-xml",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesTextXml),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        ReadableMediaTypes = new[] { "text/xml", "other/xml" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/writeresolver/text-xml",
                httpMethods: new[] { "GET" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableOverrides),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        WriterResolver = (args) => {
                            var formatters = new List<IFormatStreamReaderWriter>();
                            var formatter = args.Context.RequestServices.GetService<CustomXmlFormatStreamReaderWriter>();

                            if (formatter != null)
                            {
                                formatters.Add(formatter);
                            }

                            return Task.FromResult(new FormatterWriteOverrides(formatters));
                        }
                    }
                });

            staticDiscovery.AddRoute(
                 template: "pipeline/readwrite/configuration/writeresolver/none",
                 httpMethods: new[] { "GET" },
                 controller: typeof(ReadWriteConfigurationController),
                 endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableOverrides),
                 config: new DefaultApiRequestConfiguration
                 {
                     ReadWriteConfiguration = new ApiReadWriteConfiguration
                     {
                         WriterResolver = (args) => {
                             var formatters = new List<IFormatStreamReaderWriter>();
                             return Task.FromResult(new FormatterWriteOverrides(formatters));
                         }
                     }
                 });

            staticDiscovery.AddRoute(
                 template: "pipeline/readwrite/configuration/writeresolver/writeabletypes",
                 httpMethods: new[] { "GET" },
                 controller: typeof(ReadWriteConfigurationController),
                 endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableOverrides),
                 config: new DefaultApiRequestConfiguration
                 {
                     ReadWriteConfiguration = new ApiReadWriteConfiguration
                     {
                         WriterResolver = (args) => {
                             var formatters = new List<IFormatStreamReaderWriter>();
                             var formatter = args.Context.RequestServices.GetService<CustomXmlFormatStreamReaderWriter>();

                             if (formatter != null)
                             {
                                 formatters.Add(formatter);
                             }

                             return Task.FromResult(new FormatterWriteOverrides(formatters));
                         },
                         WriteableMediaTypes = new[] { "other/xml", "text/xml" }
                     }
                 });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/xml",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesTextXml),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        ReaderResolver = (args) => {
                            var formatters = new List<IFormatStreamReaderWriter>();
                            var formatter = args.Context.RequestServices.GetService<CustomXmlFormatStreamReaderWriter>();

                            if (formatter != null)
                            {
                                formatters.Add(formatter);
                            }

                            return Task.FromResult(new FormatterReadOverrides(formatters));
                        },
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/plaintext",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesPlainText),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        ReaderResolver = (args) => {

                            var formatters = new List<IFormatStreamReaderWriter>
                            {
                                new PlainTextFormatStreamReaderWriter()
                            };

                            return Task.FromResult(new FormatterReadOverrides(formatters));
                        },
                        WriterResolver = (args) =>
                        {
                            var formatters = new List<IFormatStreamReaderWriter>
                            {
                                new PlainTextFormatStreamReaderWriter()
                            };

                            return Task.FromResult(new FormatterWriteOverrides(formatters));
                        }
                    }
                });

            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/all/plus/plaintext",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesPlainText),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        ReaderResolver = (args) => {
                            var formatters = args.Context.RequestServices.GetServices<IFormatStreamReaderWriter>().ToList();

                            formatters.Add(new PlainTextFormatStreamReaderWriter());
    
                            return Task.FromResult(new FormatterReadOverrides(formatters));
                        },
                        WriterResolver = (args) => {
                            var formatters = args.Context.RequestServices.GetServices<IFormatStreamReaderWriter>().ToList();

                            formatters.Add(new PlainTextFormatStreamReaderWriter());

                            return Task.FromResult(new FormatterWriteOverrides(formatters));
                        },
                    }
                });


            staticDiscovery.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/readabletypes",
                httpMethods: new[] { "POST" },
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesTextXml),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        ReaderResolver = (args) => {
                            var formatters = new List<IFormatStreamReaderWriter>();
                            var formatter = args.Context.RequestServices.GetService<CustomXmlFormatStreamReaderWriter>();

                            if (formatter != null)
                            {
                                formatters.Add(formatter);
                            }

                            return Task.FromResult(new FormatterReadOverrides(formatters));
                        },
                        ReadableMediaTypes = new[] { "other/xml" }
                    }
                });


            staticDiscovery.AddRoute(
                template: "authentication/single/supported/schemes",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithSingleSupportedScheme),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" }
                });

            staticDiscovery.AddRoute(
                template: "authentication/multiple/supported/schemes",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithMultipleSupportedScheme),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token", "Token2" }
                });

            staticDiscovery.AddRoute(
                template: "authentication/not/defined/supported/schemes",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithSupportedSchemesNotDefined),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false
                });

            staticDiscovery.AddRoute(
                template: "authentication/empty/defined/supported/scheme",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithNoSupportedScheme),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { }
                });

            staticDiscovery.AddRoute(
                template: "authentication/anonymous/allowed",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithAllowAnonymousTrue),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true
                });

            staticDiscovery.AddRoute(
                template: "head/configured/disabled",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            staticDiscovery.AddRoute(
                template: "head/configured/disabled/maxage",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = false,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedHeaders = new string[] { "Content-Type", "X-CorrelationId" }
                    }
                });

            staticDiscovery.AddRoute(
                template: "head/configured/enabled",
                httpMethods: new[] { "GET" },
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadEnabled),
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = true,
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowCredentials = false,
                        AllowedHeaders = new string[] { "Content-Type", "X-CorrelationId" }
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
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            staticDiscovery.AddRoute(
                template: "method/not/found",
                httpMethods: new[] { "GET" },
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.Get),
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
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
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = true
                });

            staticDiscovery.AddRoute(
                template: "authorization/anonymous/allowed",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true
                });

            staticDiscovery.AddRoute(
                template: "authorization/policy/configured/success/provider",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "Default"
                    }
                });

            staticDiscovery.AddRoute(
                template: "authorization/policy/configured/failing/provider",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "DefaultFail"
                    }
                });

            staticDiscovery.AddRoute(
                template: "authorization/policy/configured/no/provider",
                httpMethods: new[] { "GET" },
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" },
                    AuthorizationConfig = new ApiResourceAuthorizationConfiguration
                    {
                        Policy = "Default1"
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


            return new List<IRouteDiscoveryStrategy>
            {
                staticDiscovery,
                new DelegatedRouteDiscoveryStrategy(
                    assemblyDirectoryPath: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    assemblyMatchPattern: "Api.DeepSleep.Controllers.dll"),
                new AttributeRouteDiscoveryStrategy(
                    assemblyDirectoryPath: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    assemblyMatchPattern: "Api.DeepSleep.Controllers.dll"
                )
            };
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
