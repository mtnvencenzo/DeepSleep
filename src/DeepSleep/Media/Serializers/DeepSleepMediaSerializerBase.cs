namespace DeepSleep.Media.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public abstract class DeepSleepMediaSerializerBase : IDeepSleepMediaSerializer
    {
        /// <summary>Whether the formatter can read content</summary>
        public abstract bool SupportsRead { get; }

        /// <summary>Whether the formatter can write content</summary>
        public abstract bool SupportsWrite { get; }

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public abstract IList<string> ReadableMediaTypes { get; }

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public abstract IList<string> WriteableMediaTypes { get; }

        /// <summary>Determines whether this instance [can handle type] the specified type.</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance [can handle type] the specified type; otherwise, <c>false</c>.</returns>
        public abstract bool CanHandleType(Type type);

        /// <summary>Deserializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected abstract Task<object> Deserialize(Stream stream, Type objType, IMediaSerializerOptions options);

        /// <summary>Serializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected abstract Task Serialize(Stream stream, object obj, IMediaSerializerOptions options);

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        public virtual async Task<object> ReadType(Stream stream, Type objType)
        {
            return await ReadType(
                stream: stream, 
                objType: objType, 
                options: new MediaSerializerOptions()).ConfigureAwait(false);
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public virtual async Task<object> ReadType(Stream stream, Type objType, IMediaSerializerOptions options)
        {
            object obj = null;

            using (var reader = new StreamReader(stream, true))
            {
                var result = await PreSerializationReadType(stream, objType, options).ConfigureAwait(false);

                if (result?.HasRead ?? false)
                {
                    return result.ObjectResult;
                }

                obj = await Deserialize(
                    stream: stream, 
                    objType: objType,
                    options: options).ConfigureAwait(false);
            }

            return obj;
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        public virtual async Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null)
        {
            return await WriteType(
                stream: stream, 
                obj: obj, 
                options: new MediaSerializerOptions(),
                preWriteCallback: preWriteCallback).ConfigureAwait(false);
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        public virtual async Task<long> WriteType(Stream stream, object obj, IMediaSerializerOptions options, Action<long> preWriteCallback = null)
        {
            long length = 0;

            if (obj != null)
            {
                using (var ms = new MemoryStream())
                {
                    await Serialize(
                        stream: ms, 
                        obj: obj, 
                        options: options).ConfigureAwait(false);

                    length = ms.Length;
                    ms.Seek(0, SeekOrigin.Begin);

                    preWriteCallback?.Invoke(length);

                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }

            return length;
        }

        /// <summary>Pres the type of the serialization read.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected virtual Task<PreSerializationResult> PreSerializationReadType(Stream stream, Type objType, IMediaSerializerOptions options)
        {
            //if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(ArraySegment<>))
            //{
            //    var typeObject = objType.GetGenericArguments()[0];
            //    var bufferArrayType = typeObject.MakeArrayType();

            //    var bufferobj = await Deserialize(
            //        stream: stream, 
            //        objType: bufferArrayType,
            //        options: options).ConfigureAwait(false);

            //    var obj = typeof(ArraySegment<>)
            //        .MakeGenericType(typeObject)
            //        .GetConstructor(new Type[] { typeObject.MakeArrayType() })
            //        .Invoke(new object[] { bufferobj });


            //    return PreSerializationResult.Handled(obj);
            //}

            return Task.FromResult(PreSerializationResult.NotHandled());
        }
    }
}
