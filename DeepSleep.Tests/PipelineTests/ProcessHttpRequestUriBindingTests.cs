﻿namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Pipeline;
    using DeepSleep.Tests.TestArtifacts;
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestUriBindingTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestUriBinding(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullInvocationContext()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = null
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullUriModelType()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = null
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForUriModelTypeFoundInServiceProvider()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();
        }

        [Fact]
        public async void ReturnsTrueForUriModelTypeActivatedWhenNotExistsInServiceProvider()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();
        }

        [Fact]
        public async void ReturnsTrueForUriModelTypeActivatedWhenServiceProviderThrowsException()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();
        }

        [Fact]
        public async void ThrowExceptionForNonDefaultConstructorType()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(NonDefaultConstructorModel)
                    }
                }
            };

            try
            {
                var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Assert.Contains($"Unable to instantiate UriModel '{typeof(NonDefaultConstructorModel).FullName}'", ex.Message);
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ReturnsTrueAndCorrectlyBindsRouteVariables()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    }
                },
                RouteInfo= new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "StringProp", "StringProp1" },
                            { "CharProp", "a" },
                            { "ByteProp", "1" },
                            { "SByteProp", "1" },
                            { "BoolProp", "true" },
                            { "IntProp", "-23" },
                            { "UIntProp", "23" },
                            { "ShortProp", "-2" },
                            { "UShortProp", "2" },
                            { "LongProp", "-4" },
                            { "ULongProp", "34" },
                            { "DoubleProp", "-8.45" },
                            { "FloatProp", "5.9" },
                            { "DecimalProp", "3.9098" },
                            { "ObjectProp", "1" },
                            { "DateTimeProp", "4/2/2007 7:23:57 PM" },
                            { "DateTimeOfsetProp", "4/2/2007 7:23:57 PM -01:00" },
                            { "TimeSpanProp", "10:00:00" },
                            { "GuidProp", "0F6AD742-3248-4229-B9A3-DC20EFA074D1" },
                            { "EnumProp", "Item1" }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardModel;
            model.Should().NotBeNull();
            model.StringProp.Should().Be("StringProp1");
            model.CharProp.Should().Be('a');
            model.ByteProp.Should().Be(1);
            model.SByteProp.Should().Be(1);
            model.BoolProp.Should().Be(true);
            model.IntProp.Should().Be(-23);
            model.UIntProp.Should().Be(23);
            model.ShortProp.Should().Be(-2);
            model.UShortProp.Should().Be(2);
            model.LongProp.Should().Be(-4);
            model.ULongProp.Should().Be(34);
            model.DoubleProp.Should().Be(-8.45);
            model.FloatProp.Should().Be(5.9f);
            model.DecimalProp.Should().Be(3.9098m);
            model.ObjectProp.Should().Be("1");
            model.DateTimeProp.ToLocalTime().ToString().Should().Be("4/2/2007 7:23:57 PM");
            model.DateTimeOfsetProp.ToString().Should().Be("4/2/2007 7:23:57 PM -01:00");
            model.TimeSpanProp.ToString().Should().Be("10:00:00");
            model.GuidProp.ToString().Should().Be("0f6ad742-3248-4229-b9a3-dc20efa074d1");
            model.EnumProp.Should().Be(StandardEnum.Item1);
        }

        [Theory]
        [InlineData("Mon, 15 Jun 2009 20:45:30 GMT")]
        [InlineData("2009-06-15T13:45:30.0000000-07:00")]
        [InlineData("2009-06-15T20:45:30.0000000-00:00")]
        public async void ReturnsTrueAndCorrectlyBindsUtcDateTimeRouteVariables(string date)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "DateTimeProp", date }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardModel;
            model.Should().NotBeNull();
            model.DateTimeProp.ToString("r").Should().Be("Mon, 15 Jun 2009 20:45:30 GMT");
        }

        [Theory]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("TRUE")]
        [InlineData("1")]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("FALSE")]
        [InlineData("0")]
        public async void ReturnsTrueAndCorrectlyBindsBoolRouteVariables(string boolValue)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "BoolProp", boolValue }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardModel;
            model.Should().NotBeNull();

            if (boolValue.ToString().ToLower() == "true" || boolValue == "1")
            {
                model.BoolProp.ToString().ToLower().Should().Be("true");
            }
            else
            {
                model.BoolProp.ToString().ToLower().Should().Be("false");
            }
        }

        [Fact]
        public async void ReturnsTrueAndCorrectlyBindsNullableRouteVariables()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardNullableModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardNullableModel)
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "CharProp", "a" },
                            { "ByteProp", "1" },
                            { "SByteProp", "1" },
                            { "BoolProp", "true" },
                            { "IntProp", "-23" },
                            { "UIntProp", "23" },
                            { "ShortProp", "-2" },
                            { "UShortProp", "2" },
                            { "LongProp", "-4" },
                            { "ULongProp", "34" },
                            { "DoubleProp", "-8.45" },
                            { "FloatProp", "5.9" },
                            { "DecimalProp", "3.9098" },
                            { "ObjectProp", "1" },
                            { "DateTimeProp", "4/2/2007 7:23:57 PM" },
                            { "DateTimeOfsetProp", "4/2/2007 7:23:57 PM -01:00" },
                            { "TimeSpanProp", "10:00:00" },
                            { "GuidProp", "0F6AD742-3248-4229-B9A3-DC20EFA074D1" },
                            { "EnumProp", "Item1" }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardNullableModel;
            model.Should().NotBeNull();
            model.CharProp.Value.Should().Be('a');
            model.ByteProp.Value.Should().Be(1);
            model.SByteProp.Value.Should().Be(1);
            model.BoolProp.Value.Should().Be(true);
            model.IntProp.Value.Should().Be(-23);
            model.UIntProp.Value.Should().Be(23);
            model.ShortProp.Value.Should().Be(-2);
            model.UShortProp.Value.Should().Be(2);
            model.LongProp.Value.Should().Be(-4);
            model.ULongProp.Value.Should().Be(34);
            model.DoubleProp.Value.Should().Be(-8.45);
            model.FloatProp.Value.Should().Be(5.9f);
            model.DecimalProp.Value.Should().Be(3.9098m);
            model.DateTimeProp.Value.ToLocalTime().ToString().Should().Be("4/2/2007 7:23:57 PM");
            model.DateTimeOfsetProp.Value.ToString().Should().Be("4/2/2007 7:23:57 PM -01:00");
            model.TimeSpanProp.Value.ToString().Should().Be("10:00:00");
            model.GuidProp.ToString().Should().Be("0f6ad742-3248-4229-b9a3-dc20efa074d1");
            model.EnumProp.Should().Be(StandardEnum.Item1);
        }

        [Fact]
        public async void ReturnsTrueAndCorrectlyBindsQueryVariables()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    },
                    QueryVariables = new Dictionary<string, string>
                    {                            
                        { "StringProp", "StringProp1" },
                        { "CharProp", "a" },
                        { "ByteProp", "1" },
                        { "SByteProp", "1" },
                        { "BoolProp", "true" },
                        { "IntProp", "-23" },
                        { "UIntProp", "23" },
                        { "ShortProp", "-2" },
                        { "UShortProp", "2" },
                        { "LongProp", "-4" },
                        { "ULongProp", "34" },
                        { "DoubleProp", "-8.45" },
                        { "FloatProp", "5.9" },
                        { "DecimalProp", "3.9098" },
                        { "ObjectProp", "1" },
                        { "DateTimeProp", "4/2/2007 7:23:57 PM" },
                        { "DateTimeOfsetProp", "4/2/2007 7:23:57 PM -01:00" },
                        { "TimeSpanProp", "10:00:00" },
                        { "GuidProp", "0F6AD742-3248-4229-B9A3-DC20EFA074D1" },
                        { "EnumProp", "Item1" }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardModel;
            model.Should().NotBeNull();
            model.StringProp.Should().Be("StringProp1");
            model.CharProp.Should().Be('a');
            model.ByteProp.Should().Be(1);
            model.SByteProp.Should().Be(1);
            model.BoolProp.Should().Be(true);
            model.IntProp.Should().Be(-23);
            model.UIntProp.Should().Be(23);
            model.ShortProp.Should().Be(-2);
            model.UShortProp.Should().Be(2);
            model.LongProp.Should().Be(-4);
            model.ULongProp.Should().Be(34);
            model.DoubleProp.Should().Be(-8.45);
            model.FloatProp.Should().Be(5.9f);
            model.DecimalProp.Should().Be(3.9098m);
            model.ObjectProp.Should().Be("1");
            model.DateTimeProp.ToLocalTime().ToString().Should().Be("4/2/2007 7:23:57 PM");
            model.DateTimeOfsetProp.ToString().Should().Be("4/2/2007 7:23:57 PM -01:00");
            model.TimeSpanProp.ToString().Should().Be("10:00:00");
            model.GuidProp.ToString().Should().Be("0f6ad742-3248-4229-b9a3-dc20efa074d1");
            model.EnumProp.Should().Be(StandardEnum.Item1);
        }

        [Fact]
        public async void ReturnsTrueAndCorrectlyBindsNullableQueryVariables()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardNullableModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardNullableModel)
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "CharProp", "a" },
                            { "ByteProp", "1" },
                            { "SByteProp", "1" },
                            { "BoolProp", "true" },
                            { "IntProp", "-23" },
                            { "UIntProp", "23" },
                            { "ShortProp", "-2" },
                            { "UShortProp", "2" },
                            { "LongProp", "-4" },
                            { "ULongProp", "34" },
                            { "DoubleProp", "-8.45" },
                            { "FloatProp", "5.9" },
                            { "DecimalProp", "3.9098" },
                            { "ObjectProp", "1" },
                            { "DateTimeProp", "4/2/2007 7:23:57 PM" },
                            { "DateTimeOfsetProp", "4/2/2007 7:23:57 PM -01:00" },
                            { "TimeSpanProp", "10:00:00" },
                            { "GuidProp", "0F6AD742-3248-4229-B9A3-DC20EFA074D1" },
                            { "EnumProp", "Item1" }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardNullableModel;
            model.Should().NotBeNull();
            model.CharProp.Value.Should().Be('a');
            model.ByteProp.Value.Should().Be(1);
            model.SByteProp.Value.Should().Be(1);
            model.BoolProp.Value.Should().Be(true);
            model.IntProp.Value.Should().Be(-23);
            model.UIntProp.Value.Should().Be(23);
            model.ShortProp.Value.Should().Be(-2);
            model.UShortProp.Value.Should().Be(2);
            model.LongProp.Value.Should().Be(-4);
            model.ULongProp.Value.Should().Be(34);
            model.DoubleProp.Value.Should().Be(-8.45);
            model.FloatProp.Value.Should().Be(5.9f);
            model.DecimalProp.Value.Should().Be(3.9098m);
            model.DateTimeProp.Value.ToLocalTime().ToString().Should().Be("4/2/2007 7:23:57 PM");
            model.DateTimeOfsetProp.Value.ToString().Should().Be("4/2/2007 7:23:57 PM -01:00");
            model.TimeSpanProp.Value.ToString().Should().Be("10:00:00");
            model.GuidProp.ToString().Should().Be("0f6ad742-3248-4229-b9a3-dc20efa074d1");
            model.EnumProp.Should().Be(StandardEnum.Item1);
        }

        [Fact]
        public async void ReturnsTrueAndCorrectlyBindsRouteVariablesAndDoesntOverwriteRouteVariableBindings()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    },
                    QueryVariables = new Dictionary<string, string>
                    {
                        { "StringProp", "StringProp1" },
                        { "CharProp", "b" },
                        { "ByteProp", "0" },
                        { "SByteProp", "1" },
                        { "BoolProp", "false" },
                        { "IntProp", "-24" },
                        { "UIntProp", "24" },
                        { "ShortProp", "-3" },
                        { "UShortProp", "3" },
                        { "LongProp", "-3" },
                        { "ULongProp", "33" },
                        { "DoubleProp", "-8.43" },
                        { "FloatProp", "5.3" },
                        { "DecimalProp", "3.9093" },
                        { "ObjectProp", "3" },
                        { "DateTimeProp", "4/2/2007 7:23:53 PM" },
                        { "DateTimeOfsetProp", "4/2/2007 7:23:53 PM -01:00" },
                        { "TimeSpanProp", "30:00:00" },
                        { "GuidProp", "3F6AD742-3248-4229-B9A3-DC20EFA074D1" },
                        { "EnumProp", "Item2" }
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "StringProp", "StringProp1" },
                            { "CharProp", "a" },
                            { "ByteProp", "1" },
                            { "SByteProp", "1" },
                            { "BoolProp", "true" },
                            { "IntProp", "-23" },
                            { "UIntProp", "23" },
                            { "ShortProp", "-2" },
                            { "UShortProp", "2" },
                            { "LongProp", "-4" },
                            { "ULongProp", "34" },
                            { "DoubleProp", "-8.45" },
                            { "FloatProp", "5.9" },
                            { "DecimalProp", "3.9098" },
                            { "ObjectProp", "1" },
                            { "DateTimeProp", "4/2/2007 7:23:57 PM" },
                            { "DateTimeOfsetProp", "4/2/2007 7:23:57 PM -01:00" },
                            { "TimeSpanProp", "10:00:00" },
                            { "GuidProp", "0F6AD742-3248-4229-B9A3-DC20EFA074D1" },
                            { "EnumProp", "Item1" }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardModel;
            model.Should().NotBeNull();
            model.StringProp.Should().Be("StringProp1");
            model.CharProp.Should().Be('a');
            model.ByteProp.Should().Be(1);
            model.SByteProp.Should().Be(1);
            model.BoolProp.Should().Be(true);
            model.IntProp.Should().Be(-23);
            model.UIntProp.Should().Be(23);
            model.ShortProp.Should().Be(-2);
            model.UShortProp.Should().Be(2);
            model.LongProp.Should().Be(-4);
            model.ULongProp.Should().Be(34);
            model.DoubleProp.Should().Be(-8.45);
            model.FloatProp.Should().Be(5.9f);
            model.DecimalProp.Should().Be(3.9098m);
            model.ObjectProp.Should().Be("1");
            model.DateTimeProp.ToLocalTime().ToString().Should().Be("4/2/2007 7:23:57 PM");
            model.DateTimeOfsetProp.ToString().Should().Be("4/2/2007 7:23:57 PM -01:00");
            model.TimeSpanProp.ToString().Should().Be("10:00:00");
            model.GuidProp.ToString().Should().Be("0f6ad742-3248-4229-b9a3-dc20efa074d1");
            model.EnumProp.Should().Be(StandardEnum.Item1);
        }

        [Fact]
        public async void ReturnsTrueAndCorrectlyBindsRouteAndQueryVariables()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    },
                    QueryVariables = new Dictionary<string, string>
                    {
                        { "LongProp", "-4" },
                        { "ULongProp", "34" },
                        { "DoubleProp", "-8.45" },
                        { "FloatProp", "5.9" },
                        { "DecimalProp", "3.9098" },
                        { "ObjectProp", "1" },
                        { "DateTimeProp", "4/2/2007 7:23:57 PM" },
                        { "DateTimeOfsetProp", "4/2/2007 7:23:57 PM -01:00" },
                        { "TimeSpanProp", "10:00:00" },
                        { "GuidProp", "0F6AD742-3248-4229-B9A3-DC20EFA074D1" },
                        { "EnumProp", "Item1" }
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "StringProp", "StringProp1" },
                            { "CharProp", "a" },
                            { "ByteProp", "1" },
                            { "SByteProp", "1" },
                            { "BoolProp", "true" },
                            { "IntProp", "-23" },
                            { "UIntProp", "23" },
                            { "ShortProp", "-2" },
                            { "UShortProp", "2" },
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().NotBeNull();

            var model = context.RequestInfo.InvocationContext.UriModel as StandardModel;
            model.Should().NotBeNull();
            model.StringProp.Should().Be("StringProp1");
            model.CharProp.Should().Be('a');
            model.ByteProp.Should().Be(1);
            model.SByteProp.Should().Be(1);
            model.BoolProp.Should().Be(true);
            model.IntProp.Should().Be(-23);
            model.UIntProp.Should().Be(23);
            model.ShortProp.Should().Be(-2);
            model.UShortProp.Should().Be(2);
            model.LongProp.Should().Be(-4);
            model.ULongProp.Should().Be(34);
            model.DoubleProp.Should().Be(-8.45);
            model.FloatProp.Should().Be(5.9f);
            model.DecimalProp.Should().Be(3.9098m);
            model.ObjectProp.Should().Be("1");
            model.DateTimeProp.ToLocalTime().ToString().Should().Be("4/2/2007 7:23:57 PM");
            model.DateTimeOfsetProp.ToString().Should().Be("4/2/2007 7:23:57 PM -01:00");
            model.TimeSpanProp.ToString().Should().Be("10:00:00");
            model.GuidProp.ToString().Should().Be("0f6ad742-3248-4229-b9a3-dc20efa074d1");
            model.EnumProp.Should().Be(StandardEnum.Item1);
        }

        [Fact]
        public async void ReturnsFalseAndErrorsWhenBindingFails()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new StandardModel());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        UriModelType = typeof(StandardModel)
                    },
                    QueryVariables = new Dictionary<string, string>
                    {
                        { "LongProp", "def" },
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        RouteVariables = new Dictionary<string, string>
                        {
                            { "IntProp", "abc" }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriBinding(mockServiceProvider.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(400);
            context.RouteInfo.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();

            context.ProcessingInfo.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().HaveCount(2);
            context.ProcessingInfo.ExtendedMessages[0].Code.Should().Be("400.000001");
            context.ProcessingInfo.ExtendedMessages[0].Message.Should().StartWith("Could not bind the route value 'abc'");
            context.ProcessingInfo.ExtendedMessages[1].Code.Should().Be("400.000001");
            context.ProcessingInfo.ExtendedMessages[1].Message.Should().StartWith("Could not bind the route value 'def'");
        }
    }
}
