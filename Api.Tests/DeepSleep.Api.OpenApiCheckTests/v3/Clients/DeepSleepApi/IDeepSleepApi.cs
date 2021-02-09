// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DeepSleep.Api.OpenApiCheckTests.v3
{
    using Microsoft.Rest;
    using Models;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// </summary>
    public partial interface IDeepSleepApi : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }

        /// <summary>
        /// Subscription credentials which uniquely identify client
        /// subscription.
        /// </summary>
        ServiceClientCredentials Credentials { get; }


        /// <summary>
        /// Posts the basic object model with no doc attributes.
        /// </summary>
        /// <remarks>
        /// Offically posts the basic object model.
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='id'>
        /// The route identifier.
        /// </param>
        /// <param name='body'>
        /// The request.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<BasicObject,PostBasicObjectModelNoDocAttributesHeaders>> PostBasicObjectModelNoDocAttributesWithHttpMessagesAsync(int id, BasicObject body = default(BasicObject), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Posts the enum model with URI bound enums.
        /// </summary>
        /// <remarks>
        /// Offically posts the enum object to the service
        ///
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='explicitEnumProperty'>
        /// Gets or sets the explicit enum property. Possible values include:
        /// 'None', 'Item1', 'Item2'
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<EnumUriObjectModelRs,PostEnumUriModelNoDocAttributesHeaders>> PostEnumUriModelNoDocAttributesWithHttpMessagesAsync(string explicitEnumProperty = default(string), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Puts the enum model with URI bound enums.
        /// </summary>
        /// <remarks>
        /// Offically puts the enum object to the service
        ///
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='explicitEnumProperty'>
        /// Gets or sets the explicit enum property. Possible values include:
        /// 'None', 'Item1', 'Item2'
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<EnumUriObjectModelRs,PutEnumUriModelNoDocAttributesHeaders>> PutEnumUriModelNoDocAttributesWithHttpMessagesAsync(string explicitEnumProperty = default(string), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Patch the enum model with URI bound enums.
        /// </summary>
        /// <remarks>
        /// Offically patches the enum object to the service
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='explicitEnumProperty'>
        /// The explicit enum property. Possible values include: 'None',
        /// 'Item1', 'Item2'
        /// </param>
        /// <param name='nullableExplicitEnumProperty'>
        /// The nullable explicit enum property. Possible values include:
        /// 'None', 'Item1', 'Item2'
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<EnumUriObjectModelRs,PatchEnumUriModelNoDocAttributesHeaders>> PatchEnumUriModelNoDocAttributesWithHttpMessagesAsync(string explicitEnumProperty = default(string), string nullableExplicitEnumProperty = default(string), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the enum from route.
        /// </summary>
        /// <remarks>
        /// Offically posts the enum nul object to the service
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='enumValue'>
        /// The enum value. Possible values include: 'None', 'Item1', 'Item2'
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<string,GetEnumInRouteSimpleMemberHeaders>> GetEnumInRouteSimpleMemberWithHttpMessagesAsync(string enumValue, string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='enumValue'>
        /// The enum value. Possible values include: 'None', 'Item1', 'Item2'
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadEnumInRouteSimpleMemberHeaders>> HeadEnumInRouteSimpleMemberWithHttpMessagesAsync(string enumValue, string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Posts the int32 model with URI bound values.
        /// </summary>
        /// <remarks>
        /// Offically posts the int32 uri to the service
        ///
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='int32Property'>
        /// Gets or sets the int32 property.
        /// </param>
        /// <param name='nullableInt32Property'>
        /// Gets or sets the nullable int32 property.
        /// </param>
        /// <param name='uInt32Property'>
        /// Gets or sets the u int32 property.
        /// </param>
        /// <param name='nullableUInt32Property'>
        /// Gets or sets the nullable u int32 property.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<Int32ObjectModelRq,PostInt32UriModelNoDocAttributesHeaders>> PostInt32UriModelNoDocAttributesWithHttpMessagesAsync(int? int32Property = default(int?), int? nullableInt32Property = default(int?), int? uInt32Property = default(int?), int? nullableUInt32Property = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the int32 combined route and query values.
        /// </summary>
        /// <remarks>
        /// Offically gets the combined int32 values
        /// </remarks>
        /// <param name='routeint'>
        /// The route int.
        /// </param>
        /// <param name='queryInt1'>
        /// The query int1.
        /// </param>
        /// <param name='queryInt2'>
        /// The query int2.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?,GetInt32ValuesOverriddenOpIdHeaders>> GetInt32ValuesOverriddenOpIdWithHttpMessagesAsync(int routeint, int? queryInt1 = default(int?), int? queryInt2 = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='routeint'>
        /// The route int.
        /// </param>
        /// <param name='queryInt1'>
        /// The query int1.
        /// </param>
        /// <param name='queryInt2'>
        /// The query int2.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadInt32ValuesOverriddenOpIdHeaders>> HeadInt32ValuesOverriddenOpIdWithHttpMessagesAsync(int routeint, int? queryInt1 = default(int?), int? queryInt2 = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i list int array response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>,GetSimpleIlistIntArrayResponseHeaders>> GetSimpleIlistIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadSimpleIlistIntArrayResponseHeaders>> HeadSimpleIlistIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i list int array request.
        /// </summary>
        /// <param name='body'>
        /// The ints.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>,PostSimpleIlistIntArrayRequestHeaders>> PostSimpleIlistIntArrayRequestWithHttpMessagesAsync(IList<int?> body = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i enumerable int array response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>,GetSimpleIenumerableIntArrayResponseHeaders>> GetSimpleIenumerableIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadSimpleIenumerableIntArrayResponseHeaders>> HeadSimpleIenumerableIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i enumerable int array request.
        /// </summary>
        /// <param name='body'>
        /// The ints.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>,PostSimpleIenumerableIntArrayRequestHeaders>> PostSimpleIenumerableIntArrayRequestWithHttpMessagesAsync(IList<int?> body = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the array int array response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>,GetSimpleArrayIntArrayResponseHeaders>> GetSimpleArrayIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadSimpleArrayIntArrayResponseHeaders>> HeadSimpleArrayIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the array int array request.
        /// </summary>
        /// <param name='body'>
        /// The ints.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>,PostSimpleArrayIntArrayRequestHeaders>> PostSimpleArrayIntArrayRequestWithHttpMessagesAsync(IList<int?> body = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i list int array query string.
        /// </summary>
        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?,GetSimpleIlistIntArrayQuerystringHeaders>> GetSimpleIlistIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadSimpleIlistIntArrayQuerystringHeaders>> HeadSimpleIlistIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i enumerable int array query string.
        /// </summary>
        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?,GetSimpleIenumerableIntArrayQuerystringHeaders>> GetSimpleIenumerableIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadSimpleIenumerableIntArrayQuerystringHeaders>> HeadSimpleIenumerableIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the array int array query string.
        /// </summary>
        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?,GetSimpleArrayIntArrayQuerystringHeaders>> GetSimpleArrayIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<HeadSimpleArrayIntArrayQuerystringHeaders>> HeadSimpleArrayIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Posts the basic object model with void return.
        /// </summary>
        /// <remarks>
        /// Offically posts the basic object model with void return.
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='body'>
        /// The request.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<PostBasicObjectModelReturnVoidHeaders>> PostBasicObjectModelReturnVoidWithHttpMessagesAsync(BasicObject body = default(BasicObject), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Posts the basic object model with task return.
        /// </summary>
        /// <remarks>
        /// Offically posts the basic object model with task return.
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='body'>
        /// The request.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<PostBasicObjectModelReturnTaskHeaders>> PostBasicObjectModelReturnTaskWithHttpMessagesAsync(BasicObject body = default(BasicObject), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Posts the basic object model with task 202 return.
        /// </summary>
        /// <remarks>
        /// Offically posts the basic object model with task 202 return.
        /// Here's some documentation:
        /// &lt;a href="http://www.google.com" /&gt;.
        /// </remarks>
        /// <param name='body'>
        /// The request.
        /// </param>
        /// <param name='xCorrelationId'>
        /// A correlation value that will be echoed back within the
        /// X-CorrelationId response header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationHeaderResponse<PostBasicObjectModelReturnTaskWith202AttributeHeaders>> PostBasicObjectModelReturnTaskWith202AttributeWithHttpMessagesAsync(BasicObject body = default(BasicObject), string xCorrelationId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

    }
}
