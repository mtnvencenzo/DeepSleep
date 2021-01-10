namespace Api.DeepSleep.Controllers.RequestPipeline
{
    using global::DeepSleep;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Discovery;
    using global::DeepSleep.Pipeline;
    using global::DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RequestPipelineController : IRouteRegistrationProvider
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        public RequestPipelineController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

        public RequestPipelineModel GetConfiguredAfterEndpoint()
        {
            this.apiRequestContextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        public RequestPipelineModel GetConfiguredBeforeEndpoint()
        {
            this.apiRequestContextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        [ApiEndpointValidation(typeof(RequestPipelineModelValidator))]
        public RequestPipelineModel GetConfiguredBeforeValidation()
        {
            this.apiRequestContextResolver.GetContext().TryGetItem(nameof(RequestPipelineModelValidator), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.AfterEndpointInvocation, 0)]
        public RequestPipelineModel GetAttributeAfterEndpoint()
        {
            this.apiRequestContextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointInvocation, 0)]
        public RequestPipelineModel GetAttributeBeforeEndpoint()
        {
            this.apiRequestContextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        [ApiEndpointValidation(typeof(RequestPipelineModelValidator))]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointValidation, 0)]
        public RequestPipelineModel GetAttributeBeforeValidation()
        {
            this.apiRequestContextResolver.GetContext().TryGetItem(nameof(RequestPipelineModelValidator), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent4), PipelinePlacement.BeforeEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent5), PipelinePlacement.BeforeEndpointInvocation, 0)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent3), PipelinePlacement.BeforeEndpointInvocation, 2)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent1), PipelinePlacement.AfterEndpointInvocation, 2)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent6), PipelinePlacement.AfterEndpointInvocation, 0)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent2), PipelinePlacement.AfterEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent8), PipelinePlacement.BeforeEndpointValidation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent7), PipelinePlacement.BeforeEndpointValidation, 0)]
        public RequestPipelineModel GetAttributesMultiple()
        {
            return new RequestPipelineModel
            {
                Value = "TEST"
            };
        }

        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent4), PipelinePlacement.BeforeEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent5), PipelinePlacement.BeforeEndpointInvocation, 0)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent3), PipelinePlacement.BeforeEndpointInvocation, 2)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent1), PipelinePlacement.AfterEndpointInvocation, 3)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent6), PipelinePlacement.AfterEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent2), PipelinePlacement.AfterEndpointInvocation, 2)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent8), PipelinePlacement.BeforeEndpointValidation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent7), PipelinePlacement.BeforeEndpointValidation, 0)]
        public RequestPipelineModel GetMixedMultiple()
        {
            return new RequestPipelineModel
            {
                Value = "TEST"
            };
        }


        public Task<IList<ApiRouteRegistration>> GetRoutes(IServiceProvider serviceProvider)
        {
            var routes = new List<ApiRouteRegistration>();

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/getafterendpoint/with/static/configured/pipeline", 
                httpMethods: new[] { "GET" }, 
                controller: this.GetType(), 
                endpoint: nameof(GetConfiguredAfterEndpoint), 
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                    PipelineComponents = new List<IRequestPipelineComponent>
                    {
                        new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.AfterEndpointInvocation, 0)
                    }
                }));

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/getbeforeendpoint/with/static/configured/pipeline",
                httpMethods: new[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetConfiguredBeforeEndpoint),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                    PipelineComponents = new List<IRequestPipelineComponent>
                    {
                        new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.BeforeEndpointInvocation, 0)
                    }
                }));

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/getbeforevalidation/with/static/configured/pipeline",
                httpMethods: new[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetConfiguredBeforeValidation),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                    PipelineComponents = new List<IRequestPipelineComponent>
                    {
                        new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.BeforeEndpointValidation, 0)
                    }
                }));

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/getafterendpoint/with/attribute/configured/pipeline",
                httpMethods: new[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetAttributeAfterEndpoint),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                }));

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/getbeforeendpoint/with/attribute/configured/pipeline",
                httpMethods: new[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetAttributeBeforeEndpoint),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                }));

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/getbeforevalidation/with/attribute/configured/pipeline",
                httpMethods: new[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetAttributeBeforeValidation),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                }));

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/getAttributesMultiple/pipeline",
                httpMethods: new[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetAttributesMultiple),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                }));

            routes.Add(new ApiRouteRegistration(
                template: "requestpipeline/mixed/pipeline",
                httpMethods: new[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetMixedMultiple),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                    PipelineComponents = new List<IRequestPipelineComponent>
                    {
                        new RequestPipelineComponent<CustomRequestPipelineComponent9>(PipelinePlacement.AfterEndpointInvocation, 0)
                    }
                }));

            return Task.FromResult(routes as IList<ApiRouteRegistration>);
        }
    }

    public class RequestPipelineModel
    {
        public string Value { get; set; }
    }

    public class RequestPipelineModelValidator : IEndpointValidator
    {
        public int Order { get; }

        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            if (args?.ApiContext?.ValidationState() != ApiValidationState.Failed)
            {
                if (args.ApiContext.Items.TryGetValue(nameof(CustomRequestPipelineComponent), out var beforeValidator))
                {
                    if (beforeValidator as string == "SET")
                    {
                        args.ApiContext.TryAddItem(nameof(RequestPipelineModelValidator), "VALIDATOR-SET");
                    }
                }
            }

            return Task.FromResult(null as IList<ApiValidationResult>);
        }
    }

    public class CustomRequestPipelineComponent1 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent1), "SET-1");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent2 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent2), "SET-2");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent3 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent3), "SET-3");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent4 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent4), "SET-4");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent5 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent5), "SET-5");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent6 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent6), "SET-6");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent7 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent7), "SET-7");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent8 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent8), "SET-8");
            }

            return Task.CompletedTask;
        }
    }

    public class CustomRequestPipelineComponent9 : IPipelineComponent
    {
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent9), "SET-9");
            }

            return Task.CompletedTask;
        }
    }
}
