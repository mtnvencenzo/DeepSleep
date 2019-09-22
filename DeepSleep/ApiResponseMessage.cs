// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseMessage.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DeepSleep
{
    /// <summary></summary>
    public class ApiResponseMessage
    {
        #region Methods

        /// <summary>Builds the response message from resource.</summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">resource - Resource is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        public static ApiResponseMessage BuildResponseMessageFromResource(string resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource", "Resource is null.");
            }

            string[] resourceParts = resource.Split(new[] { '|' });

            if (resourceParts.Length == 2)
            {
                return new ApiResponseMessage
                {
                    Code = resourceParts[0],
                    Message = resourceParts[1]
                };
            }

            throw new ArgumentException(
                "Resource '" + resource
                + "' is not in the correct format.  Resources should be pipe delimited with two parts.  "
                + "First, a decimal representing the error code.  Secondly, a short description.  "
                + "And lastly a long description of the error.  I.x 400.1|Error|My Error");
        }

        #endregion

        /// <summary>Gets or sets the code.</summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }

    /// <summary></summary>
    public static class ResponseMessageExtensionMethods
    {
        /// <summary>Clears the duplicates.</summary>
        /// <param name="messages">The messages.</param>
        public static void ClearDuplicates(this List<ApiResponseMessage> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                return;
            }

            var uniqueMessages = new List<ApiResponseMessage>();

            foreach (var message in messages)
            {
                if (!uniqueMessages.Exists(i => i.Code == message.Code && i.Message == message.Message))
                {
                    uniqueMessages.Add(message);
                }
            }

            messages.Clear();
            messages.AddRange(uniqueMessages);
        }

        /// <summary>Finds the by code.</summary>
        /// <param name="messages">The messages.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ApiResponseMessage FindByCode(this List<ApiResponseMessage> messages, string code, string type)
        {
            return messages.FirstOrDefault(i => i.Code == code);
        }

        /// <summary>Formats for header.</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static string FormatForHeader(this ApiResponseMessage message)
        {
            return string.Format("{0}|{1}", message.Code, message.Message);
        }

        /// <summary>Sorts the messages.</summary>
        /// <param name="messages">The messages.</param>
        public static void SortMessagesByCode(this List<ApiResponseMessage> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                return;
            }

            messages.Sort(CompareResponseMessages);
        }

        /// <summary>Compares the response messages.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The <see cref="int"/>.</returns>
        private static int CompareResponseMessages(ApiResponseMessage x, ApiResponseMessage y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return x.Code.CompareTo(y.Code);
        }
    }
}