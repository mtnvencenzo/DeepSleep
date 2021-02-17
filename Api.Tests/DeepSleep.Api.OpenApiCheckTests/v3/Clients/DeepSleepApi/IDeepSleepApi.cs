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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<BasicObject>> PostBasicObjectModelNoDocAttributesWithHttpMessagesAsync(int id, BasicObject body = default(BasicObject), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='id'>
        /// </param>
        /// <param name='body'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<CustomObjectIdModel>> PostCustomObjectDeepModelsWithHttpMessagesAsync(int id, CustomObjectModel body = default(CustomObjectModel), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<EnumUriObjectModelRs>> PostEnumUriModelNoDocAttributesWithHttpMessagesAsync(string explicitEnumProperty = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<EnumUriObjectModelRs>> PutEnumUriModelNoDocAttributesWithHttpMessagesAsync(string explicitEnumProperty = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<EnumUriObjectModelRs>> PatchEnumUriModelNoDocAttributesWithHttpMessagesAsync(string explicitEnumProperty = default(string), string nullableExplicitEnumProperty = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> GetEnumInRouteSimpleMemberWithHttpMessagesAsync(string enumValue, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='enumValue'>
        /// The enum value. Possible values include: 'None', 'Item1', 'Item2'
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadEnumInRouteSimpleMemberWithHttpMessagesAsync(string enumValue, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<Int32ObjectModelRq>> PostInt32UriModelNoDocAttributesWithHttpMessagesAsync(int? int32Property = default(int?), int? nullableInt32Property = default(int?), int? uInt32Property = default(int?), int? nullableUInt32Property = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?>> GetInt32ValuesOverriddenOpIdWithHttpMessagesAsync(int routeint, int? queryInt1 = default(int?), int? queryInt2 = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='routeint'>
        /// The route int.
        /// </param>
        /// <param name='queryInt1'>
        /// The query int1.
        /// </param>
        /// <param name='queryInt2'>
        /// The query int2.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadInt32ValuesOverriddenOpIdWithHttpMessagesAsync(int routeint, int? queryInt1 = default(int?), int? queryInt2 = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i list int array response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>>> GetSimpleIlistIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleIlistIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i list int array request.
        /// </summary>
        /// <param name='body'>
        /// The ints.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>>> PostSimpleIlistIntArrayRequestWithHttpMessagesAsync(IList<int?> body = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i enumerable int array response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>>> GetSimpleIenumerableIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleIenumerableIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i enumerable int array request.
        /// </summary>
        /// <param name='body'>
        /// The ints.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>>> PostSimpleIenumerableIntArrayRequestWithHttpMessagesAsync(IList<int?> body = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the array int array response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>>> GetSimpleArrayIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleArrayIntArrayResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the array int array request.
        /// </summary>
        /// <param name='body'>
        /// The ints.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<int?>>> PostSimpleArrayIntArrayRequestWithHttpMessagesAsync(IList<int?> body = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i list int array query string.
        /// </summary>
        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?>> GetSimpleIlistIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleIlistIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i enumerable int array query string.
        /// </summary>
        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?>> GetSimpleIenumerableIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleIenumerableIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the array int array query string.
        /// </summary>
        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<int?>> GetSimpleArrayIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='queryItems'>
        /// The query items.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleArrayIntArrayQuerystringWithHttpMessagesAsync(IList<int?> queryItems = default(IList<int?>), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i dictionary string string response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IDictionary<string, string>>> GetSimpleIdctionaryStringStringResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleIdctionaryStringStringResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Simples the i dictionary int string response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IDictionary<string, string>>> GetSimpleIdctionaryIntStringResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadSimpleIdctionaryIntStringResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Objects the i dictionary string dictionary object response.
        /// </summary>
        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IDictionary<string, DictionaryObject>>> GetObjectIdctionaryStringDictionaryobjectResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='count'>
        /// The count.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> HeadObjectIdctionaryStringDictionaryobjectResponseWithHttpMessagesAsync(int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Posts the touple simple.
        /// </summary>
        /// <remarks>
        /// Post Touple Simple Custom Endpoint Description
        /// </remarks>
        /// <param name='body'>
        /// The request.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> PostToupleSimpleCustomOperationWithHttpMessagesAsync(object body = default(object), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> PostBasicObjectModelReturnVoidWithHttpMessagesAsync(BasicObject body = default(BasicObject), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> PostBasicObjectModelReturnTaskWithHttpMessagesAsync(BasicObject body = default(BasicObject), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> PostBasicObjectModelReturnTaskWith202AttributeWithHttpMessagesAsync(BasicObject body = default(BasicObject), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='body'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> PostBasicObjectModelReturnTaskWith200DefaultAttributeWithHttpMessagesAsync(BasicObject body = default(BasicObject), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

    }
}