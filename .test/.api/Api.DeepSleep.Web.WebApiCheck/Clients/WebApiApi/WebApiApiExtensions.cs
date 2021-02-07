// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Api.DeepSleep.Web.WebApiCheck.Clients
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for WebApiApi.
    /// </summary>
    public static partial class WebApiApiExtensions
    {
            /// <summary>
            /// Posts the basic object model with no doc attributes.
            /// </summary>
            /// <remarks>
            /// Offically posts the basic object model.
            /// Here's some documentation:
            /// &lt;a href="http://www.google.com" /&gt;.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// The route identifier.
            /// </param>
            /// <param name='body'>
            /// The request.
            /// </param>
            public static BasicObject PostBasicObjectModelNoDocAttributes(this IWebApiApi operations, int id, BasicObject body = default(BasicObject))
            {
                return operations.PostBasicObjectModelNoDocAttributesAsync(id, body).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Posts the basic object model with no doc attributes.
            /// </summary>
            /// <remarks>
            /// Offically posts the basic object model.
            /// Here's some documentation:
            /// &lt;a href="http://www.google.com" /&gt;.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// The route identifier.
            /// </param>
            /// <param name='body'>
            /// The request.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<BasicObject> PostBasicObjectModelNoDocAttributesAsync(this IWebApiApi operations, int id, BasicObject body = default(BasicObject), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostBasicObjectModelNoDocAttributesWithHttpMessagesAsync(id, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
