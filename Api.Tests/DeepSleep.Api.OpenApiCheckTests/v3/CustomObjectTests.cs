namespace DeepSleep.Api.OpenApiCheckTests.v3
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class CustomObjectTests : PipelineTestBase
    {
        [Fact]
        public async Task custombject_endpoints___post_custom_object_model_all_nulls()
        {
            using (var client = base.GetClient())
            {
                var body = new Models.CustomObjectModel
                {
                };

                var response = await client.PostCustomObjectDeepModelsAsync(
                    id: 1,
                    cancellationToken: default,
                    body: body).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Id.Should().Be(1);
                response.Inherited.Should().BeNull();
                response.InnerModels.Should().BeNull();
                response.KeyedModels.Should().BeNull();
                response.SecondObject.Should().BeNull();
                response.Value.Should().BeNull();
            }
        }

        [Fact]
        public async Task custombject_endpoints___post_custom_object_model()
        {
            using (var client = base.GetClient())
            {
                var body = new Models.CustomObjectModel
                {
                    Inherited = new Models.CustomObjectModel
                    {
                        Value = "testvalue1",
                        InnerModels = new List<Models.CustomObjectModel>
                        {
                            new Models.CustomObjectModel
                            {
                                Value = "testvalue2",
                                SecondObject = new Models.SecondCustomObject
                                {
                                    ThirdObject = new Models.ThirdCustomObject
                                    {
                                        IdModel = new Models.CustomObjectIdModel
                                        {
                                            Id = 2,
                                            Inherited = new Models.CustomObjectModel
                                            {
                                                Value = "testvalue3",
                                            }
                                        }
                                    }                                    
                                }
                            }
                        },
                        Inherited = new Models.CustomObjectModel
                        {
                            Value = "testvalue4",
                        },
                        SecondObject = new Models.SecondCustomObject
                        {
                            ThirdObject = new Models.ThirdCustomObject
                            {
                            }
                        }
                    },
                    Value = "MyTestValue1",
                    KeyedModels = new Dictionary<string, Models.CustomObjectModel>
                    {
                        { "key1", new Models.CustomObjectModel{ Value = "testvalue4" } },
                        { "key2", new Models.CustomObjectModel{ Value = "testvalue5" } }
                    }
                };

                var response = await client.PostCustomObjectDeepModelsAsync(
                    id: 1,
                    cancellationToken: default,
                    body: body).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Id.Should().Be(1);
                response.Value.Should().Be("MyTestValue1");
                response.Inherited.Should().NotBeNull();
                response.Inherited.Value.Should().Be("testvalue1");
                response.Inherited.InnerModels.Should().NotBeNull();
                response.Inherited.InnerModels.Should().HaveCount(1);
                response.Inherited.InnerModels[0].Value.Should().Be("testvalue2");
                response.Inherited.InnerModels[0].Inherited.Should().BeNull();
                response.Inherited.InnerModels[0].InnerModels.Should().BeNull();
                response.Inherited.InnerModels[0].KeyedModels.Should().BeNull();
                response.Inherited.InnerModels[0].Inherited.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.Should().NotBeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.Should().NotBeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.Inherited.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.KeyedModels.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.KeyedSecondModels.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.KeyedThirdModels.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.Model.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.SecondObject.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.ThirdObject.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.Should().NotBeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.InnerModels.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.KeyedModels.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.SecondObject.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.Value.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.KeyedModels.Should().BeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.Id.Should().Be(2);
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.Inherited.Should().NotBeNull();
                response.Inherited.InnerModels[0].SecondObject.ThirdObject.IdModel.Inherited.Value.Should().Be("testvalue3");

                response.KeyedModels.Should().NotBeNull();
                response.KeyedModels.Should().HaveCount(2);
                response.KeyedModels["key1"].Should().NotBeNull();
                response.KeyedModels["key1"].Value.Should().Be("testvalue4");
                response.KeyedModels["key2"].Should().NotBeNull();
                response.KeyedModels["key2"].Value.Should().Be("testvalue5");
            }
        }
    }
}
