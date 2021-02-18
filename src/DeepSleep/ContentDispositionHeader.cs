namespace DeepSleep
{
    using System;
    using System.Diagnostics;

    /// <summary>Represents a Multipart content disposition header</summary>
    /// <remarks>
    /// In [RFC2183] there is a discussion of the "Content-Disposition" header
    /// field and the description of the initial values allowed in this
    /// header.  Additional values may be registered with the IANA
    /// following the procedures in section 9 of [RFC2183].
    /// </remarks>
    [DebuggerDisplay("{ToString()}")]
    public class ContentDispositionHeader : MediaHeaderValueWithParameters
    {
        private readonly string comparisonValue;

        /// <summary>Initializes a new instance of the <see cref="ContentDispositionHeader"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ContentDispositionHeader(string value)
            : base(value)
        {
            this.FileNameStar = this.MediaValue?.GetParameterValue("filename*")
                ?.Trim()
                ?.Replace("\"", string.Empty)
                ?.Trim() ?? string.Empty;

            this.FileName = this.MediaValue?.GetParameterValue("filename")
                ?.Trim()
                ?.Replace("\"", string.Empty)
                ?.Trim() ?? string.Empty;

            this.Name = this.MediaValue?.GetParameterValue("name")
                ?.Trim()
                ?.Replace("\"", string.Empty)
                ?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(this.MediaValue?.Type))
            {
                this.Type = Enum.TryParse<ContentDispositionType>(this.MediaValue.Type.Replace("-", "_"), true, out var dispositionType)
                    ? dispositionType
                    : ContentDispositionType.None;
            }
            else
            {
                this.Type = ContentDispositionType.None;
            }

            var rawSize = this.MediaValue?.GetParameterValue("size")
                ?.Trim()
                ?.Replace("\"", string.Empty)
                ?.Trim();

            if (!string.IsNullOrWhiteSpace(rawSize))
            {
                this.Size = long.TryParse(rawSize, out var size)
                    ? size
                    : null as long?;
            }

            var creationDateRaw = this.MediaValue?.GetParameterValue("creation-date")
                ?.Trim()
                ?.Replace("\"", string.Empty)
                ?.Trim();

            if (!string.IsNullOrWhiteSpace(creationDateRaw))
            {
                this.CreationDate = DateTimeOffset.TryParse(creationDateRaw, out var creationDate)
                    ? creationDate
                    : null as DateTimeOffset?;
            }

            var modificationDateRaw = this.MediaValue?.GetParameterValue("modification-date")
                ?.Trim()
                ?.Replace("\"", string.Empty)
                ?.Trim();

            if (!string.IsNullOrWhiteSpace(modificationDateRaw))
            {
                this.ModificationDate = DateTimeOffset.TryParse(modificationDateRaw, out var modificationDate)
                    ? modificationDate
                    : null as DateTimeOffset?;
            }


            var readDateRaw = this.MediaValue?.GetParameterValue("read-date")
                ?.Trim()
                ?.Replace("\"", string.Empty)
                ?.Trim();

            if (!string.IsNullOrWhiteSpace(readDateRaw))
            {
                this.ReadDate = DateTimeOffset.TryParse(readDateRaw, out var readDate)
                    ? readDate
                    : null as DateTimeOffset?;
            }

            this.comparisonValue = $"{this.Type.ToString().Replace("_", "-")}{this.NameString()}{this.FileNameString()}{this.FileNameStarString()}{this.SizeString()}{this.CreationDateString()}{this.ModificationDateString()}{this.ReadDateString()}";
        }

        /// <summary>Performs an implicit conversion from <see cref="ContentDispositionHeader"/> to <see cref="System.String"/>.</summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(ContentDispositionHeader v)
        {
            return v?.ToString();
        }

        /// <summary>Performs an implicit conversion from <see cref="System.String"/> to <see cref="ContentDispositionHeader"/>.</summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ContentDispositionHeader(string s)
        {
            return new ContentDispositionHeader(s);
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ContentDispositionHeader header, string str)
        {
            return header == new ContentDispositionHeader(str);
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="header">The header.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ContentDispositionHeader header, ContentDispositionHeader compareTo)
        {
            return header?.ToString() == compareTo?.ToString();
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(string str, ContentDispositionHeader header)
        {
            return header == new ContentDispositionHeader(str);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ContentDispositionHeader header, string str)
        {
            return header != new ContentDispositionHeader(str);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="header">The header.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ContentDispositionHeader header, ContentDispositionHeader compareTo)
        {
            return header?.ToString() != compareTo?.ToString();
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(string str, ContentDispositionHeader header)
        {
            return header != new ContentDispositionHeader(str);
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
            if (obj is ContentDispositionHeader header)
            {
                return ToString().Equals(header?.ToString());
            }
            else if (obj is string str)
            {
                return ToString().Equals(new ContentDispositionHeader(str).ToString());
            }

            return ToString().Equals(obj);
        }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public ContentDispositionType Type { get; private set; }

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileName { get; private set; }

        /// <summary>Files the name string.</summary>
        /// <returns></returns>
        internal virtual string FileNameString()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return string.Empty;

            return $"; filename=\"{FileName}\"";
        }

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileNameStar { get; private set; }

        /// <summary>Files the name star string.</summary>
        /// <returns></returns>
        internal virtual string FileNameStarString()
        {
            if (string.IsNullOrWhiteSpace(FileNameStar))
                return string.Empty;

            return $"; filename*=\"{FileNameStar}\"";
        }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>Names the string.</summary>
        /// <returns></returns>
        internal virtual string NameString()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return string.Empty;

            return $"; name=\"{Name}\"";
        }

        /// <summary>Gets or sets the size.</summary>
        /// <value>The size.</value>
        public long? Size { get; private set; }

        /// <summary>Sizes the string.</summary>
        /// <returns></returns>
        internal virtual string SizeString()
        {
            if (!Size.HasValue)
                return string.Empty;

            return $"; size={Size}";
        }

        /// <summary>Gets or sets the creation date.</summary>
        /// <value>The creation date.</value>
        public DateTimeOffset? CreationDate { get; private set; }

        /// <summary>Creations the date string.</summary>
        /// <returns></returns>
        internal virtual string CreationDateString()
        {
            if (!CreationDate.HasValue)
                return string.Empty;

            return $"; creation-date=\"{CreationDate}\"";
        }

        /// <summary>Gets or sets the modification date.</summary>
        /// <value>The modification date.</value>
        public DateTimeOffset? ModificationDate { get; private set; }

        /// <summary>Modifications the date string.</summary>
        /// <returns></returns>
        internal virtual string ModificationDateString()
        {
            if (!ModificationDate.HasValue)
                return string.Empty;

            return $"; modification-date=\"{ModificationDate}\"";
        }

        /// <summary>Gets or sets the read date.</summary>
        /// <value>The read date.</value>
        public DateTimeOffset? ReadDate { get; private set; }

        /// <summary>Reads the date string.</summary>
        /// <returns></returns>
        internal virtual string ReadDateString()
        {
            if (!ReadDate.HasValue)
                return string.Empty;

            return $"; read-date=\"{ReadDate}\"";
        }

        /// <summary>Gets the parameter string.</summary>
        /// <value>The parameter string.</value>
        public virtual string ParameterString() => this.MediaValue?.ParameterString() ?? string.Empty;

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
