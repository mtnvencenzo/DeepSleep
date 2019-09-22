﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestContextItemGroup.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace DeepSleep
{
    /// <summary></summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class RequestContextItemGroup<TKey, TItem>
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="RequestContextItemGroup{TKey,TItem}"/> class. 
        ///     Initializes a new instance of the <see cref="RequestContextItemGroup{TKey, TItem}"/> class.</summary>
        public RequestContextItemGroup()
        {
            Items = new Dictionary<TKey, TItem>();
        }

        #endregion

        /// <summary>Gets the items.</summary>
        /// <value>The items.</value>
        public Dictionary<TKey, TItem> Items { get; private set; }
    }
}