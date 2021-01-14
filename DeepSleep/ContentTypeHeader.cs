namespace DeepSleep
{
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.MediaHeaderValueWithParameters" />
    [DebuggerDisplay("{ToString()}")]
    public class ContentTypeHeader : MediaHeaderValueWithParameters
    {
        private readonly string comparisonValue;

        /// <summary>Initializes a new instance of the <see cref="ContentTypeHeader"/> class.</summary>
        /// <param name="value">The value.</param>
        public ContentTypeHeader(string value)
            : base(value)
        {
            var charset = (this.MediaValue?.GetParameterValue("charset") ?? string.Empty).ToLowerInvariant();

            if (charset.Trim().StartsWith("\"") && charset.Trim().EndsWith("\""))
            {
                charset = charset
                    .Trim()
                    .Replace("\"", string.Empty);
            }

            this.Charset = charset;


            var boundary = (this.MediaValue?.GetParameterValue("boundary", false) ?? string.Empty);

            if (boundary.Trim().StartsWith("\"") && boundary.Trim().EndsWith("\""))
            {
                boundary = boundary
                    .Trim()
                    .Replace("\"", string.Empty);
            }

            this.Boundary = boundary;

            this.comparisonValue = $"{this.MediaValue?.MediaType}{this.CharsetString()}{this.BoundaryString()}";
        }

        /// <summary>Performs an implicit conversion from <see cref="ContentTypeHeader"/> to <see cref="System.String"/>.</summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(ContentTypeHeader v)
        {
            return v?.ToString();
        }

        /// <summary>Performs an implicit conversion from <see cref="System.String"/> to <see cref="ContentTypeHeader"/>.</summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ContentTypeHeader(string s)
        {
            return new ContentTypeHeader(s);
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ContentTypeHeader header, string str)
        {
            return header == new ContentTypeHeader(str);
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="header">The header.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ContentTypeHeader header, ContentTypeHeader compareTo)
        {
            return header?.ToString() == compareTo?.ToString();
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(string str, ContentTypeHeader header)
        {
            return header == new ContentTypeHeader(str);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ContentTypeHeader header, string str)
        {
            return header != new ContentTypeHeader(str);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="header">The header.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ContentTypeHeader header, ContentTypeHeader compareTo)
        {
            return header?.ToString() != compareTo?.ToString();
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(string str, ContentTypeHeader header)
        {
            return header != new ContentTypeHeader(str);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ContentTypeHeader header)
            {
                return ToString().Equals(header?.ToString());
            }
            else if (obj is string str)
            {
                return ToString().Equals(new ContentTypeHeader(str).ToString());
            }

            return ToString().Equals(obj);
        }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        public string Type => this.MediaValue?.Type ?? string.Empty;

        /// <summary>Gets the type of the sub.</summary>
        /// <value>The type of the sub.</value>
        public string SubType => this.MediaValue?.SubType ?? string.Empty;

        /// <summary>Gets the type of the media.</summary>
        /// <value>The type of the media.</value>
        public string MediaType => this.MediaValue?.MediaType ?? string.Empty;

        /// <summary>Gets or sets the charset.</summary>
        /// <value>The charset.</value>
        public virtual string Charset { get; private set; }

        /// <summary>Gets or sets the boundary.</summary>
        /// <value>The boundary.</value>
        public virtual string Boundary { get; private set; }

        /// <summary>Gets the parameter string.</summary>
        /// <value>The parameter string.</value>
        public virtual string ParameterString() => this.MediaValue?.ParameterString() ?? string.Empty;

        /// <summary>Charsets the string.</summary>
        /// <returns></returns>
        internal virtual string CharsetString()
        {
            if (string.IsNullOrWhiteSpace(Charset))
                return string.Empty;

            return $"; charset={Charset.ToLowerInvariant()}";
        }

        /// <summary>Boundaries the string.</summary>
        /// <returns></returns>
        internal virtual string BoundaryString()
        {
            if (string.IsNullOrWhiteSpace(Boundary))
                return string.Empty;

            return $"; boundary={Boundary}";
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.comparisonValue;
        }
    }
}
