namespace Api.DeepSleep.Controllers.Items
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ItemsController
    {
        private readonly IApiRequestContextResolver contextResolver;

        public ItemsController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Gets the with items.</summary>
        /// <returns></returns>
        [ApiEndpointValidation(typeof(ItemsRsValidator))]
        public ItemsRs GetWithItems()
        {
            var found1 = contextResolver.GetContext().TryGetItem<string>("TestItem", out var value1);
            var found2 = contextResolver.GetContext().TryGetItem<string>("TestItem2", out var value2);
            var found3 = contextResolver.GetContext().TryGetItem<string>("TestItem3", out var value3);

            return new ItemsRs
            {
                Found1 = found1,
                Value1 = value1,

                Found2 = found2,
                Value2 = value2,

                Found3 = found3,
                Value3 = value3
            };
        }
    }

    public class ItemsRs
    {
        public string Value1 { get; set; }
        public bool Found1 { get; set; }

        public string Value2 { get; set; }
        public bool Found2 { get; set; }

        public string Value3 { get; set; }
        public bool Found3 { get; set; }
    }

    public class ItemsRsValidator : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context?.ValidationState() != ApiValidationState.Failed)
            {
                context.TryAddItem<string>("TestItem", "TestItemValue-ShouldBe-Overridden-First");
                context.UpsertItem<string>("TestItem", "TestItemValue-ShouldBe-Overridden");
                context.UpsertItem<string>("TestItem", "TestItemValue");
                context.TryAddItem<string>("TestItem", "TestItemValue-ShouldBe-Overridden-First");


                context.UpsertItem<string>("TestItem2", "TestItemValue2");
            }

            return Task.FromResult(null as IList<ApiValidationResult>);
        }
    }
}
