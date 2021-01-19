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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Discovery.IRouteRegistrationProvider" />
    public class RequestPipelineController : IRouteRegistrationProvider
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="RequestPipelineController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public RequestPipelineController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Gets the configured after endpoint.</summary>
        /// <returns></returns>
        public RequestPipelineModel GetConfiguredAfterEndpoint()
        {
            this.contextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        /// <summary>Gets the configured before endpoint.</summary>
        /// <returns></returns>
        public RequestPipelineModel GetConfiguredBeforeEndpoint()
        {
            this.contextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        /// <summary>Gets the configured before validation.</summary>
        /// <returns></returns>
        [ApiEndpointValidation(typeof(RequestPipelineModelValidator))]
        public RequestPipelineModel GetConfiguredBeforeValidation()
        {
            this.contextResolver.GetContext().TryGetItem(nameof(RequestPipelineModelValidator), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        /// <summary>Gets the attribute after endpoint.</summary>
        /// <returns></returns>
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.AfterEndpointInvocation, 0)]
        public RequestPipelineModel GetAttributeAfterEndpoint()
        {
            this.contextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        /// <summary>Gets the attribute before endpoint.</summary>
        /// <returns></returns>
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointInvocation, 0)]
        public RequestPipelineModel GetAttributeBeforeEndpoint()
        {
            this.contextResolver.GetContext().TryGetItem(nameof(CustomRequestPipelineComponent), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        /// <summary>Gets the attribute before validation.</summary>
        /// <returns></returns>
        [ApiEndpointValidation(typeof(RequestPipelineModelValidator))]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointValidation, 0)]
        public RequestPipelineModel GetAttributeBeforeValidation()
        {
            this.contextResolver.GetContext().TryGetItem(nameof(RequestPipelineModelValidator), out string item);

            return new RequestPipelineModel
            {
                Value = item
            };
        }

        /// <summary>Gets the attributes multiple.</summary>
        /// <returns></returns>
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

        /// <summary>Gets the mixed multiple.</summary>
        /// <returns></returns>
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


        /// <summary>Gets the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    public class RequestPipelineModel
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidator : IEndpointValidator
    {
        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        public int Order { get; }

        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context?.ValidationState() != ApiValidationState.Failed)
            {
                if (context.Items.TryGetValue(nameof(CustomRequestPipelineComponent), out var beforeValidator))
                {
                    if (beforeValidator as string == "SET")
                    {
                        context.TryAddItem(nameof(RequestPipelineModelValidator), "VALIDATOR-SET");
                    }
                }
            }

            return Task.FromResult(null as IList<ApiValidationResult>);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent1 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent2 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent3 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent4 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent5 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent6 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent7 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent8 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent9 : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
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
