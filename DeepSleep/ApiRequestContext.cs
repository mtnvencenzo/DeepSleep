// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiRequestContext.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace DeepSleep
{
    /// <summary>The API request context.</summary>
    public class ApiRequestContext
    {
        static Dictionary<string, ResourceManager> _resxTypes = new Dictionary<string, ResourceManager>();

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="ApiRequestContext"/> class.</summary>
        public ApiRequestContext()
        {
            RequestInfo = new ApiRequestInfo();
            ProcessingInfo = new ApiProcessingInfo();
            Items = new Dictionary<string, object>();
            RouteInfo = new ApiRoutingInfo();
            ResponseInfo = new ApiResponseInfo();
            RequestAborted = new CancellationToken(false);
        }

        #endregion

        /// <summary>Gets the items.</summary>
        /// <value>The items.</value>
        public Dictionary<string, object> Items { get; private set; }

        /// <summary>Gets the request information.</summary>
        /// <value>The request information.</value>
        public ApiRequestInfo RequestInfo { get; set; }

        /// <summary>Gets or sets the response information.</summary>
        /// <value>The response information.</value>
        public ApiResponseInfo ResponseInfo { get; set; }

        /// <summary>Gets or sets the route information.</summary>
        /// <value>The route information.</value>
        public ApiRoutingInfo RouteInfo { get; set; }

        /// <summary>Gets the response information.</summary>
        /// <value>The response information.</value>
        public ApiProcessingInfo ProcessingInfo { get; set; }

        /// <summary>Gets or sets the resource identity.</summary>
        /// <value>The resource identity.</value>
        public ApiResourceConfig ResourceConfig { get; set; }

        /// <summary>Gets or sets the path base.</summary>
        /// <value>The path base.</value>
        public string PathBase { get; set; }

        /// <summary>Gets or sets the request aborted.</summary>
        /// <value>The request aborted.</value>
        public CancellationToken RequestAborted { get; set; }

        /// <summary>Gets or sets the request services.</summary>
        /// <value>The request services.</value>
        public IServiceProvider RequestServices { get; set; }

        /// <summary>Gets the resource.</summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public string GetAcceptLanguageResource(Expression<Func<string>> expression)
        {
            var expr = expression.Body as MemberExpression;
            string resx = null;

            if (expr != null)
            {
                var propertyInfo = expr.Member as PropertyInfo;
                if (propertyInfo != null)
                {
                    if (!_resxTypes.ContainsKey(propertyInfo.DeclaringType.FullName))
                    {
                        _resxTypes.Add(propertyInfo.DeclaringType.FullName, new ResourceManager(propertyInfo.DeclaringType));
                    }

                    var rm = _resxTypes[propertyInfo.DeclaringType.FullName];
                    resx = rm.GetString(propertyInfo.Name, RequestInfo?.AcceptCulture);
                }
            }
            return resx;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestContextExtensionMethods
    {
        /// <summary>Validations the state.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static ApiValidationState ValidationState(this ApiRequestContext context)
        {
            return (context?.ProcessingInfo?.Validation?.State ?? ApiValidationState.NotAttempted);
        }

        /// <summary>Adds the exception.</summary>
        /// <param name="request">The request.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static ApiRequestContext AddException(this ApiRequestContext request, Exception ex)
        {
            request.ProcessingInfo.Exceptions.Add(ex);
            return request;
        }

        /// <summary>Adds the item.</summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="contextKey">The context key.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static ApiRequestContext AddItem<TKey, TItem>(this ApiRequestContext request, string contextKey, TKey key, TItem item)
        {
            RequestContextItemGroup<TKey, TItem> itemGroup;
            if (!request.TryGetItemGroup(contextKey, out itemGroup))
            {
                itemGroup = new RequestContextItemGroup<TKey, TItem>();
                request.Items.Add(contextKey, itemGroup);
            }

            if (!itemGroup.Items.ContainsKey(key))
            {
                itemGroup.Items.Add(key, item);
            }

            return request;
        }

        /// <summary>Tries the get item.</summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="contextKey">The context key.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool TryGetItem<TKey, TItem>(this ApiRequestContext request, string contextKey, TKey key, out TItem item)
        {
            item = default(TItem);

            RequestContextItemGroup<TKey, TItem> itemGroup;
            if (!request.TryGetItemGroup(contextKey, out itemGroup))
            {
                itemGroup = new RequestContextItemGroup<TKey, TItem>();
                request.Items.Add(contextKey, itemGroup);
            }

            if (!itemGroup.Items.ContainsKey(key))
            {
                return false;
            }

            item = itemGroup.Items[key];
            return true;
        }

        /// <summary>Tries the get item group.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="contextKey">The context key.</param>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="bool" />.</returns>
        private static bool TryGetItemGroup<TItem>(this ApiRequestContext request, string contextKey, out TItem item)
        {
            item = default(TItem);

            if (!request.Items.ContainsKey(contextKey))
            {
                return false;
            }

            item = (TItem)request.Items[contextKey];
            return true;
        }
    }
}