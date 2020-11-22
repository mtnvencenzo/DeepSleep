using System.Diagnostics;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class MediaHeaderValueWithParameters
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaHeaderValueWithParameters"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public MediaHeaderValueWithParameters(string value)
        {
            Value = value;
            MediaValue = value.GetMediaHeaderWithParameters();
        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Performs an implicit conversion from <see cref="MediaHeaderValueWithParameters"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(MediaHeaderValueWithParameters v)
        {
            return v.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="MediaHeaderValueWithParameters"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MediaHeaderValueWithParameters(string s)
        {
            return new MediaHeaderValueWithParameters(s);
        }


        /// <summary>Implements the operator ==.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MediaHeaderValueWithParameters header, string str)
        {
            return header?.Value == str;
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(string str, MediaHeaderValueWithParameters header)
        {
            return header?.Value == str;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MediaHeaderValueWithParameters header, string str)
        {
            return header?.Value != str;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(string str, MediaHeaderValueWithParameters header)
        {
            return header?.Value != str;
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Value.Equals(obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Value;
        }

        #endregion

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>Gets the values.</summary>
        /// <value>The values.</value>
        public MediaValueWithParameters MediaValue { get; private set; }
    }
}
