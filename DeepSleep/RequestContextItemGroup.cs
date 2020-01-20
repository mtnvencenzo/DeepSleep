namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary></summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class RequestContextItemGroup<TKey, TItem>
    {
        /// <summary>Initializes a new instance of the <see cref="RequestContextItemGroup{TKey,TItem}"/> class. 
        ///     Initializes a new instance of the <see cref="RequestContextItemGroup{TKey, TItem}"/> class.</summary>
        public RequestContextItemGroup()
        {
            Items = new Dictionary<TKey, TItem>();
        }

        /// <summary>Gets the items.</summary>
        /// <value>The items.</value>
        public Dictionary<TKey, TItem> Items { get; private set; }
    }
}