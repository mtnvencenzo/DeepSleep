namespace DeepSleep.Api.Web.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class NameValuePairs<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly IList<(TKey key, TValue value)> namevalues;

        public NameValuePairs()
        {
            this.namevalues = new List<(TKey key, TValue value)>();
        }

        public void Add(TKey key, TValue value)
        {
            this.namevalues.Add((key: key, value: value));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.namevalues
                .Select(n => new KeyValuePair<TKey, TValue>(n.key, n.value))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count => this.namevalues.Count;
    }
}
