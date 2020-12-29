namespace Api.DeepSleep.Controllers
{
    using Api.DeepSleep.Controllers.Authentication;
    using Api.DeepSleep.Controllers.Authorization;
    using Api.DeepSleep.Controllers.Binding;
    using Api.DeepSleep.Controllers.Exceptions;
    using Api.DeepSleep.Controllers.Formatters;
    using Api.DeepSleep.Controllers.HelperResponses;
    using Api.DeepSleep.Controllers.Items;
    using Api.DeepSleep.Controllers.Pipeline;
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Formatting;
    using global::DeepSleep.Validation;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                template: $"binding/simple/url/empty/binding/error",
                httpMethod: "GET",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DefaultApiRequestConfiguration
                {
                    ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                    {
                        UriBindingValueError = string.Empty
                    }
                });

            table.AddRoute(
                template: $"binding/simple/url/custom/binding/error",
                httpMethod: "GET",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DefaultApiRequestConfiguration
                {
                    ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                    {
                        UriBindingValueError = "100|Test {paramName}."
                    }
                });

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

            table.AddRoute(
                template: "binding/body/max/request/length",
                httpMethod: "POST",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PostForMaxRequestLength),
                config: new DefaultApiRequestConfiguration
                {
                    MaxRequestLength = 5
                });

            table.AddRoute(
                template: "binding/body/max/request/length",
                httpMethod: "PUT",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PutForMaxRequestLength),
                config: new DefaultApiRequestConfiguration
                {
                    MaxRequestLength = 5
                });

            table.AddRoute(
                template: "binding/body/max/request/length",
                httpMethod: "PATCH",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PatchForMaxRequestLength),
                config: new DefaultApiRequestConfiguration
                {
                    MaxRequestLength = 5
                });


            table.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethod: "POST",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PostForBadRequestFormat));

            table.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethod: "PUT",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PutForBadRequestFormat));

            table.AddRoute(
                template: "binding/body/bad/request/format",
                httpMethod: "PATCH",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.PatchForBadRequestFormat));

            table.AddRoute(
                template: "binding/simple/post",
                httpMethod: "POST",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimplePost));

            table.AddRoute(
                template: "binding/simple/put",
                httpMethod: "PUT",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimplePut));

            table.AddRoute(
                template: "binding/simple/patch",
                httpMethod: "PATCH",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimplePatch));

            table.AddRoute(
                template: "binding/simple/multipart",
                httpMethod: "POST",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.SimpleMultipart));

            table.AddRoute(
                template: "binding/multipart/custom",
                httpMethod: "POST",
                controller: typeof(BodyBindingController),
                endpoint: nameof(BodyBindingController.MultipartCustom));

            table.AddRoute(
                template: "formatters/accept",
                httpMethod: "GET",
                controller: typeof(AcceptController),
                endpoint: nameof(AcceptController.Get));

            table.AddRoute(
                template: "pipeline/request/id",
                httpMethod: "GET",
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.Get));

            table.AddRoute(
                template: "pipeline/request/id/exception",
                httpMethod: "GET",
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.GetException));

            table.AddRoute(
                template: "pipeline/request/id/nocontent",
                httpMethod: "GET",
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.GetNoContent));

            table.AddRoute(
                template: "pipeline/request/id/disabled",
                httpMethod: "GET",
                controller: typeof(RequestIdController),
                endpoint: nameof(RequestIdController.GetDisabled),
                config: new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = false
                });

            table.AddRoute(
                template: "pipeline/readwrite/configuration/acceptheaderoverride/xml",
                httpMethod: "GET",
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithAcceptOverride),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        AcceptHeaderOverride = "text/xml"
                    }
                });

            table.AddRoute(
                template: "pipeline/readwrite/configuration/acceptheaderoverride/406",
                httpMethod: "GET",
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWith406AcceptOverride),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        AcceptHeaderOverride = "image/jpg; q=1, text/xml; q=0, application/json; q=0"
                    }
                });


            table.AddRoute(
                template: "pipeline/readwrite/configuration/writeabletypes/text-xml",
                httpMethod: "GET",
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.GetWithWriteableTypesTextXml),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        WriteableMediaTypes = new[] { "text/xml", "other/xml" }
                    }
                });


            table.AddRoute(
                template: "pipeline/readwrite/configuration/readabletypes/text-xml",
                httpMethod: "POST",
                controller: typeof(ReadWriteConfigurationController),
                endpoint: nameof(ReadWriteConfigurationController.PostWithReadableTypesTextXml),
                config: new DefaultApiRequestConfiguration
                {
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        ReadableMediaTypes = new[] { "text/xml", "other/xml" }
                    }
                });

            table.AddRoute(
                template: "pipeline/readwrite/configuration/writeresolver/text-xml",
                httpMethod: "GET",
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

            table.AddRoute(
                 template: "pipeline/readwrite/configuration/writeresolver/none",
                 httpMethod: "GET",
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

            table.AddRoute(
                 template: "pipeline/readwrite/configuration/writeresolver/writeabletypes",
                 httpMethod: "GET",
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

            table.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/xml",
                httpMethod: "POST",
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

            table.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/plaintext",
                httpMethod: "POST",
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

            table.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/all/plus/plaintext",
                httpMethod: "POST",
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


            table.AddRoute(
                template: "pipeline/readwrite/configuration/readresolver/readabletypes",
                httpMethod: "POST",
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


            table.AddRoute(
                template: "authentication/single/supported/schemes",
                httpMethod: "GET",
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithSingleSupportedScheme),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" }
                });

            table.AddRoute(
                template: "authentication/multiple/supported/schemes",
                httpMethod: "GET",
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithMultipleSupportedScheme),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token", "Token2" }
                });

            table.AddRoute(
                template: "authentication/not/defined/supported/schemes",
                httpMethod: "GET",
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithSupportedSchemesNotDefined),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false
                });

            table.AddRoute(
                template: "authentication/empty/defined/supported/scheme",
                httpMethod: "GET",
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithNoSupportedScheme),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { }
                });

            table.AddRoute(
                template: "authentication/anonymous/allowed",
                httpMethod: "GET",
                controller: typeof(AuthenticationController),
                endpoint: nameof(AuthenticationController.GetWithAllowAnonymousTrue),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true
                });

            table.AddRoute(
                template: "head/configured/disabled",
                httpMethod: "GET",
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithConfiguredHeadDisbabled),
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            table.AddRoute(
                template: "head/configured/disabled/maxage",
                httpMethod: "GET",
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


            table.AddRoute(
                template: "head/configured/disabled/origin",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/disabled/exposeheaders",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/disabled/allowheaders",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/enabled",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/enabled/maxage",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/enabled/origin",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/enabled/exposeheaders",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/enabled/allowheaders",
                httpMethod: "GET",
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

            table.AddRoute(
                template: "head/configured/default",
                httpMethod: "GET",
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithDefaultEnableHead));

            table.AddRoute(
                template: "head/explicit",
                httpMethod: "HEAD",
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.ExplicitHead));

            table.AddRoute(
                template: "head/explicit",
                httpMethod: "GET",
                controller: typeof(EnableHeadController),
                endpoint: nameof(EnableHeadController.GetWithExplicitHead),
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            table.AddRoute(
                template: "method/not/found",
                httpMethod: "GET",
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.Get),
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = true
                });

            table.AddRoute(
                template: "method/not/found",
                httpMethod: "PUT",
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.Put));

            table.AddRoute(
                template: "method/not/found/nohead",
                httpMethod: "GET",
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.GetNoHead),
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = false
                });

            table.AddRoute(
                template: "method/not/found/nohead",
                httpMethod: "PUT",
                controller: typeof(MethodNotFoundController),
                endpoint: nameof(MethodNotFoundController.PutNoHead));

            table.AddRoute(
                template: "route/not/found",
                httpMethod: "GET",
                controller: typeof(NotFoundController),
                endpoint: nameof(NotFoundController.Get),
                config: new DefaultApiRequestConfiguration
                {
                    EnableHeadForGetRequests = true
                });

            table.AddRoute(
                template: "authorization/anonymous/allowed",
                httpMethod: "GET",
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true
                });

            table.AddRoute(
                template: "authorization/policy/configured/success/provider",
                httpMethod: "GET",
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "Default"
                    }
                });

            table.AddRoute(
                template: "authorization/policy/configured/failing/provider",
                httpMethod: "GET",
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "DefaultFail"
                    }
                });

            table.AddRoute(
                template: "authorization/policy/configured/no/provider",
                httpMethod: "GET",
                controller: typeof(AuthorizationController),
                endpoint: nameof(AuthorizationController.GetWithAuthorization),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = false,
                    SupportedAuthenticationSchemes = new string[] { "Token" },
                    AuthorizationConfig = new ResourceAuthorizationConfiguration
                    {
                        Policy = "Default1"
                    }
                });

            table.AddRoute(
                template: "context/items",
                httpMethod: "GET",
                controller: typeof(ItemsController),
                endpoint: nameof(ItemsController.GetWithItems));

            table.AddRoute(
                template: "context/dump",
                httpMethod: "GET",
                controller: typeof(ContextDumpController),
                endpoint: nameof(ContextDumpController.GetDump));

            table.AddRoute(
                template: "context/dump",
                httpMethod: "POST",
                controller: typeof(ContextDumpController),
                endpoint: nameof(ContextDumpController.PostDump));



            table.AddRoute(
                template: "helper/responses/ok",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Ok));

            table.AddRoute(
                template: "helper/responses/ok/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Ok_Null));

            table.AddRoute(
                template: "helper/responses/ok/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Ok_Headers));

            table.AddRoute(
                template: "helper/responses/created",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Created));

            table.AddRoute(
                template: "helper/responses/created/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Created_Null));

            table.AddRoute(
                template: "helper/responses/created/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Created_Headers));

            table.AddRoute(
                template: "helper/responses/accepted",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Accepted));

            table.AddRoute(
                template: "helper/responses/accepted/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Accepted_Null));

            table.AddRoute(
                template: "helper/responses/accepted/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Accepted_Headers));

            table.AddRoute(
                template: "helper/responses/nocontent",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NoContent));

            table.AddRoute(
                template: "helper/responses/nocontent/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NoContent_Headers));

            table.AddRoute(
                template: "helper/responses/badrequest",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.BadRequest));

            table.AddRoute(
                template: "helper/responses/badrequest/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.BadRequest_Null));

            table.AddRoute(
                template: "helper/responses/badrequest/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.BadRequest_Headers));

            table.AddRoute(
                template: "helper/responses/unauthorized",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Unauthorized));

            table.AddRoute(
                template: "helper/responses/unauthorized/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Unauthorized_Null));

            table.AddRoute(
                template: "helper/responses/unauthorized/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Unauthorized_Headers));

            table.AddRoute(
                template: "helper/responses/forbidden",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Forbidden));

            table.AddRoute(
                template: "helper/responses/forbidden/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Forbidden_Null));

            table.AddRoute(
                template: "helper/responses/forbidden/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Forbidden_Headers));

            table.AddRoute(
                template: "helper/responses/notfound",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NotFound));

            table.AddRoute(
                template: "helper/responses/notfound/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NotFound_Null));

            table.AddRoute(
                template: "helper/responses/notfound/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.NotFound_Headers));

            table.AddRoute(
                template: "helper/responses/conflict",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Conflict));

            table.AddRoute(
                template: "helper/responses/conflict/null",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Conflict_Null));

            table.AddRoute(
                template: "helper/responses/conflict/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Conflict_Headers));

            table.AddRoute(
                template: "helper/responses/movedpermanently",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.MovedPermanently));

            table.AddRoute(
                template: "helper/responses/movedpermanently/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.MovedPermanently_Headers));

            table.AddRoute(
                template: "helper/responses/found",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Found));

            table.AddRoute(
                template: "helper/responses/found/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.Found_Headers));

            table.AddRoute(
                template: "helper/responses/permanentredirect",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.PermanentRedirect));

            table.AddRoute(
                template: "helper/responses/permanentredirect/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.PermanentRedirect_Headers));

            table.AddRoute(
                template: "helper/responses/temporaryredirect",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.TemporaryRedirect));

            table.AddRoute(
                template: "helper/responses/temporaryredirect/headers",
                httpMethod: "GET",
                controller: typeof(HelperResponsesController),
                endpoint: nameof(HelperResponsesController.TemporaryRedirect_Headers));



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
