namespace Api.DeepSleep.Controllers.Items
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ItemsController
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        public ItemsController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

        /// <summary>Gets the with items.</summary>
        /// <returns></returns>
        [ApiEndpointValidation(typeof(ItemsRsValidator))]
        public ItemsRs GetWithItems()
        {
            var found1 = apiRequestContextResolver.GetContext().TryGetItem<string>("TestItem", out var value1);
            var found2 = apiRequestContextResolver.GetContext().TryGetItem<string>("TestItem2", out var value2);
            var found3 = apiRequestContextResolver.GetContext().TryGetItem<string>("TestItem3", out var value3);

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
        /// <summary>Validates the specified arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            if (args?.ApiContext?.ValidationState() != ApiValidationState.Failed)
            {
                args.ApiContext.TryAddItem<string>("TestItem", "TestItemValue-ShouldBe-Overridden-First");
                args.ApiContext.UpsertItem<string>("TestItem", "TestItemValue-ShouldBe-Overridden");
                args.ApiContext.UpsertItem<string>("TestItem", "TestItemValue");
                args.ApiContext.TryAddItem<string>("TestItem", "TestItemValue-ShouldBe-Overridden-First");


                args.ApiContext.UpsertItem<string>("TestItem2", "TestItemValue2");
            }

            return Task.FromResult(null as IList<ApiValidationResult>);
        }
    }
}
